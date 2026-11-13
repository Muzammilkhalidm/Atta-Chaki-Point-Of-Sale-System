using AttaChakiPOS.Common;

namespace AttaChakiPOS.Forms.Transactions
{
    public class FrmStockTransfer : CrudFormBase
    {
        public FrmStockTransfer() : base(
            "Stock Transfer Between Warehouses",
            "StockTransfer",
            "TransferId",
            new List<FieldDef>
            {
                FieldDef.Combo("ProductId", "Product", "SELECT ProductId, ProductName FROM Products WHERE IsActive=1", "ProductId", "ProductName"),
                FieldDef.Combo("FromWarehouseId", "From Warehouse", "SELECT WarehouseId, WarehouseName FROM Warehouses WHERE IsActive=1", "WarehouseId", "WarehouseName"),
                FieldDef.Combo("ToWarehouseId", "To Warehouse", "SELECT WarehouseId, WarehouseName FROM Warehouses WHERE IsActive=1", "WarehouseId", "WarehouseName"),
                FieldDef.Number("Quantity", "Quantity")
            })
        { }
    }
}
