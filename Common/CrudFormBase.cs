using System.Data;
using AttaChakiPOS.Database;

namespace AttaChakiPOS.Common
{
    /// <summary>
    /// Reusable master-data screen: header + labeled input fields (all aligned
    /// on the same label width / row height / margins) + action buttons +
    /// a results grid. Every master form (Category, Unit, Supplier, Customer,
    /// Employee, Bank, Tax, Machine, Vehicle, etc.) inherits this so the whole
    /// application feels like ONE consistent product instead of 50 different ones.
    /// </summary>
    public class CrudFormBase : Form
    {
        protected readonly string TableName;
        protected readonly string IdColumn;
        protected readonly List<FieldDef> Fields;
        protected readonly string DisplayTitle;

        private TableLayoutPanel _inputLayout = null!;
        private DataGridView _grid = null!;
        private TextBox _txtSearch = null!;
        private int _selectedId = 0;

        public CrudFormBase(string title, string tableName, string idColumn, List<FieldDef> fields)
        {
            DisplayTitle = title;
            TableName = tableName;
            IdColumn = idColumn;
            Fields = fields;

            Text = title;
            Width = 1100;
            Height = 700;
            UITheme.StyleForm(this);
            BuildLayout();
            LoadLookups();
            RefreshGrid();
        }

        private void BuildLayout()
        {
            Controls.Add(UITheme.MakeHeaderBar(DisplayTitle));

            // ---- Root split: input form on top, grid below ----
            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                Padding = new Padding(UITheme.FormPadding),
                BackColor = UITheme.PageBackground
            };
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            Controls.Add(root);
            root.BringToFront();

            // ---- Input panel (card) ----
            var card = new Panel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                BackColor = UITheme.PanelBackground,
                Padding = new Padding(20),
                Margin = new Padding(0, 0, 0, 16)
            };

            _inputLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 4,
                Padding = new Padding(0)
            };
            _inputLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, UITheme.LabelWidth));
            _inputLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            _inputLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, UITheme.LabelWidth));
            _inputLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

            int row = 0;
            int rowStylesAdded = 0;
            for (int i = 0; i < Fields.Count; i++)
            {
                var f = Fields[i];
                int col = (i % 2 == 0) ? 0 : 2;
                if (col == 0 && i > 0) row++;

                var lbl = UITheme.MakeFieldLabel(f.Label + (f.Required ? " *" : ""));
                _inputLayout.Controls.Add(lbl, col, row);

                Control input = f.Type switch
                {
                    FieldType.Combo => UITheme.MakeComboBox(),
                    FieldType.Date => UITheme.MakeDatePicker(),
                    _ => UITheme.MakeTextBox()
                };
                f.BoundControl = input;
                var wrap = new Panel { Dock = DockStyle.Fill, Height = UITheme.FieldRowHeight, Margin = new Padding(0, 4, 20, 4) };
                input.Dock = DockStyle.Fill;
                wrap.Controls.Add(input);
                _inputLayout.Controls.Add(wrap, col + 1, row);

                if (row + 1 > rowStylesAdded)
                {
                    _inputLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, UITheme.FieldRowHeight + 8));
                    rowStylesAdded++;
                }
            }
            row++;

            // ---- Buttons row ----
            var btnPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                Margin = new Padding(0, 12, 0, 0)
            };
            btnPanel.Controls.Add(UITheme.MakeButton("Save (Add New)", UITheme.Success, (s, e) => SaveNew()));
            btnPanel.Controls.Add(UITheme.MakeButton("Update Selected", UITheme.Info, (s, e) => UpdateSelected()));
            btnPanel.Controls.Add(UITheme.MakeButton("Delete Selected", UITheme.Danger, (s, e) => DeleteSelected()));
            btnPanel.Controls.Add(UITheme.MakeButton("Clear Form", UITheme.Warning, (s, e) => ClearForm()));

            card.Controls.Add(btnPanel);
            card.Controls.Add(_inputLayout);
            root.Controls.Add(card, 0, 0);

            // ---- Grid card ----
            var gridCard = new Panel { Dock = DockStyle.Fill, BackColor = UITheme.PanelBackground, Padding = new Padding(16) };
            var searchBar = new Panel { Dock = DockStyle.Top, Height = 46 };
            var lblSearch = new Label { Text = "Search:", Font = UITheme.FontLabelBold, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Left, Width = 70 };
            _txtSearch = new TextBox { Dock = DockStyle.Left, Width = 260, Font = UITheme.FontInput, Height = 30, Margin = new Padding(8, 6, 0, 0) };
            _txtSearch.TextChanged += (s, e) => RefreshGrid();
            searchBar.Controls.Add(_txtSearch);
            searchBar.Controls.Add(lblSearch);

            _grid = new DataGridView { Dock = DockStyle.Fill };
            UITheme.StyleGrid(_grid);
            _grid.CellClick += (s, e) => { if (e.RowIndex >= 0) LoadRowIntoForm(e.RowIndex); };

            gridCard.Controls.Add(_grid);
            gridCard.Controls.Add(searchBar);
            root.Controls.Add(gridCard, 0, 1);
        }

        protected virtual void LoadLookups()
        {
            foreach (var f in Fields.Where(f => f.Type == FieldType.Combo && f.LookupSql != null))
            {
                var dt = DbHelper.GetDataTable(f.LookupSql!);
                var combo = (ComboBox)f.BoundControl;
                combo.DisplayMember = f.LookupTextColumn;
                combo.ValueMember = f.LookupIdColumn;
                combo.DataSource = dt;
            }
        }

        protected virtual void RefreshGrid()
        {
            string cols = string.Join(", ", new[] { IdColumn }.Concat(Fields.Select(f => f.ColumnName)));
            string sql = $"SELECT {cols} FROM {TableName}";
            if (!string.IsNullOrWhiteSpace(_txtSearch?.Text))
            {
                var likeCols = Fields.Where(f => f.Type == FieldType.Text).Select(f => $"{f.ColumnName} LIKE @s");
                if (likeCols.Any())
                    sql += " WHERE " + string.Join(" OR ", likeCols);
            }
            sql += $" ORDER BY {IdColumn} DESC";

            var pars = new Dictionary<string, object>();
            if (!string.IsNullOrWhiteSpace(_txtSearch?.Text))
                pars["@s"] = $"%{_txtSearch!.Text}%";

            var table = DbHelper.GetDataTable(sql, pars);
            _grid.DataSource = table;
            if (_grid.Columns.Contains(IdColumn)) _grid.Columns[IdColumn]!.Visible = false;
        }

        private void LoadRowIntoForm(int rowIndex)
        {
            var row = _grid.Rows[rowIndex];
            _selectedId = Convert.ToInt32(row.Cells[IdColumn].Value);
            foreach (var f in Fields)
            {
                if (!_grid.Columns.Contains(f.ColumnName)) continue;
                var val = row.Cells[f.ColumnName].Value;
                SetControlValue(f, val);
            }
        }

        private void SetControlValue(FieldDef f, object? val)
        {
            switch (f.Type)
            {
                case FieldType.Combo:
                    ((ComboBox)f.BoundControl).SelectedValue = val ?? DBNull.Value;
                    break;
                case FieldType.Date:
                    if (val != null && DateTime.TryParse(val.ToString(), out var dt))
                        ((DateTimePicker)f.BoundControl).Value = dt;
                    break;
                default:
                    ((TextBox)f.BoundControl).Text = val?.ToString() ?? string.Empty;
                    break;
            }
        }

        private object GetControlValue(FieldDef f)
        {
            return f.Type switch
            {
                FieldType.Combo => ((ComboBox)f.BoundControl).SelectedValue ?? DBNull.Value,
                FieldType.Date => ((DateTimePicker)f.BoundControl).Value.ToString("yyyy-MM-dd"),
                FieldType.Number => string.IsNullOrWhiteSpace(((TextBox)f.BoundControl).Text) ? 0 : Convert.ToDouble(((TextBox)f.BoundControl).Text),
                _ => ((TextBox)f.BoundControl).Text
            };
        }

        private bool ValidateForm()
        {
            foreach (var f in Fields.Where(f => f.Required))
            {
                var v = GetControlValue(f);
                if (v == null || v == DBNull.Value || string.IsNullOrWhiteSpace(v.ToString()))
                {
                    MessageBox.Show($"'{f.Label}' is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    f.BoundControl.Focus();
                    return false;
                }
            }
            return true;
        }

        protected virtual void SaveNew()
        {
            if (!ValidateForm()) return;
            var cols = string.Join(", ", Fields.Select(f => f.ColumnName));
            var pnames = string.Join(", ", Fields.Select(f => "@" + f.ColumnName));
            var sql = $"INSERT INTO {TableName} ({cols}) VALUES ({pnames})";
            var pars = Fields.ToDictionary(f => "@" + f.ColumnName, f => GetControlValue(f));
            DbHelper.ExecuteNonQuery(sql, pars);
            MessageBox.Show("Record saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ClearForm();
            RefreshGrid();
        }

        protected virtual void UpdateSelected()
        {
            if (_selectedId == 0) { MessageBox.Show("Select a row from the grid first.", "Info"); return; }
            if (!ValidateForm()) return;
            var sets = string.Join(", ", Fields.Select(f => $"{f.ColumnName} = @{f.ColumnName}"));
            var sql = $"UPDATE {TableName} SET {sets} WHERE {IdColumn} = @id";
            var pars = Fields.ToDictionary(f => "@" + f.ColumnName, f => GetControlValue(f));
            pars["@id"] = _selectedId;
            DbHelper.ExecuteNonQuery(sql, pars);
            MessageBox.Show("Record updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ClearForm();
            RefreshGrid();
        }

        protected virtual void DeleteSelected()
        {
            if (_selectedId == 0) { MessageBox.Show("Select a row from the grid first.", "Info"); return; }
            if (MessageBox.Show("Delete this record?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            DbHelper.ExecuteNonQuery($"DELETE FROM {TableName} WHERE {IdColumn} = @id", new() { { "@id", _selectedId } });
            ClearForm();
            RefreshGrid();
        }

        protected virtual void ClearForm()
        {
            _selectedId = 0;
            foreach (var f in Fields)
            {
                if (f.Type == FieldType.Combo) ((ComboBox)f.BoundControl).SelectedIndex = -1;
                else if (f.Type == FieldType.Date) ((DateTimePicker)f.BoundControl).Value = DateTime.Now;
                else ((TextBox)f.BoundControl).Clear();
            }
        }
    }
}
