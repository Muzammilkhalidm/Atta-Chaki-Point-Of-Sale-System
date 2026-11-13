using AttaChakiPOS.Common;

namespace AttaChakiPOS.Forms.Utility
{
    public class FrmRoleMaster : CrudFormBase
    {
        public FrmRoleMaster() : base(
            "User Roles",
            "Roles",
            "RoleId",
            new List<FieldDef>
            {
                FieldDef.Text("RoleName", "Role Name"),
                FieldDef.Text("Description", "Description", required: false)
            })
        { }
    }
}
