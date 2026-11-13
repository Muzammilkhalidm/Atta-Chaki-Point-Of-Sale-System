using AttaChakiPOS.Common;

namespace AttaChakiPOS.Forms.Masters
{
    public class FrmCustomerMaster : CrudFormBase
    {
        public FrmCustomerMaster() : base(
            "Customers",
            "Customers",
            "CustomerId",
            new List<FieldDef>
            {
                FieldDef.Text("CustomerName", "Customer Name"),
                FieldDef.Text("Phone", "Phone Number", required: false),
                FieldDef.Text("Address", "Address", required: false),
                FieldDef.Number("OpeningBalance", "Opening Balance", required: false)
            })
        { }
    }
}
