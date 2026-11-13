using System.Data;
using AttaChakiPOS.Common;
using AttaChakiPOS.Database;

namespace AttaChakiPOS.Forms.Transactions
{
    /// <summary>Purchase Invoice — same visual rhythm as the POS Sale screen, used to bring stock IN.</summary>
    public class FrmPurchaseInvoice : Form
    {
        private ComboBox _cmbSupplier = null!;
        private ComboBox _cmbProduct = null!;
        private TextBox _txtQty = null!;
        private TextBox _txtRate = null!;
        private DataGridView _cartGrid = null!;
        private DataTable _cart = null!;
        private Label _lblSubtotal = null!;
        private Label _lblTotal = null!;
        private TextBox _txtPaid = null!;
        private Label _lblBalance = null!;
        private Label _lblInvoiceNo = null!;

        public FrmPurchaseInvoice()
        {
            Text = "New Purchase Invoice";
            Width = 1300;
            Height = 800;
            UITheme.StyleForm(this);
            BuildCartTable();
            BuildLayout();
            LoadLookups();
        }

        private void BuildCartTable()
        {
            _cart = new DataTable();
            _cart.Columns.Add("ProductId", typeof(int));
            _cart.Columns.Add("Product", typeof(string));
            _cart.Columns.Add("Qty", typeof(double));
            _cart.Columns.Add("Rate", typeof(double));
            _cart.Columns.Add("Total", typeof(double));
        }

        private void BuildLayout()
        {
            Controls.Add(UITheme.MakeHeaderBar("Purchase Invoice — Stock In"));

            var root = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, Padding = new Padding(UITheme.FormPadding) };
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 68));
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32));
            Controls.Add(root);
            root.BringToFront();

            var leftPanel = new Panel { Dock = DockStyle.Fill };
            var entryCard = new Panel { Dock = DockStyle.Top, Height = 150, BackColor = UITheme.PanelBackground, Padding = new Padding(20), Margin = new Padding(0, 0, 16, 16) };
            var entryLayout = new TableLayoutPanel { Dock = DockStyle.Top, ColumnCount = 4, AutoSize = true };
            entryLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, UITheme.LabelWidth));
            entryLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            entryLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, UITheme.LabelWidth));
            entryLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

            entryLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, UITheme.FieldRowHeight + 8));
            entryLayout.Controls.Add(UITheme.MakeFieldLabel("Supplier"), 0, 0);
            _cmbSupplier = UITheme.MakeComboBox();
            entryLayout.Controls.Add(Wrap(_cmbSupplier), 1, 0);
            entryLayout.Controls.Add(UITheme.MakeFieldLabel("Invoice No"), 2, 0);
            _lblInvoiceNo = new Label { Text = "(auto)", Font = UITheme.FontLabelBold, ForeColor = UITheme.PrimaryDark, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill };
            entryLayout.Controls.Add(Wrap(_lblInvoiceNo), 3, 0);

            entryLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, UITheme.FieldRowHeight + 8));
            entryLayout.Controls.Add(UITheme.MakeFieldLabel("Product"), 0, 1);
            _cmbProduct = UITheme.MakeComboBox();
            _cmbProduct.SelectedIndexChanged += (s, e) => AutoFillRate();
            entryLayout.Controls.Add(Wrap(_cmbProduct), 1, 1);
            entryLayout.Controls.Add(UITheme.MakeFieldLabel("Quantity"), 2, 1);
            _txtQty = UITheme.MakeTextBox();
            _txtQty.Text = "1";
            entryLayout.Controls.Add(Wrap(_txtQty), 3, 1);

            entryLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, UITheme.FieldRowHeight + 8));
            entryLayout.Controls.Add(UITheme.MakeFieldLabel("Rate"), 0, 2);
            _txtRate = UITheme.MakeTextBox();
            entryLayout.Controls.Add(Wrap(_txtRate), 1, 2);

            var btnAdd = UITheme.MakeButton("Add to Cart  ➕", UITheme.Success, (s, e) => AddToCart());
            btnAdd.Width = 200;
            btnAdd.Margin = new Padding(UITheme.LabelWidth + 10, 10, 0, 0);

            entryCard.Controls.Add(btnAdd);
            entryCard.Controls.Add(entryLayout);
            leftPanel.Controls.Add(entryCard);

            var cartCard = new Panel { Dock = DockStyle.Fill, BackColor = UITheme.PanelBackground, Padding = new Padding(16), Margin = new Padding(0, 0, 16, 0) };
            _cartGrid = new DataGridView { Dock = DockStyle.Fill, DataSource = _cart };
            UITheme.StyleGrid(_cartGrid);
            _cartGrid.ReadOnly = true;
            var cartButtons = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 46 };
            cartButtons.Controls.Add(UITheme.MakeButton("Remove Selected", UITheme.Danger, (s, e) => RemoveSelected()));
            cartButtons.Controls.Add(UITheme.MakeButton("Clear Cart", UITheme.Warning, (s, e) => { _cart.Clear(); RecalcTotals(); }));
            cartCard.Controls.Add(_cartGrid);
            cartCard.Controls.Add(cartButtons);
            leftPanel.Controls.Add(cartCard);
            cartCard.BringToFront();
            entryCard.BringToFront();
            root.Controls.Add(leftPanel, 0, 0);

            var rightPanel = new Panel { Dock = DockStyle.Fill, BackColor = UITheme.PanelBackground, Padding = new Padding(24) };
            var lblTitle = new Label { Text = "Purchase Summary", Font = UITheme.FontSubHeader, ForeColor = UITheme.PrimaryDark, Dock = DockStyle.Top, Height = 40 };

            var summary = new TableLayoutPanel { Dock = DockStyle.Top, ColumnCount = 2, AutoSize = true, Margin = new Padding(0, 10, 0, 0) };
            summary.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            summary.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

            summary.RowStyles.Add(new RowStyle(SizeType.Absolute, 54));
            summary.Controls.Add(new Label { Text = "TOTAL:", Font = UITheme.FontHeader, ForeColor = UITheme.PrimaryDark, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill }, 0, 0);
            _lblTotal = new Label { Text = "Rs. 0", Font = UITheme.FontHeader, ForeColor = UITheme.Success, TextAlign = ContentAlignment.MiddleRight, Dock = DockStyle.Fill };
            summary.Controls.Add(_lblTotal, 1, 0);
            _lblSubtotal = _lblTotal; // purchase has no separate discount line in this simplified flow

            summary.RowStyles.Add(new RowStyle(SizeType.Absolute, 44));
            summary.Controls.Add(new Label { Text = "Paid to Supplier:", Font = UITheme.FontLabelBold, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill }, 0, 1);
            _txtPaid = new TextBox { Text = "0", Font = UITheme.FontInput, Dock = DockStyle.Fill, TextAlign = HorizontalAlignment.Right };
            _txtPaid.TextChanged += (s, e) => RecalcTotals();
            summary.Controls.Add(_txtPaid, 1, 1);

            summary.RowStyles.Add(new RowStyle(SizeType.Absolute, 44));
            summary.Controls.Add(new Label { Text = "Balance Payable:", Font = UITheme.FontLabelBold, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill }, 0, 2);
            _lblBalance = new Label { Text = "Rs. 0", Font = UITheme.FontLabelBold, ForeColor = UITheme.Danger, TextAlign = ContentAlignment.MiddleRight, Dock = DockStyle.Fill };
            summary.Controls.Add(_lblBalance, 1, 2);

            var btnSave = UITheme.MakeButton("✅  SAVE PURCHASE", UITheme.Success, (s, e) => SavePurchase());
            btnSave.Width = 280;
            btnSave.Height = 54;
            btnSave.Dock = DockStyle.Bottom;
            btnSave.Margin = new Padding(0, 30, 0, 0);

            rightPanel.Controls.Add(btnSave);
            rightPanel.Controls.Add(summary);
            rightPanel.Controls.Add(lblTitle);
            root.Controls.Add(rightPanel, 1, 0);
        }

        private Panel Wrap(Control c)
        {
            var p = new Panel { Dock = DockStyle.Fill, Height = UITheme.FieldRowHeight, Margin = new Padding(0, 4, 20, 4) };
            c.Dock = DockStyle.Fill;
            p.Controls.Add(c);
            return p;
        }

        private void LoadLookups()
        {
            var suppliers = DbHelper.GetDataTable("SELECT SupplierId, SupplierName FROM Suppliers WHERE IsActive=1");
            _cmbSupplier.DisplayMember = "SupplierName";
            _cmbSupplier.ValueMember = "SupplierId";
            _cmbSupplier.DataSource = suppliers;

            var products = DbHelper.GetDataTable("SELECT ProductId, ProductName, PurchasePrice FROM Products WHERE IsActive=1");
            _cmbProduct.DisplayMember = "ProductName";
            _cmbProduct.ValueMember = "ProductId";
            _cmbProduct.DataSource = products;

            _lblInvoiceNo.Text = DbHelper.NextDocumentNumber("PUR", "PurchaseInvoice", "InvoiceNo");
        }

        private void AutoFillRate()
        {
            if (_cmbProduct.SelectedItem is DataRowView row)
                _txtRate.Text = Convert.ToDouble(row["PurchasePrice"]).ToString("0.##");
        }

        private void AddToCart()
        {
            if (_cmbProduct.SelectedItem is not DataRowView row) return;
            if (!double.TryParse(_txtQty.Text, out var qty) || qty <= 0)
            {
                MessageBox.Show("Enter a valid quantity.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!double.TryParse(_txtRate.Text, out var rate) || rate < 0)
            {
                MessageBox.Show("Enter a valid rate.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var newRow = _cart.NewRow();
            newRow["ProductId"] = row["ProductId"];
            newRow["Product"] = row["ProductName"];
            newRow["Qty"] = qty;
            newRow["Rate"] = rate;
            newRow["Total"] = qty * rate;
            _cart.Rows.Add(newRow);

            _txtQty.Text = "1";
            RecalcTotals();
        }

        private void RemoveSelected()
        {
            if (_cartGrid.CurrentRow?.DataBoundItem is DataRowView drv)
            {
                drv.Row.Delete();
                RecalcTotals();
            }
        }

        private void RecalcTotals()
        {
            double total = 0;
            foreach (DataRow r in _cart.Rows)
                if (r.RowState != DataRowState.Deleted) total += Convert.ToDouble(r["Total"]);

            double.TryParse(_txtPaid.Text, out var paid);
            double balance = total - paid;

            _lblTotal.Text = $"Rs. {total:N2}";
            _lblBalance.Text = $"Rs. {balance:N2}";
            _lblBalance.ForeColor = balance > 0 ? UITheme.Danger : UITheme.Success;
        }

        private void SavePurchase()
        {
            if (_cart.Rows.Count == 0)
            {
                MessageBox.Show("Cart is empty. Add at least one product.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            double total = 0;
            foreach (DataRow r in _cart.Rows) total += Convert.ToDouble(r["Total"]);
            double.TryParse(_txtPaid.Text, out var paid);
            double balance = total - paid;

            string invoiceNo = _lblInvoiceNo.Text;
            object supplierId = _cmbSupplier.SelectedValue ?? DBNull.Value;

            var purchaseId = DbHelper.ExecuteInsertReturnId(
                @"INSERT INTO PurchaseInvoice (InvoiceNo, SupplierId, SubTotal, TaxAmount, TotalAmount, PaidAmount, BalanceAmount, UserId)
                  VALUES (@inv, @sup, @sub, 0, @tot, @paid, @bal, @uid)",
                new()
                {
                    {"@inv", invoiceNo}, {"@sup", supplierId}, {"@sub", total},
                    {"@tot", total}, {"@paid", paid}, {"@bal", balance}, {"@uid", AppSession.UserId}
                });

            foreach (DataRow r in _cart.Rows)
            {
                DbHelper.ExecuteNonQuery(
                    "INSERT INTO PurchaseInvoiceItems (PurchaseId, ProductId, Quantity, Rate, Total) VALUES (@pid, @prod, @qty, @rate, @tot)",
                    new()
                    {
                        {"@pid", purchaseId}, {"@prod", r["ProductId"]}, {"@qty", r["Qty"]}, {"@rate", r["Rate"]}, {"@tot", r["Total"]}
                    });

                DbHelper.ExecuteNonQuery(
                    "UPDATE Products SET CurrentStock = CurrentStock + @qty WHERE ProductId = @pid",
                    new() { {"@qty", r["Qty"]}, {"@pid", r["ProductId"]} });
            }

            MessageBox.Show($"Purchase saved successfully.\nInvoice: {invoiceNo}\nTotal: Rs. {total:N2}", "Purchase Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

            _cart.Clear();
            _txtPaid.Text = "0";
            LoadLookups();
            RecalcTotals();
        }
    }
}
