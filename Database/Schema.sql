-- =========================================================
-- Atta Chaki POS & Inventory Management System
-- SQLite Database Schema
-- =========================================================

PRAGMA foreign_keys = ON;

-- ============ SECURITY / USERS ============
CREATE TABLE IF NOT EXISTS Roles (
    RoleId INTEGER PRIMARY KEY AUTOINCREMENT,
    RoleName TEXT NOT NULL UNIQUE,
    Description TEXT,
    IsActive INTEGER DEFAULT 1
);

CREATE TABLE IF NOT EXISTS Users (
    UserId INTEGER PRIMARY KEY AUTOINCREMENT,
    FullName TEXT NOT NULL,
    UserName TEXT NOT NULL UNIQUE,
    PasswordHash TEXT NOT NULL,
    RoleId INTEGER,
    Phone TEXT,
    Email TEXT,
    IsActive INTEGER DEFAULT 1,
    CreatedOn TEXT DEFAULT (datetime('now')),
    FOREIGN KEY (RoleId) REFERENCES Roles(RoleId)
);

CREATE TABLE IF NOT EXISTS UserActivityLog (
    LogId INTEGER PRIMARY KEY AUTOINCREMENT,
    UserId INTEGER,
    Activity TEXT,
    ActivityOn TEXT DEFAULT (datetime('now')),
    FOREIGN KEY (UserId) REFERENCES Users(UserId)
);

-- ============ COMPANY / SETTINGS ============
CREATE TABLE IF NOT EXISTS CompanyProfile (
    CompanyId INTEGER PRIMARY KEY AUTOINCREMENT,
    CompanyName TEXT NOT NULL DEFAULT 'Al-Barkat Atta Chaki',
    Address TEXT,
    City TEXT,
    Phone TEXT,
    Email TEXT,
    NTN TEXT,
    LogoPath TEXT,
    ReceiptFooter TEXT DEFAULT 'Thank you for shopping with us!',
    CurrencySymbol TEXT DEFAULT 'Rs.'
);

-- ============ MASTERS ============
CREATE TABLE IF NOT EXISTS Categories (
    CategoryId INTEGER PRIMARY KEY AUTOINCREMENT,
    CategoryName TEXT NOT NULL UNIQUE,
    Description TEXT,
    IsActive INTEGER DEFAULT 1
);

CREATE TABLE IF NOT EXISTS Units (
    UnitId INTEGER PRIMARY KEY AUTOINCREMENT,
    UnitName TEXT NOT NULL UNIQUE,
    ShortCode TEXT,
    IsActive INTEGER DEFAULT 1
);

CREATE TABLE IF NOT EXISTS Warehouses (
    WarehouseId INTEGER PRIMARY KEY AUTOINCREMENT,
    WarehouseName TEXT NOT NULL,
    Location TEXT,
    IsActive INTEGER DEFAULT 1
);

CREATE TABLE IF NOT EXISTS Suppliers (
    SupplierId INTEGER PRIMARY KEY AUTOINCREMENT,
    SupplierName TEXT NOT NULL,
    ContactPerson TEXT,
    Phone TEXT,
    Address TEXT,
    OpeningBalance REAL DEFAULT 0,
    IsActive INTEGER DEFAULT 1
);

CREATE TABLE IF NOT EXISTS Customers (
    CustomerId INTEGER PRIMARY KEY AUTOINCREMENT,
    CustomerName TEXT NOT NULL,
    Phone TEXT,
    Address TEXT,
    OpeningBalance REAL DEFAULT 0,
    IsActive INTEGER DEFAULT 1
);

CREATE TABLE IF NOT EXISTS Designations (
    DesignationId INTEGER PRIMARY KEY AUTOINCREMENT,
    DesignationName TEXT NOT NULL UNIQUE,
    IsActive INTEGER DEFAULT 1
);

CREATE TABLE IF NOT EXISTS Employees (
    EmployeeId INTEGER PRIMARY KEY AUTOINCREMENT,
    EmployeeName TEXT NOT NULL,
    DesignationId INTEGER,
    Phone TEXT,
    Salary REAL DEFAULT 0,
    JoiningDate TEXT,
    IsActive INTEGER DEFAULT 1,
    FOREIGN KEY (DesignationId) REFERENCES Designations(DesignationId)
);

CREATE TABLE IF NOT EXISTS ExpenseCategories (
    ExpenseCategoryId INTEGER PRIMARY KEY AUTOINCREMENT,
    ExpenseCategoryName TEXT NOT NULL UNIQUE,
    IsActive INTEGER DEFAULT 1
);

CREATE TABLE IF NOT EXISTS TaxRates (
    TaxId INTEGER PRIMARY KEY AUTOINCREMENT,
    TaxName TEXT NOT NULL,
    Percentage REAL NOT NULL DEFAULT 0,
    IsActive INTEGER DEFAULT 1
);

CREATE TABLE IF NOT EXISTS Banks (
    BankId INTEGER PRIMARY KEY AUTOINCREMENT,
    BankName TEXT NOT NULL,
    AccountTitle TEXT,
    AccountNumber TEXT,
    IsActive INTEGER DEFAULT 1
);

CREATE TABLE IF NOT EXISTS PaymentMethods (
    PaymentMethodId INTEGER PRIMARY KEY AUTOINCREMENT,
    MethodName TEXT NOT NULL UNIQUE,
    IsActive INTEGER DEFAULT 1
);

CREATE TABLE IF NOT EXISTS Machines (
    MachineId INTEGER PRIMARY KEY AUTOINCREMENT,
    MachineName TEXT NOT NULL,
    Capacity TEXT,
    PurchaseDate TEXT,
    Status TEXT DEFAULT 'Working',
    IsActive INTEGER DEFAULT 1
);

CREATE TABLE IF NOT EXISTS Vehicles (
    VehicleId INTEGER PRIMARY KEY AUTOINCREMENT,
    VehicleNumber TEXT NOT NULL,
    VehicleType TEXT,
    DriverName TEXT,
    IsActive INTEGER DEFAULT 1
);

CREATE TABLE IF NOT EXISTS DiscountRules (
    DiscountId INTEGER PRIMARY KEY AUTOINCREMENT,
    DiscountName TEXT NOT NULL,
    Percentage REAL DEFAULT 0,
    IsActive INTEGER DEFAULT 1
);

CREATE TABLE IF NOT EXISTS Products (
    ProductId INTEGER PRIMARY KEY AUTOINCREMENT,
    ProductCode TEXT UNIQUE,
    ProductName TEXT NOT NULL,
    CategoryId INTEGER,
    UnitId INTEGER,
    PurchasePrice REAL DEFAULT 0,
    SalePrice REAL DEFAULT 0,
    OpeningStock REAL DEFAULT 0,
    CurrentStock REAL DEFAULT 0,
    ReorderLevel REAL DEFAULT 10,
    WarehouseId INTEGER,
    IsActive INTEGER DEFAULT 1,
    FOREIGN KEY (CategoryId) REFERENCES Categories(CategoryId),
    FOREIGN KEY (UnitId) REFERENCES Units(UnitId),
    FOREIGN KEY (WarehouseId) REFERENCES Warehouses(WarehouseId)
);

CREATE TABLE IF NOT EXISTS PriceList (
    PriceListId INTEGER PRIMARY KEY AUTOINCREMENT,
    ProductId INTEGER,
    PriceListName TEXT,
    Price REAL,
    FOREIGN KEY (ProductId) REFERENCES Products(ProductId)
);

-- ============ SALES / POS ============
CREATE TABLE IF NOT EXISTS SalesInvoice (
    SaleId INTEGER PRIMARY KEY AUTOINCREMENT,
    InvoiceNo TEXT UNIQUE,
    CustomerId INTEGER,
    SaleDate TEXT DEFAULT (datetime('now')),
    SubTotal REAL DEFAULT 0,
    DiscountAmount REAL DEFAULT 0,
    TaxAmount REAL DEFAULT 0,
    TotalAmount REAL DEFAULT 0,
    PaidAmount REAL DEFAULT 0,
    BalanceAmount REAL DEFAULT 0,
    PaymentMethodId INTEGER,
    UserId INTEGER,
    Status TEXT DEFAULT 'Completed',
    FOREIGN KEY (CustomerId) REFERENCES Customers(CustomerId),
    FOREIGN KEY (PaymentMethodId) REFERENCES PaymentMethods(PaymentMethodId),
    FOREIGN KEY (UserId) REFERENCES Users(UserId)
);

CREATE TABLE IF NOT EXISTS SalesInvoiceItems (
    SaleItemId INTEGER PRIMARY KEY AUTOINCREMENT,
    SaleId INTEGER,
    ProductId INTEGER,
    Quantity REAL,
    Rate REAL,
    Discount REAL DEFAULT 0,
    Total REAL,
    FOREIGN KEY (SaleId) REFERENCES SalesInvoice(SaleId),
    FOREIGN KEY (ProductId) REFERENCES Products(ProductId)
);

CREATE TABLE IF NOT EXISTS SalesReturn (
    ReturnId INTEGER PRIMARY KEY AUTOINCREMENT,
    ReturnNo TEXT UNIQUE,
    SaleId INTEGER,
    ReturnDate TEXT DEFAULT (datetime('now')),
    TotalAmount REAL DEFAULT 0,
    Reason TEXT,
    FOREIGN KEY (SaleId) REFERENCES SalesInvoice(SaleId)
);

CREATE TABLE IF NOT EXISTS SalesReturnItems (
    ReturnItemId INTEGER PRIMARY KEY AUTOINCREMENT,
    ReturnId INTEGER,
    ProductId INTEGER,
    Quantity REAL,
    Rate REAL,
    Total REAL,
    FOREIGN KEY (ReturnId) REFERENCES SalesReturn(ReturnId),
    FOREIGN KEY (ProductId) REFERENCES Products(ProductId)
);

-- ============ PURCHASE ============
CREATE TABLE IF NOT EXISTS PurchaseOrder (
    PoId INTEGER PRIMARY KEY AUTOINCREMENT,
    PoNo TEXT UNIQUE,
    SupplierId INTEGER,
    PoDate TEXT DEFAULT (datetime('now')),
    TotalAmount REAL DEFAULT 0,
    Status TEXT DEFAULT 'Pending',
    FOREIGN KEY (SupplierId) REFERENCES Suppliers(SupplierId)
);

CREATE TABLE IF NOT EXISTS PurchaseInvoice (
    PurchaseId INTEGER PRIMARY KEY AUTOINCREMENT,
    InvoiceNo TEXT UNIQUE,
    SupplierId INTEGER,
    PurchaseDate TEXT DEFAULT (datetime('now')),
    SubTotal REAL DEFAULT 0,
    TaxAmount REAL DEFAULT 0,
    TotalAmount REAL DEFAULT 0,
    PaidAmount REAL DEFAULT 0,
    BalanceAmount REAL DEFAULT 0,
    UserId INTEGER,
    FOREIGN KEY (SupplierId) REFERENCES Suppliers(SupplierId),
    FOREIGN KEY (UserId) REFERENCES Users(UserId)
);

CREATE TABLE IF NOT EXISTS PurchaseInvoiceItems (
    PurchaseItemId INTEGER PRIMARY KEY AUTOINCREMENT,
    PurchaseId INTEGER,
    ProductId INTEGER,
    Quantity REAL,
    Rate REAL,
    Total REAL,
    FOREIGN KEY (PurchaseId) REFERENCES PurchaseInvoice(PurchaseId),
    FOREIGN KEY (ProductId) REFERENCES Products(ProductId)
);

CREATE TABLE IF NOT EXISTS PurchaseReturn (
    ReturnId INTEGER PRIMARY KEY AUTOINCREMENT,
    ReturnNo TEXT UNIQUE,
    PurchaseId INTEGER,
    ReturnDate TEXT DEFAULT (datetime('now')),
    TotalAmount REAL DEFAULT 0,
    Reason TEXT,
    FOREIGN KEY (PurchaseId) REFERENCES PurchaseInvoice(PurchaseId)
);

CREATE TABLE IF NOT EXISTS PurchaseReturnItems (
    ReturnItemId INTEGER PRIMARY KEY AUTOINCREMENT,
    ReturnId INTEGER,
    ProductId INTEGER,
    Quantity REAL,
    Rate REAL,
    Total REAL,
    FOREIGN KEY (ReturnId) REFERENCES PurchaseReturn(ReturnId),
    FOREIGN KEY (ProductId) REFERENCES Products(ProductId)
);

-- ============ STOCK ============
CREATE TABLE IF NOT EXISTS StockAdjustment (
    AdjustmentId INTEGER PRIMARY KEY AUTOINCREMENT,
    AdjustmentNo TEXT UNIQUE,
    ProductId INTEGER,
    AdjustmentDate TEXT DEFAULT (datetime('now')),
    QuantityBefore REAL,
    QuantityAfter REAL,
    Reason TEXT,
    UserId INTEGER,
    FOREIGN KEY (ProductId) REFERENCES Products(ProductId)
);

CREATE TABLE IF NOT EXISTS StockTransfer (
    TransferId INTEGER PRIMARY KEY AUTOINCREMENT,
    TransferNo TEXT UNIQUE,
    ProductId INTEGER,
    FromWarehouseId INTEGER,
    ToWarehouseId INTEGER,
    Quantity REAL,
    TransferDate TEXT DEFAULT (datetime('now')),
    FOREIGN KEY (ProductId) REFERENCES Products(ProductId)
);

CREATE TABLE IF NOT EXISTS OpeningStockEntry (
    EntryId INTEGER PRIMARY KEY AUTOINCREMENT,
    ProductId INTEGER,
    Quantity REAL,
    EntryDate TEXT DEFAULT (datetime('now')),
    FOREIGN KEY (ProductId) REFERENCES Products(ProductId)
);

-- ============ PRODUCTION (Grinding) ============
CREATE TABLE IF NOT EXISTS ProductionEntry (
    ProductionId INTEGER PRIMARY KEY AUTOINCREMENT,
    ProductionNo TEXT UNIQUE,
    MachineId INTEGER,
    RawProductId INTEGER,
    RawQuantity REAL,
    OutputProductId INTEGER,
    OutputQuantity REAL,
    WastageQuantity REAL DEFAULT 0,
    ProductionDate TEXT DEFAULT (datetime('now')),
    FOREIGN KEY (MachineId) REFERENCES Machines(MachineId),
    FOREIGN KEY (RawProductId) REFERENCES Products(ProductId),
    FOREIGN KEY (OutputProductId) REFERENCES Products(ProductId)
);

CREATE TABLE IF NOT EXISTS WastageEntry (
    WastageId INTEGER PRIMARY KEY AUTOINCREMENT,
    ProductId INTEGER,
    Quantity REAL,
    Reason TEXT,
    WastageDate TEXT DEFAULT (datetime('now')),
    FOREIGN KEY (ProductId) REFERENCES Products(ProductId)
);

-- ============ FINANCE ============
CREATE TABLE IF NOT EXISTS ExpenseEntry (
    ExpenseId INTEGER PRIMARY KEY AUTOINCREMENT,
    ExpenseCategoryId INTEGER,
    Amount REAL,
    Description TEXT,
    ExpenseDate TEXT DEFAULT (datetime('now')),
    UserId INTEGER,
    FOREIGN KEY (ExpenseCategoryId) REFERENCES ExpenseCategories(ExpenseCategoryId)
);

CREATE TABLE IF NOT EXISTS PaymentReceive (
    PaymentId INTEGER PRIMARY KEY AUTOINCREMENT,
    CustomerId INTEGER,
    Amount REAL,
    PaymentDate TEXT DEFAULT (datetime('now')),
    PaymentMethodId INTEGER,
    Notes TEXT,
    FOREIGN KEY (CustomerId) REFERENCES Customers(CustomerId)
);

CREATE TABLE IF NOT EXISTS PaymentPay (
    PaymentId INTEGER PRIMARY KEY AUTOINCREMENT,
    SupplierId INTEGER,
    Amount REAL,
    PaymentDate TEXT DEFAULT (datetime('now')),
    PaymentMethodId INTEGER,
    Notes TEXT,
    FOREIGN KEY (SupplierId) REFERENCES Suppliers(SupplierId)
);

-- ============ SEED DATA ============
INSERT OR IGNORE INTO Roles (RoleId, RoleName, Description) VALUES
 (1, 'Administrator', 'Full access to all modules'),
 (2, 'Cashier', 'POS sales access only'),
 (3, 'Manager', 'Sales, purchase and reports access');

INSERT OR IGNORE INTO Users (UserId, FullName, UserName, PasswordHash, RoleId) VALUES
 (1, 'System Admin', 'admin', 'admin123', 1);

INSERT OR IGNORE INTO CompanyProfile (CompanyId, CompanyName, Address, City, Phone) VALUES
 (1, 'Al-Barkat Atta Chaki', 'Main Bazaar Road', 'Lahore', '0300-0000000');

INSERT OR IGNORE INTO Categories (CategoryName) VALUES
 ('Atta (Flour)'), ('Maida'), ('Suji (Semolina)'), ('Chokar (Bran)'), ('Wheat (Gandum)'), ('Besan'), ('Packaging');

INSERT OR IGNORE INTO Units (UnitName, ShortCode) VALUES
 ('Kilogram','KG'), ('Bag 10KG','BAG10'), ('Bag 20KG','BAG20'), ('Bag 40KG','BAG40'), ('Ton','TON'), ('Piece','PCS');

INSERT OR IGNORE INTO Warehouses (WarehouseName, Location) VALUES ('Main Store', 'Shop Floor'), ('Godown', 'Back Storage');

INSERT OR IGNORE INTO PaymentMethods (MethodName) VALUES ('Cash'), ('Credit / Udhaar'), ('Bank Transfer'), ('Card');

INSERT OR IGNORE INTO ExpenseCategories (ExpenseCategoryName) VALUES ('Electricity Bill'), ('Machine Maintenance'), ('Labour Wages'), ('Transport'), ('Rent'), ('Misc');

INSERT OR IGNORE INTO Designations (DesignationName) VALUES ('Chaki Operator'), ('Cashier'), ('Helper'), ('Driver'), ('Manager');

INSERT OR IGNORE INTO TaxRates (TaxName, Percentage) VALUES ('No Tax', 0), ('GST', 17);
