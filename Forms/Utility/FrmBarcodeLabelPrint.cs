using System.Data;
using AttaChakiPOS.Common;
using AttaChakiPOS.Database;

namespace AttaChakiPOS.Forms.Utility
{
    public class FrmBarcodeLabelPrint : Form
    {
        private ComboBox _cmbProduct = null!;
        private NumericUpDown _numCopies = null!;
        private Label _lblPreview = null!;

        public FrmBarcodeLabelPrint()
        {
            Text = "Barcode / Label Print";
            Width = 800;
            Height = 500;
            UITheme.StyleForm(this);
            BuildLayout();
        }

        private void BuildLayout()
        {
            Controls.Add(UITheme.MakeHeaderBar("Product Barcode / Price Label Printing"));
            var card = new Panel { Dock = DockStyle.Fill, BackColor = UITheme.PanelBackground, Padding = new Padding(UITheme.FormPadding) };

            var layout = new TableLayoutPanel { Dock = DockStyle.Top, ColumnCount = 2, AutoSize = true };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, UITheme.LabelWidth));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, UITheme.FieldRowHeight + 8));
            layout.Controls.Add(UITheme.MakeFieldLabel("Select Product"), 0, 0);
            _cmbProduct = UITheme.MakeComboBox();
            _cmbProduct.DisplayMember = "ProductName";
            _cmbProduct.ValueMember = "ProductId";
            _cmbProduct.DataSource = DbHelper.GetDataTable("SELECT ProductId, ProductName, SalePrice FROM Products WHERE IsActive=1");
            layout.Controls.Add(Wrap(_cmbProduct), 1, 0);

            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, UITheme.FieldRowHeight + 8));
            layout.Controls.Add(UITheme.MakeFieldLabel("Number of Labels"), 0, 1);
            _numCopies = new NumericUpDown { Minimum = 1, Maximum = 500, Value = 1, Height = UITheme.FieldRowHeight, Font = UITheme.FontInput, Dock = DockStyle.Fill };
            layout.Controls.Add(Wrap(_numCopies), 1, 1);

            var btnPreview = UITheme.MakeButton("Generate Preview", UITheme.Info, (s, e) => GeneratePreview());
            btnPreview.Margin = new Padding(UITheme.LabelWidth + 10, 20, 0, 20);

            _lblPreview = new Label
            {
                Dock = DockStyle.Top,
                Height = 140,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Consolas", 12F),
                TextAlign = ContentAlignment.MiddleCenter
            };

            card.Controls.Add(_lblPreview);
            card.Controls.Add(btnPreview);
            card.Controls.Add(layout);
            Controls.Add(card);
            card.BringToFront();
        }

        private Panel Wrap(Control c)
        {
            var p = new Panel { Dock = DockStyle.Fill, Height = UITheme.FieldRowHeight, Margin = new Padding(0, 4, 0, 4) };
            c.Dock = DockStyle.Fill;
            p.Controls.Add(c);
            return p;
        }

        private void GeneratePreview()
        {
            if (_cmbProduct.SelectedItem == null) return;
            var row = ((DataRowView)_cmbProduct.SelectedItem).Row;
            string name = row["ProductName"].ToString() ?? "";
            string price = Convert.ToDouble(row["SalePrice"]).ToString("N2");
            int id = Convert.ToInt32(row["ProductId"]);
            _lblPreview.Text = $"{name}\nPrice: Rs. {price}\n*{id:D8}*\n(x{_numCopies.Value} copies queued)";
        }
    }
}
