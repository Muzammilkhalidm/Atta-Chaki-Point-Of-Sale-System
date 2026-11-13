using AttaChakiPOS.Common;
using AttaChakiPOS.Database;

namespace AttaChakiPOS.Forms.Utility
{
    public class FrmCompanyProfile : Form
    {
        private TextBox _txtName = null!, _txtAddress = null!, _txtCity = null!, _txtPhone = null!, _txtEmail = null!, _txtNtn = null!, _txtFooter = null!;

        public FrmCompanyProfile()
        {
            Text = "Company Profile";
            Width = 900;
            Height = 620;
            UITheme.StyleForm(this);
            BuildLayout();
            LoadData();
        }

        private void BuildLayout()
        {
            Controls.Add(UITheme.MakeHeaderBar("Company Profile Settings"));

            var card = new Panel { Dock = DockStyle.Fill, BackColor = UITheme.PanelBackground, Padding = new Padding(UITheme.FormPadding) };

            var layout = new TableLayoutPanel { Dock = DockStyle.Top, ColumnCount = 2, AutoSize = true };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, UITheme.LabelWidth));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            int r = 0;
            AddRow(layout, ref r, "Company Name", out _txtName);
            AddRow(layout, ref r, "Address", out _txtAddress);
            AddRow(layout, ref r, "City", out _txtCity);
            AddRow(layout, ref r, "Phone", out _txtPhone);
            AddRow(layout, ref r, "Email", out _txtEmail);
            AddRow(layout, ref r, "NTN / Tax No.", out _txtNtn);
            AddRow(layout, ref r, "Receipt Footer Text", out _txtFooter);

            var btnSave = UITheme.MakeButton("Save Changes", UITheme.Success, (s, e) => SaveData());
            btnSave.Margin = new Padding(UITheme.LabelWidth + 10, 20, 0, 0);

            card.Controls.Add(btnSave);
            card.Controls.Add(layout);
            Controls.Add(card);
            card.BringToFront();
        }

        private void AddRow(TableLayoutPanel layout, ref int r, string label, out TextBox box)
        {
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, UITheme.FieldRowHeight + 8));
            layout.Controls.Add(UITheme.MakeFieldLabel(label), 0, r);
            box = UITheme.MakeTextBox();
            var wrap = new Panel { Dock = DockStyle.Fill, Height = UITheme.FieldRowHeight, Margin = new Padding(0, 4, 0, 4) };
            box.Dock = DockStyle.Fill;
            wrap.Controls.Add(box);
            layout.Controls.Add(wrap, 1, r);
            r++;
        }

        private void LoadData()
        {
            var dt = DbHelper.GetDataTable("SELECT * FROM CompanyProfile WHERE CompanyId = 1");
            if (dt.Rows.Count == 0) return;
            var row = dt.Rows[0];
            _txtName.Text = row["CompanyName"]?.ToString();
            _txtAddress.Text = row["Address"]?.ToString();
            _txtCity.Text = row["City"]?.ToString();
            _txtPhone.Text = row["Phone"]?.ToString();
            _txtEmail.Text = row["Email"]?.ToString();
            _txtNtn.Text = row["NTN"]?.ToString();
            _txtFooter.Text = row["ReceiptFooter"]?.ToString();
        }

        private void SaveData()
        {
            DbHelper.ExecuteNonQuery(
                "UPDATE CompanyProfile SET CompanyName=@n, Address=@a, City=@c, Phone=@p, Email=@e, NTN=@t, ReceiptFooter=@f WHERE CompanyId=1",
                new()
                {
                    {"@n", _txtName.Text}, {"@a", _txtAddress.Text}, {"@c", _txtCity.Text},
                    {"@p", _txtPhone.Text}, {"@e", _txtEmail.Text}, {"@t", _txtNtn.Text}, {"@f", _txtFooter.Text}
                });
            MessageBox.Show("Company profile updated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
