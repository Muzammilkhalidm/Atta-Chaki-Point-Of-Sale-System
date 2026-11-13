using AttaChakiPOS.Common;

namespace AttaChakiPOS.Forms.Masters
{
    public class FrmEmployeeMaster : CrudFormBase
    {
        public FrmEmployeeMaster() : base(
            "Employees",
            "Employees",
            "EmployeeId",
            new List<FieldDef>
            {
                FieldDef.Text("EmployeeName", "Employee Name"),
                FieldDef.Combo("DesignationId", "Designation", "SELECT DesignationId, DesignationName FROM Designations WHERE IsActive=1", "DesignationId", "DesignationName"),
                FieldDef.Text("Phone", "Phone Number", required: false),
                FieldDef.Number("Salary", "Monthly Salary", required: false),
                FieldDef.Date("JoiningDate", "Joining Date", required: false)
            })
        { }
    }
}
