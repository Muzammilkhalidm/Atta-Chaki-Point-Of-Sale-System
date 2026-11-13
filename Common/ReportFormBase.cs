using System.Data;
using AttaChakiPOS.Database;

namespace AttaChakiPOS.Common
{
    /// <summary>
    /// Reusable report screen: header + From/To date filter + Run button +
    /// results grid + totals footer. All 15+ report forms share this exact
    /// layout so every report in the system looks and behaves the same way.
    /// </summary>
    public class ReportFormBase : Form
    {
        protected DateTimePicker DpFrom = null!;
        protected DateTimePicker DpTo = null!;
        protected DataGridView Grid = null!;
        protected Label LblTotal = null!;
        private readonly Func<DateTime, DateTime, DataTable> _dataLoader;
        private readonly string? _totalColumn;

        public ReportFormBase(string title, Func<DateTime, DateTime, DataTable> dataLoader, string? totalColumn = null)
        {
            _dataLoader = dataLoader;
            _totalColumn = totalColumn;
            Text = title;
            Width = 1150;
            Height = 700;
            UITheme.StyleForm(this);
            BuildLayout(title);
            RunReport();
        }

        private void BuildLayout(string title)
        {
            Controls.Add(UITheme.MakeHeaderBar(title));

            var filterBar = new Panel { Dock = DockStyle.Top, Height = 76, BackColor = UITheme.PanelBackground, Padding = new Padding(UITheme.FormPadding, 16, UITheme.FormPadding, 16) };

            var flow = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.LeftToRight };

            flow.Controls.Add(new Label { Text = "From:", Font = UITheme.FontLabelBold, TextAlign = ContentAlignment.MiddleLeft, Width = 50, Height = UITheme.FieldRowHeight });
            DpFrom = new DateTimePicker { Width = 150, Height = UITheme.FieldRowHeight, Font = UITheme.FontInput, Value = DateTime.Now.AddDays(-30), Margin = new Padding(0, 0, 20, 0) };
            flow.Controls.Add(DpFrom);

            flow.Controls.Add(new Label { Text = "To:", Font = UITheme.FontLabelBold, TextAlign = ContentAlignment.MiddleLeft, Width = 35, Height = UITheme.FieldRowHeight });
            DpTo = new DateTimePicker { Width = 150, Height = UITheme.FieldRowHeight, Font = UITheme.FontInput, Value = DateTime.Now, Margin = new Padding(0, 0, 20, 0) };
            flow.Controls.Add(DpTo);

            flow.Controls.Add(UITheme.MakeButton("Run Report", UITheme.Primary, (s, e) => RunReport()));
            flow.Controls.Add(UITheme.MakeButton("Export CSV", UITheme.AccentDark, (s, e) => ExportCsv()));

            filterBar.Controls.Add(flow);
            Controls.Add(filterBar);
            filterBar.BringToFront();

            var gridCard = new Panel { Dock = DockStyle.Fill, BackColor = UITheme.PanelBackground, Padding = new Padding(UITheme.FormPadding) };
            Grid = new DataGridView { Dock = DockStyle.Fill };
            UITheme.StyleGrid(Grid);

            var footer = new Panel { Dock = DockStyle.Bottom, Height = 44 };
            LblTotal = new Label { Dock = DockStyle.Right, Width = 320, Font = UITheme.FontSubHeader, TextAlign = ContentAlignment.MiddleRight, ForeColor = UITheme.PrimaryDark };
            footer.Controls.Add(LblTotal);

            gridCard.Controls.Add(Grid);
            gridCard.Controls.Add(footer);
            Controls.Add(gridCard);
            gridCard.BringToFront();
        }

        protected virtual void RunReport()
        {
            var dt = _dataLoader(DpFrom.Value.Date, DpTo.Value.Date.AddDays(1).AddSeconds(-1));
            Grid.DataSource = dt;

            if (_totalColumn != null && dt.Columns.Contains(_totalColumn))
            {
                double total = 0;
                foreach (DataRow r in dt.Rows)
                    total += r[_totalColumn] == DBNull.Value ? 0 : Convert.ToDouble(r[_totalColumn]);
                LblTotal.Text = $"Total: Rs. {total:N2}   |   Rows: {dt.Rows.Count}";
            }
            else
            {
                LblTotal.Text = $"Rows: {dt.Rows.Count}";
            }
        }

        private void ExportCsv()
        {
            if (Grid.DataSource is not DataTable dt || dt.Rows.Count == 0)
            {
                MessageBox.Show("No data to export.", "Info");
                return;
            }
            using var sfd = new SaveFileDialog { Filter = "CSV file (*.csv)|*.csv", FileName = $"{Text}_{DateTime.Now:yyyyMMdd}.csv" };
            if (sfd.ShowDialog() != DialogResult.OK) return;

            using var sw = new StreamWriter(sfd.FileName);
            sw.WriteLine(string.Join(",", dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName)));
            foreach (DataRow row in dt.Rows)
                sw.WriteLine(string.Join(",", row.ItemArray.Select(v => $"\"{v}\"")));

            MessageBox.Show("Report exported successfully.", "Success");
        }
    }
}
