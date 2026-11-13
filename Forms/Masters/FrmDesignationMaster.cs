using AttaChakiPOS.Common;

namespace AttaChakiPOS.Forms.Masters
{
    public class FrmDesignationMaster : CrudFormBase
    {
        public FrmDesignationMaster() : base(
            "Employee Designations",
            "Designations",
            "DesignationId",
            new List<FieldDef>
            {
                FieldDef.Text("DesignationName", "Designation Name")
            })
        { }
    }
}
