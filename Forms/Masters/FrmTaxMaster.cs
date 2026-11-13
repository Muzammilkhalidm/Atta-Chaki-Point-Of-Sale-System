using AttaChakiPOS.Common;

namespace AttaChakiPOS.Forms.Masters
{
    public class FrmTaxMaster : CrudFormBase
    {
        public FrmTaxMaster() : base(
            "Tax Rates",
            "TaxRates",
            "TaxId",
            new List<FieldDef>
            {
                FieldDef.Text("TaxName", "Tax Name"),
                FieldDef.Number("Percentage", "Percentage (%)")
            })
        { }
    }
}
