using AttaChakiPOS.Common;
using AttaChakiPOS.Database;

namespace AttaChakiPOS.Forms.Utility
{
    public class FrmChangePassword : Form
    {
        private TextBox _txtOld = null!, _txtNew = null!, _txtConfirm = null!;

        public FrmChangePassword()
        {
            Text = "Change Password";
            Width = 700;
            Height = 420;
            UITheme.StyleForm(this);
            BuildLayout();
        }

        private void BuildLayout()
        {
            Controls.Add(UITheme.MakeHeaderBar("Change My Password"));
            var card = new Panel { Dock = DockStyle.Fill, BackColor = UITheme.PanelBackground, Padding = new Padding(UITheme.FormPadding) };

            var layout = new TableLayoutPanel { Dock = DockStyle.Top, ColumnCount = 2, AutoSize = true };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, UITheme.LabelWidth));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            AddRow(layout, "Current Password", out _txtOld, true);
            AddRow(layout, "New Password", out _txtNew, true);
            AddRow(layout, "Confirm New Password", out _txtConfirm, true);

            var btn = UITheme.MakeButton("Update Password", UITheme.Success, (s, e) => DoChange());
            btn.Margin = new Padding(UITheme.LabelWidth + 10, 20, 0, 0);

            card.Controls.Add(btn);
            card.Controls.Add(layout);
            Controls.Add(card);
            card.BringToFront();
        }

        private void AddRow(TableLayoutPanel layout, string label, out TextBox box, bool isPassword)
        {
            int r = layout.RowCount;
            layout.RowCount = r + 1;
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, UITheme.FieldRowHeight + 8));
            layout.Controls.Add(UITheme.MakeFieldLabel(label), 0, r);
            box = UITheme.MakeTextBox();
            box.UseSystemPasswordChar = isPassword;
            var wrap = new Panel { Dock = DockStyle.Fill, Height = UITheme.FieldRowHeight, Margin = new Padding(0, 4, 0, 4) };
            box.Dock = DockStyle.Fill;
            wrap.Controls.Add(box);
            layout.Controls.Add(wrap, 1, r);
        }

        private void DoChange()
        {
            var current = DbHelper.ExecuteScalar("SELECT PasswordHash FROM Users WHERE UserId=@id", new() { { "@id", Common.AppSession.UserId } });
            if (current?.ToString() != _txtOld.Text)
            {
                MessageBox.Show("Current password is incorrect.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(_txtNew.Text) || _txtNew.Text != _txtConfirm.Text)
            {
                MessageBox.Show("New password and confirmation do not match.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DbHelper.ExecuteNonQuery("UPDATE Users SET PasswordHash=@p WHERE UserId=@id", new() { { "@p", _txtNew.Text }, { "@id", Common.AppSession.UserId } });
            MessageBox.Show("Password updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
        }
    }
}
