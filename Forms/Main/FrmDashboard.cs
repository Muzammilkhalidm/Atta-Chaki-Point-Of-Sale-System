using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using AttaChakiPOS.Common;
using AttaChakiPOS.Database;
using AttaChakiPOS.Forms.Masters;
using AttaChakiPOS.Forms.Transactions;
using AttaChakiPOS.Forms.Reports;
using AttaChakiPOS.Forms.Utility;
using AttaChakiPOS.Forms.Login;

namespace AttaChakiPOS.Forms.Main
{
    public class FrmDashboard : Form
    {
        private Panel _sidebar = null!;
        private Panel _contentHost = null!;
        private FlowLayoutPanel _kpiRow = null!;
        private Button? _activeNavButton;

        public FrmDashboard()
        {
            Text = "Al-Barkat Atta Chaki - Dashboard";
            WindowState = FormWindowState.Maximized;
            RightToLeft = RightToLeft.No;
            RightToLeftLayout = false;

            UITheme.StyleForm(this);
            BuildLayout();
            ShowHome();
        }

        private void BuildLayout()
        {
            // Clear existing controls if re-building
            Controls.Clear();

            // 1. Top Bar
            var topBar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 65,
                BackColor = UITheme.PrimaryDark,
                Padding = new Padding(20, 0, 20, 0)
            };

            var lblCompany = new Label
            {
                Text = "🌾  Al-Barkat Atta Chaki  |  POS & Inventory Management",
                Font = UITheme.FontSubHeader,
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Left,
                AutoSize = true
            };

            var rightHeaderPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Right,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Padding = new Padding(0, 14, 0, 0)
            };

            var lblUser = new Label
            {
                Text = $"👤 {AppSession.FullName} ({AppSession.RoleName})",
                Font = UITheme.FontLabelBold,
                ForeColor = Color.FromArgb(220, 230, 242),
                TextAlign = ContentAlignment.MiddleLeft,
                AutoSize = true,
                Margin = new Padding(0, 8, 15, 0)
            };

            var btnLogout = UITheme.MakeButton("Logout 🚪", UITheme.Danger, (s, e) => Logout());
            btnLogout.Height = 36;
            btnLogout.Width = 100;
            btnLogout.Margin = new Padding(0);

            rightHeaderPanel.Controls.Add(lblUser);
            rightHeaderPanel.Controls.Add(btnLogout);

            topBar.Controls.Add(lblCompany);
            topBar.Controls.Add(rightHeaderPanel);

            // 2. Left Sidebar
            _sidebar = new Panel
            {
                Dock = DockStyle.Left,
                Width = 240,
                BackColor = UITheme.Primary,
                AutoScroll = true
            };
            BuildSidebar();

            // 3. Main Content Panel (Takes whatever space remains on the right)
            _contentHost = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = UITheme.PageBackground
            };

            // Order matters for Docking calculations in WinForms:
            // Add Fill control FIRST, then Top/Left docking controls.
            Controls.Add(_contentHost);
            Controls.Add(_sidebar);
            Controls.Add(topBar);
        }

        private void BuildSidebar()
        {
            var flow = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(0, 10, 0, 25)
            };

            flow.Controls.Add(NavHeader("HOME"));
            flow.Controls.Add(NavButton("🏠  Dashboard Home", () => ShowHome()));

            flow.Controls.Add(NavHeader("SALES / POS"));
            flow.Controls.Add(NavButton("🛒  New POS Sale", () => OpenChild(new FrmPOSSale())));
            flow.Controls.Add(NavButton("📄  Sales Invoice List", () => OpenChild(new FrmSalesInvoiceList())));
            flow.Controls.Add(NavButton("↩️  Sales Return", () => OpenChild(new FrmSalesReturn())));

            flow.Controls.Add(NavHeader("PURCHASE"));
            flow.Controls.Add(NavButton("📝  Purchase Order", () => OpenChild(new FrmPurchaseOrder())));
            flow.Controls.Add(NavButton("📦  Purchase Invoice", () => OpenChild(new FrmPurchaseInvoice())));
            flow.Controls.Add(NavButton("↩️  Purchase Return", () => OpenChild(new FrmPurchaseReturn())));

            flow.Controls.Add(NavHeader("PRODUCTION (CHAKI)"));
            flow.Controls.Add(NavButton("⚙️  Production Entry", () => OpenChild(new FrmProductionEntry())));
            flow.Controls.Add(NavButton("🗑️  Wastage Entry", () => OpenChild(new FrmWastageEntry())));
            flow.Controls.Add(NavButton("🧾  Raw Material", () => OpenChild(new FrmRawMaterialConsumption())));

            flow.Controls.Add(NavHeader("STOCK / INVENTORY"));
            flow.Controls.Add(NavButton("🔄  Stock Adjustment", () => OpenChild(new FrmStockAdjustment())));
            flow.Controls.Add(NavButton("🚚  Stock Transfer", () => OpenChild(new FrmStockTransfer())));
            flow.Controls.Add(NavButton("📥  Opening Stock", () => OpenChild(new FrmOpeningStock())));

            flow.Controls.Add(NavHeader("FINANCE"));
            flow.Controls.Add(NavButton("💵  Expense Entry", () => OpenChild(new FrmExpenseEntry())));
            flow.Controls.Add(NavButton("💰  Receive Payment", () => OpenChild(new FrmPaymentReceive())));
            flow.Controls.Add(NavButton("💸  Pay Supplier", () => OpenChild(new FrmPaymentPay())));
            flow.Controls.Add(NavButton("📒  Customer Ledger", () => OpenChild(new FrmCustomerLedger())));
            flow.Controls.Add(NavButton("📒  Supplier Ledger", () => OpenChild(new FrmSupplierLedger())));
            flow.Controls.Add(NavButton("🧮  Daily Cash Book", () => OpenChild(new FrmDailyCashBook())));

            flow.Controls.Add(NavHeader("MASTER DATA"));
            flow.Controls.Add(NavButton("🏷️  Products / Items", () => OpenChild(new FrmProductMaster())));
            flow.Controls.Add(NavButton("📂  Categories", () => OpenChild(new FrmCategoryMaster())));
            flow.Controls.Add(NavButton("📏  Units", () => OpenChild(new FrmUnitMaster())));
            flow.Controls.Add(NavButton("🏭  Warehouses", () => OpenChild(new FrmWarehouseMaster())));
            flow.Controls.Add(NavButton("🚛  Suppliers", () => OpenChild(new FrmSupplierMaster())));
            flow.Controls.Add(NavButton("🙋  Customers", () => OpenChild(new FrmCustomerMaster())));

            _sidebar.Controls.Add(flow);
        }

        private Label NavHeader(string text) => new Label
        {
            Text = text,
            Font = new Font("Segoe UI", 8F, FontStyle.Bold),
            ForeColor = UITheme.Accent,
            AutoSize = false,
            Width = 220,
            Height = 25,
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(12, 0, 0, 0)
        };

        private Button NavButton(string text, Action onClick)
        {
            var btn = new Button
            {
                Text = text,
                Width = 220,
                Height = 34,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(12, 0, 0, 0),
                FlatStyle = FlatStyle.Flat,
                BackColor = UITheme.Primary,
                ForeColor = Color.White,
                Font = UITheme.FontLabel,
                Cursor = Cursors.Hand,
                Margin = new Padding(5, 1, 5, 1)
            };

            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = UITheme.AccentDark;

            btn.Click += (s, e) =>
            {
                if (_activeNavButton != null)
                {
                    _activeNavButton.BackColor = UITheme.Primary;
                }
                btn.BackColor = UITheme.AccentDark;
                _activeNavButton = btn;
                onClick();
            };

            return btn;
        }

        private void OpenChild(Form child)
        {
            _contentHost.SuspendLayout();
            foreach (Control c in _contentHost.Controls.Cast<Control>().ToList())
            {
                _contentHost.Controls.Remove(c);
                c.Dispose();
            }

            child.TopLevel = false;
            child.FormBorderStyle = FormBorderStyle.None;
            child.Dock = DockStyle.Fill;
            _contentHost.Controls.Add(child);
            _contentHost.ResumeLayout();

            child.Visible = true;
            child.Show();
            child.BringToFront();
        }

        private async void ShowHome()
        {
            foreach (Control c in _contentHost.Controls.Cast<Control>().ToList())
            {
                _contentHost.Controls.Remove(c);
                c.Dispose();
            }

            var scroller = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(25)
            };

            var headerPanel = new Panel { Dock = DockStyle.Top, Height = 60 };
            var lblWelcome = new Label
            {
                Text = $"👋 Welcome back, {AppSession.FullName}",
                Font = UITheme.FontHeader,
                ForeColor = UITheme.PrimaryDark,
                Dock = DockStyle.Top,
                Height = 32,
                TextAlign = ContentAlignment.MiddleLeft
            };

            var lblDate = new Label
            {
                Text = DateTime.Now.ToString("dddd, dd MMMM yyyy"),
                Font = UITheme.FontLabel,
                ForeColor = UITheme.TextMuted,
                Dock = DockStyle.Top,
                Height = 22,
                TextAlign = ContentAlignment.MiddleLeft
            };

            headerPanel.Controls.Add(lblDate);
            headerPanel.Controls.Add(lblWelcome);

            _kpiRow = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                Margin = new Padding(0, 10, 0, 20)
            };

            var quickActionContainer = new Panel { Dock = DockStyle.Top, Height = 100, Margin = new Padding(0, 10, 0, 0) };
            var lblQuick = new Label
            {
                Text = "Quick Actions",
                Font = UITheme.FontSubHeader,
                ForeColor = UITheme.PrimaryDark,
                Dock = DockStyle.Top,
                Height = 30,
                TextAlign = ContentAlignment.MiddleLeft
            };

            var quickFlow = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true
            };

            quickFlow.Controls.Add(MakeActionButton("New POS Sale", UITheme.Success, () => OpenChild(new FrmPOSSale())));
            quickFlow.Controls.Add(MakeActionButton("New Purchase", UITheme.Info, () => OpenChild(new FrmPurchaseInvoice())));
            quickFlow.Controls.Add(MakeActionButton("Add Product", UITheme.Warning, () => OpenChild(new FrmProductMaster())));
            quickFlow.Controls.Add(MakeActionButton("View Reports", UITheme.Primary, () => OpenChild(new FrmSalesReport())));

            quickActionContainer.Controls.Add(quickFlow);
            quickActionContainer.Controls.Add(lblQuick);

            scroller.Controls.Add(quickActionContainer);
            scroller.Controls.Add(_kpiRow);
            scroller.Controls.Add(headerPanel);

            _contentHost.Controls.Add(scroller);

            await LoadKpisAsync();
        }

        private Button MakeActionButton(string text, Color themeColor, Action onClick)
        {
            var btn = UITheme.MakeButton(text, themeColor, (s, e) => onClick());
            btn.Height = 38;
            btn.Width = 130;
            btn.Margin = new Padding(0, 5, 12, 5);
            return btn;
        }

        private async Task LoadKpisAsync()
        {
            _kpiRow.Controls.Clear();
            try
            {
                var todaySales = await Task.Run(() => DbHelper.ExecuteScalar("SELECT IFNULL(SUM(TotalAmount),0) FROM SalesInvoice WHERE date(SaleDate)=date('now')"));
                var productCount = await Task.Run(() => DbHelper.ExecuteScalar("SELECT COUNT(*) FROM Products WHERE IsActive=1"));
                var lowStock = await Task.Run(() => DbHelper.ExecuteScalar("SELECT COUNT(*) FROM Products WHERE CurrentStock <= ReorderLevel AND IsActive=1"));
                var todayExpense = await Task.Run(() => DbHelper.ExecuteScalar("SELECT IFNULL(SUM(Amount),0) FROM ExpenseEntry WHERE date(ExpenseDate)=date('now')"));

                _kpiRow.Controls.Add(KpiCard("Today's Sales", $"Rs. {Convert.ToDouble(todaySales):N0}", UITheme.Success));
                _kpiRow.Controls.Add(KpiCard("Active Products", Convert.ToString(productCount) ?? "0", UITheme.Info));
                _kpiRow.Controls.Add(KpiCard("Low Stock Items", Convert.ToString(lowStock) ?? "0", UITheme.Danger));
                _kpiRow.Controls.Add(KpiCard("Today's Expenses", $"Rs. {Convert.ToDouble(todayExpense):N0}", UITheme.Warning));
            }
            catch
            {
                _kpiRow.Controls.Add(KpiCard("Today's Sales", "Rs. 0", UITheme.Success));
            }
        }

        private Panel KpiCard(string label, string value, Color accent)
        {
            var card = new Panel
            {
                Width = 210,
                Height = 90,
                BackColor = UITheme.PanelBackground,
                Margin = new Padding(0, 0, 15, 15)
            };

            var strip = new Panel
            {
                Dock = DockStyle.Left,
                Width = 5,
                BackColor = accent
            };

            var contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(12, 8, 8, 8)
            };

            var lblLabel = new Label
            {
                Text = label.ToUpper(),
                Font = UITheme.FontKpiLabel,
                ForeColor = UITheme.TextMuted,
                Dock = DockStyle.Top,
                Height = 20,
                TextAlign = ContentAlignment.MiddleLeft
            };

            var lblValue = new Label
            {
                Text = value,
                Font = UITheme.FontKpiValue,
                ForeColor = UITheme.TextDark,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };

            contentPanel.Controls.Add(lblValue);
            contentPanel.Controls.Add(lblLabel);

            card.Controls.Add(contentPanel);
            card.Controls.Add(strip);

            return card;
        }

        private void Logout()
        {
            if (MessageBox.Show("Are you sure you want to log out?", "Confirm Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            var login = new FrmLogin();
            login.Show();
            Close();
        }
    }
}