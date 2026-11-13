using AttaChakiPOS.Common;

namespace AttaChakiPOS.Forms.Masters
{
    public class FrmDiscountMaster : CrudFormBase
    {
        public FrmDiscountMaster() : base(
            "Discount Rules",
            "DiscountRules",
            "DiscountId",
            new List<FieldDef>
            {
                FieldDef.Text("DiscountName", "Discount Name"),
                FieldDef.Number("Percentage", "Percentage (%)")
            })
        { }
    }
}
