using AttaChakiPOS.Common;
using AttaChakiPOS.Database;

namespace AttaChakiPOS.Forms.Transactions
{
    /// <summary>Records a chaki grinding run: raw material (wheat) consumed -> finished product (atta) produced.</summary>
    public class FrmProductionEntry : Form
    {
        private ComboBox _cmbMachine = null!;
        private ComboBox _cmbRawProduct = null!;
        private TextBox _txtRawQty = null!;
        private ComboBox _cmbOutputProduct = null!;
        private TextBox _txtOutputQty = null!;
        private TextBox _txtWastage = null!;
        private DataGridView _grid = null!;

        public FrmProductionEntry()
        {
            Text = "Production Entry (Chaki Grinding)";
            Width = 1150;
            Height = 750;
            UITheme.StyleForm(this);
            BuildLayout();
            LoadLookups();
            RefreshGrid();
        }

        private void BuildLayout()
        {
            Controls.Add(UITheme.MakeHeaderBar("Production Entry — Wheat to Atta Grinding"));

            var root = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2, Padding = new Padding(UITheme.FormPadding) };
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            Controls.Add(root);
            root.BringToFront();

            var card = new Panel { Dock = DockStyle.Top, AutoSize = true, BackColor = UITheme.PanelBackground, Padding = new Padding(20), Margin = new Padding(0, 0, 0, 16) };
            var layout = new TableLayoutPanel { Dock = DockStyle.Top, ColumnCount = 4, AutoSize = true };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, UITheme.LabelWidth));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, UITheme.LabelWidth));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, UITheme.FieldRowHeight + 8));
            layout.Controls.Add(UITheme.MakeFieldLabel("Machine"), 0, 0);
            _cmbMachine = UITheme.MakeComboBox();
            layout.Controls.Add(Wrap(_cmbMachine), 1, 0);
            layout.Controls.Add(UITheme.MakeFieldLabel("Wastage Qty"), 2, 0);
            _txtWastage = UITheme.MakeTextBox();
            _txtWastage.Text = "0";
            layout.Controls.Add(Wrap(_txtWastage), 3, 0);

            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, UITheme.FieldRowHeight + 8));
            layout.Controls.Add(UITheme.MakeFieldLabel("Raw Material (Wheat)"), 0, 1);
            _cmbRawProduct = UITheme.MakeComboBox();
            layout.Controls.Add(Wrap(_cmbRawProduct), 1, 1);
            layout.Controls.Add(UITheme.MakeFieldLabel("Raw Quantity"), 2, 1);
            _txtRawQty = UITheme.MakeTextBox();
            layout.Controls.Add(Wrap(_txtRawQty), 3, 1);

            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, UITheme.FieldRowHeight + 8));
            layout.Controls.Add(UITheme.MakeFieldLabel("Output Product (Atta)"), 0, 2);
            _cmbOutputProduct = UITheme.MakeComboBox();
            layout.Controls.Add(Wrap(_cmbOutputProduct), 1, 2);
            layout.Controls.Add(UITheme.MakeFieldLabel("Output Quantity"), 2, 2);
            _txtOutputQty = UITheme.MakeTextBox();
            layout.Controls.Add(Wrap(_txtOutputQty), 3, 2);

            var btnSave = UITheme.MakeButton("Record Production", UITheme.Success, (s, e) => SaveProduction());
            btnSave.Width = 220;
            btnSave.Margin = new Padding(UITheme.LabelWidth + 10, 12, 0, 0);

            card.Controls.Add(btnSave);
            card.Controls.Add(layout);
            root.Controls.Add(card, 0, 0);

            var gridCard = new Panel { Dock = DockStyle.Fill, BackColor = UITheme.PanelBackground, Padding = new Padding(16) };
            _grid = new DataGridView { Dock = DockStyle.Fill };
            UITheme.StyleGrid(_grid);
            gridCard.Controls.Add(_grid);
            root.Controls.Add(gridCard, 0, 1);
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
            var machines = DbHelper.GetDataTable("SELECT MachineId, MachineName FROM Machines WHERE IsActive=1");
            _cmbMachine.DisplayMember = "MachineName";
            _cmbMachine.ValueMember = "MachineId";
            _cmbMachine.DataSource = machines;

            var products = DbHelper.GetDataTable("SELECT ProductId, ProductName FROM Products WHERE IsActive=1");
            _cmbRawProduct.DisplayMember = "ProductName";
            _cmbRawProduct.ValueMember = "ProductId";
            _cmbRawProduct.DataSource = products;

            var products2 = DbHelper.GetDataTable("SELECT ProductId, ProductName FROM Products WHERE IsActive=1");
            _cmbOutputProduct.DisplayMember = "ProductName";
            _cmbOutputProduct.ValueMember = "ProductId";
            _cmbOutputProduct.DataSource = products2;
        }

        private void RefreshGrid()
        {
            var dt = DbHelper.GetDataTable(
                @"SELECT p.ProductionNo AS 'Batch No', m.MachineName AS Machine, r.ProductName AS 'Raw Material',
                         p.RawQuantity AS 'Raw Qty', o.ProductName AS Output, p.OutputQuantity AS 'Output Qty',
                         p.WastageQuantity AS Wastage, p.ProductionDate AS Date
                  FROM ProductionEntry p
                  LEFT JOIN Machines m ON p.MachineId = m.MachineId
                  LEFT JOIN Products r ON p.RawProductId = r.ProductId
                  LEFT JOIN Products o ON p.OutputProductId = o.ProductId
                  ORDER BY p.ProductionId DESC");
            _grid.DataSource = dt;
        }

        private void SaveProduction()
        {
            if (!double.TryParse(_txtRawQty.Text, out var rawQty) || rawQty <= 0)
            {
                MessageBox.Show("Enter a valid raw material quantity.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!double.TryParse(_txtOutputQty.Text, out var outQty) || outQty <= 0)
            {
                MessageBox.Show("Enter a valid output quantity.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            double.TryParse(_txtWastage.Text, out var wastage);

            string batchNo = DbHelper.NextDocumentNumber("PROD", "ProductionEntry", "ProductionNo");

            DbHelper.ExecuteNonQuery(
                @"INSERT INTO ProductionEntry (ProductionNo, MachineId, RawProductId, RawQuantity, OutputProductId, OutputQuantity, WastageQuantity)
                  VALUES (@no, @m, @rp, @rq, @op, @oq, @w)",
                new()
                {
                    {"@no", batchNo}, {"@m", _cmbMachine.SelectedValue ?? DBNull.Value},
                    {"@rp", _cmbRawProduct.SelectedValue ?? DBNull.Value}, {"@rq", rawQty},
                    {"@op", _cmbOutputProduct.SelectedValue ?? DBNull.Value}, {"@oq", outQty}, {"@w", wastage}
                });

            // Raw material consumed -> reduce stock
            DbHelper.ExecuteNonQuery("UPDATE Products SET CurrentStock = CurrentStock - @q WHERE ProductId = @id",
                new() { {"@q", rawQty}, {"@id", _cmbRawProduct.SelectedValue ?? DBNull.Value} });

            // Output produced -> increase stock
            DbHelper.ExecuteNonQuery("UPDATE Products SET CurrentStock = CurrentStock + @q WHERE ProductId = @id",
                new() { {"@q", outQty}, {"@id", _cmbOutputProduct.SelectedValue ?? DBNull.Value} });

            MessageBox.Show($"Production batch {batchNo} recorded successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            _txtRawQty.Clear();
            _txtOutputQty.Clear();
            _txtWastage.Text = "0";
            RefreshGrid();
        }
    }
}
