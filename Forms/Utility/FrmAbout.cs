using AttaChakiPOS.Common;

namespace AttaChakiPOS.Forms.Utility
{
    public class FrmAbout : Form
    {
        public FrmAbout()
        {
            Text = "About";
            Width = 700;
            Height = 480;
            UITheme.StyleForm(this);
            BuildLayout();
        }

        private void BuildLayout()
        {
            Controls.Add(UITheme.MakeHeaderBar("About This System"));
            var card = new Panel { Dock = DockStyle.Fill, BackColor = UITheme.PanelBackground, Padding = new Padding(30) };

            var lbl = new Label
            {
                Dock = DockStyle.Fill,
                Font = UITheme.FontLabel,
                Text =
                    "Al-Barkat Atta Chaki\nPOS & Inventory Management System\nVersion 1.0.0\n\n" +
                    "Built on .NET 8 Windows Forms with a local SQLite database.\n\n" +
                    "Modules: POS Sales, Purchases, Production (Chaki grinding),\n" +
                    "Stock & Inventory, Finance / Ledgers, Master Data and\n" +
                    "Management Reports.\n\n" +
                    "All screens share one consistent design system so labels,\n" +
                    "margins and button placement are identical across every\n" +
                    "form in the application."
            };
            card.Controls.Add(lbl);
            Controls.Add(card);
            card.BringToFront();
        }
    }
}
