namespace AttaChakiPOS.Common
{
    /// <summary>
    /// Single source of truth for every color, font and spacing value used
    /// anywhere in the application. Because ALL 50+ forms pull from this
    /// class instead of hard-coding their own values, every label, textbox,
    /// button and grid lines up identically and no screen ever looks
    /// "half-and-half" compared to another.
    /// </summary>
    public static class UITheme
    {
        // ---- Brand colors (wheat / mill theme) ----
        public static readonly Color PrimaryDark = Color.FromArgb(66, 42, 25);      // deep roasted brown
        public static readonly Color Primary = Color.FromArgb(121, 79, 43);         // wheat brown
        public static readonly Color Accent = Color.FromArgb(224, 155, 45);         // golden wheat
        public static readonly Color AccentDark = Color.FromArgb(184, 122, 25);
        public static readonly Color Success = Color.FromArgb(46, 139, 87);
        public static readonly Color Danger = Color.FromArgb(178, 34, 34);
        public static readonly Color Warning = Color.FromArgb(214, 137, 16);
        public static readonly Color Info = Color.FromArgb(43, 100, 140);

        public static readonly Color PageBackground = Color.FromArgb(244, 240, 232);
        public static readonly Color PanelBackground = Color.White;
        public static readonly Color HeaderBackground = Color.FromArgb(121, 79, 43);
        public static readonly Color GridHeaderBack = Color.FromArgb(121, 79, 43);
        public static readonly Color GridAltRow = Color.FromArgb(250, 246, 238);
        public static readonly Color TextDark = Color.FromArgb(35, 28, 20);
        public static readonly Color TextMuted = Color.FromArgb(110, 100, 90);
        public static readonly Color BorderColor = Color.FromArgb(210, 198, 180);

        // ---- Fonts (consistent everywhere = "prominent, not half-half") ----
        public static readonly Font FontHeader = new Font("Segoe UI", 16F, FontStyle.Bold);
        public static readonly Font FontSubHeader = new Font("Segoe UI", 12F, FontStyle.Bold);
        public static readonly Font FontLabel = new Font("Segoe UI", 10.5F, FontStyle.Regular);
        public static readonly Font FontLabelBold = new Font("Segoe UI", 10.5F, FontStyle.Bold);
        public static readonly Font FontInput = new Font("Segoe UI", 10.5F, FontStyle.Regular);
        public static readonly Font FontButton = new Font("Segoe UI", 10.5F, FontStyle.Bold);
        public static readonly Font FontGrid = new Font("Segoe UI", 10F, FontStyle.Regular);
        public static readonly Font FontGridHeader = new Font("Segoe UI", 10F, FontStyle.Bold);
        public static readonly Font FontKpiValue = new Font("Segoe UI", 22F, FontStyle.Bold);
        public static readonly Font FontKpiLabel = new Font("Segoe UI", 10F, FontStyle.Regular);

        // ---- Fixed spacing so every form shares identical rhythm ----
        public const int FormPadding = 20;      // outer margin used on every form
        public const int FieldRowHeight = 36;    // height of every label/input row
        public const int LabelWidth = 160;       // every field label is this wide -> inputs all line up
        public const int ControlSpacing = 12;    // gap between controls
        public const int ButtonHeight = 38;
        public const int ButtonWidth = 130;

        public static void StyleForm(Form f)
        {
            f.BackColor = PageBackground;
            f.Font = FontLabel;
            f.StartPosition = FormStartPosition.CenterScreen;
        }

        public static Button MakeButton(string text, Color backColor, EventHandler onClick)
        {
            var btn = new Button
            {
                Text = text,
                Width = ButtonWidth,
                Height = ButtonHeight,
                BackColor = backColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = FontButton,
                Cursor = Cursors.Hand,
                Margin = new Padding(6, 6, 6, 6),
                TextAlign = ContentAlignment.MiddleCenter
            };
            btn.FlatAppearance.BorderSize = 0;
            if (onClick != null) btn.Click += onClick;
            return btn;
        }

        public static Label MakeFieldLabel(string text)
        {
            return new Label
            {
                Text = text,
                Width = LabelWidth,
                Height = FieldRowHeight,
                TextAlign = ContentAlignment.MiddleRight,
                Font = FontLabelBold,
                ForeColor = TextDark,
                Margin = new Padding(0, 0, 10, 0),
                Padding = new Padding(0, 0, 8, 0)
            };
        }

        public static TextBox MakeTextBox()
        {
            return new TextBox
            {
                Height = FieldRowHeight,
                Font = FontInput,
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 4, 0, 4),
                BorderStyle = BorderStyle.FixedSingle
            };
        }

        public static ComboBox MakeComboBox()
        {
            return new ComboBox
            {
                Height = FieldRowHeight,
                Font = FontInput,
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 4, 0, 4),
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat
            };
        }

        public static DateTimePicker MakeDatePicker()
        {
            return new DateTimePicker
            {
                Height = FieldRowHeight,
                Font = FontInput,
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 4, 0, 4),
                Format = DateTimePickerFormat.Short
            };
        }

        public static void StyleGrid(DataGridView grid)
        {
            grid.BorderStyle = BorderStyle.None;
            grid.BackgroundColor = PanelBackground;
            grid.RowHeadersVisible = false;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.ReadOnly = true;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.MultiSelect = false;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.Font = FontGrid;
            grid.RowTemplate.Height = 32;
            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersDefaultCellStyle.BackColor = GridHeaderBack;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.Font = FontGridHeader;
            grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            grid.ColumnHeadersDefaultCellStyle.Padding = new Padding(8, 0, 0, 0);
            grid.ColumnHeadersHeight = 38;
            grid.DefaultCellStyle.Padding = new Padding(8, 0, 0, 0);
            grid.DefaultCellStyle.SelectionBackColor = Accent;
            grid.DefaultCellStyle.SelectionForeColor = Color.White;
            grid.AlternatingRowsDefaultCellStyle.BackColor = GridAltRow;
            grid.GridColor = BorderColor;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        }

        public static Panel MakeHeaderBar(string title, string subtitle = "")
        {
            var panel = new Panel { Dock = DockStyle.Top, Height = 64, BackColor = HeaderBackground };
            var lblTitle = new Label
            {
                Text = title,
                Font = FontSubHeader,
                ForeColor = Color.White,
                AutoSize = false,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(FormPadding, 0, 0, 0)
            };
            panel.Controls.Add(lblTitle);
            return panel;
        }
    }
}
