using AttaChakiPOS.Common;

namespace AttaChakiPOS.Forms.Masters
{
    public class FrmSupplierMaster : CrudFormBase
    {
        public FrmSupplierMaster() : base(
            "Suppliers",
            "Suppliers",
            "SupplierId",
            new List<FieldDef>
            {
                FieldDef.Text("SupplierName", "Supplier Name"),
                FieldDef.Text("ContactPerson", "Contact Person", required: false),
                FieldDef.Text("Phone", "Phone Number", required: false),
                FieldDef.Text("Address", "Address", required: false),
                FieldDef.Number("OpeningBalance", "Opening Balance", required: false)
            })
        { }
    }
}
