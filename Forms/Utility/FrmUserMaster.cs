using AttaChakiPOS.Common;

namespace AttaChakiPOS.Forms.Utility
{
    public class FrmUserMaster : CrudFormBase
    {
        public FrmUserMaster() : base(
            "System Users",
            "Users",
            "UserId",
            new List<FieldDef>
            {
                FieldDef.Text("FullName", "Full Name"),
                FieldDef.Text("UserName", "Login Username"),
                FieldDef.Text("PasswordHash", "Password"),
                FieldDef.Combo("RoleId", "Role", "SELECT RoleId, RoleName FROM Roles WHERE IsActive=1", "RoleId", "RoleName"),
                FieldDef.Text("Phone", "Phone Number", required: false),
                FieldDef.Text("Email", "Email", required: false)
            })
        { }
    }
}
