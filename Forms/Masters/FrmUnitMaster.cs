using AttaChakiPOS.Common;

namespace AttaChakiPOS.Forms.Masters
{
    public class FrmUnitMaster : CrudFormBase
    {
        public FrmUnitMaster() : base(
            "Units of Measure",
            "Units",
            "UnitId",
            new List<FieldDef>
            {
                FieldDef.Text("UnitName", "Unit Name"),
                FieldDef.Text("ShortCode", "Short Code", required: false)
            })
        { }
    }
}
