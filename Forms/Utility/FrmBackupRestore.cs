using AttaChakiPOS.Common;
using AttaChakiPOS.Database;

namespace AttaChakiPOS.Forms.Utility
{
    public class FrmBackupRestore : Form
    {
        private Label _lblStatus = null!;

        public FrmBackupRestore()
        {
            Text = "Backup & Restore";
            Width = 750;
            Height = 420;
            UITheme.StyleForm(this);
            BuildLayout();
        }

        private void BuildLayout()
        {
            Controls.Add(UITheme.MakeHeaderBar("Database Backup & Restore"));
            var card = new Panel { Dock = DockStyle.Fill, BackColor = UITheme.PanelBackground, Padding = new Padding(UITheme.FormPadding) };

            var lblInfo = new Label
            {
                Text = "Take a full backup of the SQLite database file, or restore from a previously saved backup.\nAlways close all open forms before restoring.",
                Dock = DockStyle.Top,
                Height = 60,
                Font = UITheme.FontLabel,
                ForeColor = UITheme.TextMuted
            };

            var flow = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 60, Margin = new Padding(0, 20, 0, 0) };
            flow.Controls.Add(UITheme.MakeButton("Backup Now", UITheme.Success, (s, e) => DoBackup()));
            flow.Controls.Add(UITheme.MakeButton("Restore Backup", UITheme.Warning, (s, e) => DoRestore()));

            _lblStatus = new Label { Dock = DockStyle.Top, Height = 40, Font = UITheme.FontLabelBold, ForeColor = UITheme.Success, Margin = new Padding(0, 20, 0, 0) };

            card.Controls.Add(_lblStatus);
            card.Controls.Add(flow);
            card.Controls.Add(lblInfo);
            Controls.Add(card);
            card.BringToFront();
        }

        private void DoBackup()
        {
            using var sfd = new SaveFileDialog { Filter = "Database Backup (*.db)|*.db", FileName = $"AttaChakiPOS_Backup_{DateTime.Now:yyyyMMdd_HHmm}.db" };
            if (sfd.ShowDialog() != DialogResult.OK) return;
            try
            {
                string dbFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AttaChakiPOS.db");
                File.Copy(dbFile, sfd.FileName, true);
                _lblStatus.ForeColor = UITheme.Success;
                _lblStatus.Text = "Backup completed successfully.";
            }
            catch (Exception ex)
            {
                _lblStatus.ForeColor = UITheme.Danger;
                _lblStatus.Text = "Backup failed: " + ex.Message;
            }
        }

        private void DoRestore()
        {
            using var ofd = new OpenFileDialog { Filter = "Database Backup (*.db)|*.db" };
            if (ofd.ShowDialog() != DialogResult.OK) return;
            if (MessageBox.Show("This will overwrite the current database. Continue?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            try
            {
                string dbFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AttaChakiPOS.db");
                File.Copy(ofd.FileName, dbFile, true);
                _lblStatus.ForeColor = UITheme.Success;
                _lblStatus.Text = "Restore completed. Please restart the application.";
            }
            catch (Exception ex)
            {
                _lblStatus.ForeColor = UITheme.Danger;
                _lblStatus.Text = "Restore failed: " + ex.Message;
            }
        }
    }
}
