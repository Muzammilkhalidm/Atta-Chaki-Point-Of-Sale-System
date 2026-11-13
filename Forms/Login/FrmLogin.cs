using AttaChakiPOS.Common;
using AttaChakiPOS.Database;
using AttaChakiPOS.Forms.Main;

namespace AttaChakiPOS.Forms.Login
{
    public class FrmLogin : Form
    {
        private TextBox _txtUser = null!;
        private TextBox _txtPass = null!;

        public FrmLogin()
        {
            Text = "Atta Chaki POS - Login";
            Width = 480;
            Height = 520;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            UITheme.StyleForm(this);
            BuildLayout();
        }

        private void BuildLayout()
        {
            var card = new Panel
            {
                Width = 380,
                Height = 420,
                BackColor = UITheme.PanelBackground,
                Left = (Width - 400) / 2,
                Top = 40
            };

            var lblLogo = new Label
            {
                Text = "🌾",
                Font = new Font("Segoe UI", 40F),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 90
            };

            var lblTitle = new Label
            {
                Text = "Al-Barkat Atta Chaki",
                Font = UITheme.FontHeader,
                ForeColor = UITheme.PrimaryDark,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 40
            };
            var lblSub = new Label
            {
                Text = "POS & Inventory Management System",
                Font = UITheme.FontLabel,
                ForeColor = UITheme.TextMuted,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 30
            };

            var formPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                ColumnCount = 2,
                AutoSize = true,
                Padding = new Padding(30, 20, 30, 0)
            };
            formPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));
            formPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            formPanel.Controls.Add(new Label { Text = "Username", Font = UITheme.FontLabelBold, TextAlign = ContentAlignment.MiddleLeft, Height = UITheme.FieldRowHeight, Dock = DockStyle.Fill }, 0, 0);
            _txtUser = UITheme.MakeTextBox();
            _txtUser.Text = "admin";
            formPanel.Controls.Add(_txtUser, 1, 0);

            formPanel.Controls.Add(new Label { Text = "Password", Font = UITheme.FontLabelBold, TextAlign = ContentAlignment.MiddleLeft, Height = UITheme.FieldRowHeight, Dock = DockStyle.Fill }, 0, 1);
            _txtPass = UITheme.MakeTextBox();
            _txtPass.UseSystemPasswordChar = true;
            _txtPass.Text = "admin123";
            formPanel.Controls.Add(_txtPass, 1, 1);

            var btnLogin = UITheme.MakeButton("LOGIN", UITheme.Primary, (s, e) => DoLogin());
            btnLogin.Width = 320;
            btnLogin.Height = 44;
            btnLogin.Left = 30;
            btnLogin.Top = 300;

            var lblHint = new Label
            {
                Text = "Default: admin / admin123",
                Font = UITheme.FontLabel,
                ForeColor = UITheme.TextMuted,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Bottom,
                Height = 30
            };

            card.Controls.Add(lblHint);
            card.Controls.Add(btnLogin);
            card.Controls.Add(formPanel);
            card.Controls.Add(lblSub);
            card.Controls.Add(lblTitle);
            card.Controls.Add(lblLogo);

            Controls.Add(card);

            AcceptButton = btnLogin;
        }

        private void DoLogin()
        {
            var dt = DbHelper.GetDataTable(
                "SELECT u.UserId, u.FullName, u.UserName, u.RoleId, r.RoleName FROM Users u LEFT JOIN Roles r ON u.RoleId = r.RoleId WHERE u.UserName=@u AND u.PasswordHash=@p AND u.IsActive=1",
                new() { { "@u", _txtUser.Text.Trim() }, { "@p", _txtPass.Text.Trim() } });

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var row = dt.Rows[0];
            AppSession.UserId = Convert.ToInt32(row["UserId"]);
            AppSession.FullName = row["FullName"].ToString() ?? "";
            AppSession.UserName = row["UserName"].ToString() ?? "";
            AppSession.RoleId = Convert.ToInt32(row["RoleId"]);
            AppSession.RoleName = row["RoleName"].ToString() ?? "";

            DbHelper.ExecuteNonQuery("INSERT INTO UserActivityLog (UserId, Activity) VALUES (@u, 'Logged in')", new() { { "@u", AppSession.UserId } });

            Hide();
            var dash = new FrmDashboard();
            dash.FormClosed += (s, e) => Close();
            dash.Show();
        }
    }
}
