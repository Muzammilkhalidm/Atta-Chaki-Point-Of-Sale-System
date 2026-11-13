using System.Data;
using AttaChakiPOS.Common;
using AttaChakiPOS.Database;

namespace AttaChakiPOS.Forms.Transactions
{
    /// <summary>
    /// Main POS Sale screen — Product entry on the left in the same aligned
    /// label/input rhythm as every other form, live cart grid, and a totals
    /// panel on the right that is always fully visible (never half-cut).
    /// </summary>
    public class FrmPOSSale : Form
    {
        private ComboBox _cmbCustomer = null!;
        private ComboBox _cmbProduct = null!;
        private TextBox _txtQty = null!;
        private TextBox _txtRate = null!;
        private ComboBox _cmbPaymentMethod = null!;
        private DataGridView _cartGrid = null!;
        private DataTable _cart = null!;

        private Label _lblSubtotal = null!;
        private Label _lblDiscount = null!;
        private Label _lblTotal = null!;
        private TextBox _txtDiscount = null!;
        private TextBox _txtPaid = null!;
        private Label _lblBalance = null!;
        private Label _lblInvoiceNo = null!;

        public FrmPOSSale()
        {
            Text = "New POS Sale";
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
            Controls.Add(UITheme.MakeHeaderBar("Point of Sale — New Sale"));

            var root = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, Padding = new Padding(UITheme.FormPadding) };
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 68));
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32));
            Controls.Add(root);
            root.BringToFront();

            // ============ LEFT: entry + cart ============
            var leftPanel = new Panel { Dock = DockStyle.Fill };

            // AutoSize (not a fixed Height) so this card always grows to fit
            // both the 3 field rows AND the button row below them — a fixed
            // Height here previously left no room for the button at all.
            var entryCard = new Panel { Dock = DockStyle.Top, AutoSize = true, BackColor = UITheme.PanelBackground, Padding = new Padding(20), Margin = new Padding(0, 0, 16, 16) };
            var entryLayout = new TableLayoutPanel { Dock = DockStyle.Top, ColumnCount = 4, AutoSize = true };
            entryLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, UITheme.LabelWidth));
            entryLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            entryLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, UITheme.LabelWidth));
            entryLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

            entryLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, UITheme.FieldRowHeight + 8));
            entryLayout.Controls.Add(UITheme.MakeFieldLabel("Customer"), 0, 0);
            _cmbCustomer = UITheme.MakeComboBox();
            entryLayout.Controls.Add(Wrap(_cmbCustomer), 1, 0);
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
            entryLayout.Controls.Add(UITheme.MakeFieldLabel("Payment Method"), 2, 2);
            _cmbPaymentMethod = UITheme.MakeComboBox();
            entryLayout.Controls.Add(Wrap(_cmbPaymentMethod), 3, 2);

            // The button needs its own Dock=Top row (a raw Button with only a
            // Margin set, and no Dock, is never placed by the docking engine —
            // it just sits at (0,0) and gets painted over by entryLayout, which
            // is exactly why it used to disappear behind the Customer/Product
            // fields). Wrapping it in a docked FlowLayoutPanel fixes that.
            var btnAdd = UITheme.MakeButton("Add to Cart  ➕", UITheme.Success, (s, e) => AddToCart());
            btnAdd.Width = 200;
            btnAdd.Height = UITheme.ButtonHeight + 6;

            var btnRow = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, FlowDirection = FlowDirection.LeftToRight, Margin = new Padding(0, 12, 0, 0) };
            btnRow.Controls.Add(btnAdd);

            // Controls docked to the same edge stack with the LAST one added
            // sitting closest to that edge, so add the button row first and
            // the field grid last: fields render on top, the button sits
            // directly beneath them, nothing overlaps.
            entryCard.Controls.Add(btnRow);
            entryCard.Controls.Add(entryLayout);

            var cartCard = new Panel { Dock = DockStyle.Fill, BackColor = UITheme.PanelBackground, Padding = new Padding(16), Margin = new Padding(0, 0, 16, 0) };
            _cartGrid = new DataGridView { Dock = DockStyle.Fill, DataSource = _cart };
            UITheme.StyleGrid(_cartGrid);
            _cartGrid.ReadOnly = true;

            var cartButtons = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 46 };
            cartButtons.Controls.Add(UITheme.MakeButton("Remove Selected", UITheme.Danger, (s, e) => RemoveSelected()));
            cartButtons.Controls.Add(UITheme.MakeButton("Clear Cart", UITheme.Warning, (s, e) => { _cart.Clear(); RecalcTotals(); }));

            cartCard.Controls.Add(_cartGrid);
            cartCard.Controls.Add(cartButtons);
            // entryCard (Dock=Top) is added before cartCard (Dock=Fill), so the
            // entry form sits above the cart grid, which fills the rest —
            // no BringToFront needed once the add order matches the visual order.
            leftPanel.Controls.Add(entryCard);
            leftPanel.Controls.Add(cartCard);

            root.Controls.Add(leftPanel, 0, 0);

            // ============ RIGHT: totals / checkout ============
            var rightPanel = new Panel { Dock = DockStyle.Fill, BackColor = UITheme.PanelBackground, Padding = new Padding(24) };

            var lblTitle = new Label { Text = "Order Summary", Font = UITheme.FontSubHeader, ForeColor = UITheme.PrimaryDark, Dock = DockStyle.Top, Height = 40 };

            var summary = new TableLayoutPanel { Dock = DockStyle.Top, ColumnCount = 2, AutoSize = true, Margin = new Padding(0, 10, 0, 0) };
            summary.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            summary.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

            summary.RowStyles.Add(new RowStyle(SizeType.Absolute, 44));
            summary.Controls.Add(new Label { Text = "Subtotal:", Font = UITheme.FontLabelBold, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill }, 0, 0);
            _lblSubtotal = new Label { Text = "Rs. 0", Font = UITheme.FontLabelBold, TextAlign = ContentAlignment.MiddleRight, Dock = DockStyle.Fill };
            summary.Controls.Add(_lblSubtotal, 1, 0);

            summary.RowStyles.Add(new RowStyle(SizeType.Absolute, 44));
            summary.Controls.Add(new Label { Text = "Discount (Rs.):", Font = UITheme.FontLabelBold, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill }, 0, 1);
            _txtDiscount = new TextBox { Text = "0", Font = UITheme.FontInput, Dock = DockStyle.Fill, TextAlign = HorizontalAlignment.Right };
            _txtDiscount.TextChanged += (s, e) => RecalcTotals();
            summary.Controls.Add(_txtDiscount, 1, 1);

            summary.RowStyles.Add(new RowStyle(SizeType.Absolute, 54));
            var lblTotalCaption = new Label { Text = "TOTAL:", Font = UITheme.FontHeader, ForeColor = UITheme.PrimaryDark, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill };
            summary.Controls.Add(lblTotalCaption, 0, 2);
            _lblTotal = new Label { Text = "Rs. 0", Font = UITheme.FontHeader, ForeColor = UITheme.Success, TextAlign = ContentAlignment.MiddleRight, Dock = DockStyle.Fill };
            summary.Controls.Add(_lblTotal, 1, 2);

            summary.RowStyles.Add(new RowStyle(SizeType.Absolute, 44));
            summary.Controls.Add(new Label { Text = "Paid Amount:", Font = UITheme.FontLabelBold, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill }, 0, 3);
            _txtPaid = new TextBox { Text = "0", Font = UITheme.FontInput, Dock = DockStyle.Fill, TextAlign = HorizontalAlignment.Right };
            _txtPaid.TextChanged += (s, e) => RecalcTotals();
            summary.Controls.Add(_txtPaid, 1, 3);

            summary.RowStyles.Add(new RowStyle(SizeType.Absolute, 44));
            summary.Controls.Add(new Label { Text = "Balance / Change:", Font = UITheme.FontLabelBold, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill }, 0, 4);
            _lblBalance = new Label { Text = "Rs. 0", Font = UITheme.FontLabelBold, ForeColor = UITheme.Danger, TextAlign = ContentAlignment.MiddleRight, Dock = DockStyle.Fill };
            summary.Controls.Add(_lblBalance, 1, 4);

            var btnCheckout = UITheme.MakeButton("✅  COMPLETE SALE", UITheme.Success, (s, e) => CompleteSale());
            btnCheckout.Width = 280;
            btnCheckout.Height = 54;
            btnCheckout.Dock = DockStyle.Bottom;
            btnCheckout.Margin = new Padding(0, 30, 0, 0);

            rightPanel.Controls.Add(btnCheckout);
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
            var customers = DbHelper.GetDataTable("SELECT CustomerId, CustomerName FROM Customers WHERE IsActive=1");
            _cmbCustomer.DisplayMember = "CustomerName";
            _cmbCustomer.ValueMember = "CustomerId";
            _cmbCustomer.DataSource = customers;

            var products = DbHelper.GetDataTable("SELECT ProductId, ProductName, SalePrice, CurrentStock FROM Products WHERE IsActive=1");
            _cmbProduct.DisplayMember = "ProductName";
            _cmbProduct.ValueMember = "ProductId";
            _cmbProduct.DataSource = products;

            var methods = DbHelper.GetDataTable("SELECT PaymentMethodId, MethodName FROM PaymentMethods WHERE IsActive=1");
            _cmbPaymentMethod.DisplayMember = "MethodName";
            _cmbPaymentMethod.ValueMember = "PaymentMethodId";
            _cmbPaymentMethod.DataSource = methods;

            _lblInvoiceNo.Text = DbHelper.NextDocumentNumber("INV", "SalesInvoice", "InvoiceNo");
        }

        private void AutoFillRate()
        {
            if (_cmbProduct.SelectedItem is DataRowView row)
                _txtRate.Text = Convert.ToDouble(row["SalePrice"]).ToString("0.##");
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

            double stock = Convert.ToDouble(row["CurrentStock"]);
            if (qty > stock)
            {
                if (MessageBox.Show($"Only {stock} in stock. Add anyway?", "Low Stock", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
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
            double subtotal = 0;
            foreach (DataRow r in _cart.Rows)
                if (r.RowState != DataRowState.Deleted) subtotal += Convert.ToDouble(r["Total"]);

            double.TryParse(_txtDiscount.Text, out var discount);
            double total = Math.Max(0, subtotal - discount);
            double.TryParse(_txtPaid.Text, out var paid);
            double balance = paid - total;

            _lblSubtotal.Text = $"Rs. {subtotal:N2}";
            _lblTotal.Text = $"Rs. {total:N2}";
            _lblBalance.Text = $"Rs. {balance:N2}";
            _lblBalance.ForeColor = balance < 0 ? UITheme.Danger : UITheme.Success;
        }

        private void CompleteSale()
        {
            if (_cart.Rows.Count == 0)
            {
                MessageBox.Show("Cart is empty. Add at least one product.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            double subtotal = 0;
            foreach (DataRow r in _cart.Rows) subtotal += Convert.ToDouble(r["Total"]);
            double.TryParse(_txtDiscount.Text, out var discount);
            double total = Math.Max(0, subtotal - discount);
            double.TryParse(_txtPaid.Text, out var paid);
            double balance = total - paid;

            string invoiceNo = _lblInvoiceNo.Text;
            object customerId = _cmbCustomer.SelectedValue ?? DBNull.Value;
            object paymentMethodId = _cmbPaymentMethod.SelectedValue ?? DBNull.Value;

            var saleId = DbHelper.ExecuteInsertReturnId(
                @"INSERT INTO SalesInvoice (InvoiceNo, CustomerId, SubTotal, DiscountAmount, TaxAmount, TotalAmount, PaidAmount, BalanceAmount, PaymentMethodId, UserId, Status)
                  VALUES (@inv, @cust, @sub, @disc, 0, @tot, @paid, @bal, @pm, @uid, 'Completed')",
                new()
                {
                    {"@inv", invoiceNo}, {"@cust", customerId}, {"@sub", subtotal}, {"@disc", discount},
                    {"@tot", total}, {"@paid", paid}, {"@bal", balance}, {"@pm", paymentMethodId}, {"@uid", AppSession.UserId}
                });

            foreach (DataRow r in _cart.Rows)
            {
                DbHelper.ExecuteNonQuery(
                    "INSERT INTO SalesInvoiceItems (SaleId, ProductId, Quantity, Rate, Discount, Total) VALUES (@sid, @pid, @qty, @rate, 0, @tot)",
                    new()
                    {
                        {"@sid", saleId}, {"@pid", r["ProductId"]}, {"@qty", r["Qty"]}, {"@rate", r["Rate"]}, {"@tot", r["Total"]}
                    });

                DbHelper.ExecuteNonQuery(
                    "UPDATE Products SET CurrentStock = CurrentStock - @qty WHERE ProductId = @pid",
                    new() { {"@qty", r["Qty"]}, {"@pid", r["ProductId"]} });
            }

            MessageBox.Show($"Sale completed successfully.\nInvoice: {invoiceNo}\nTotal: Rs. {total:N2}", "Sale Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);

            _cart.Clear();
            _txtDiscount.Text = "0";
            _txtPaid.Text = "0";
            LoadLookups();
            RecalcTotals();
        }
    }
}
