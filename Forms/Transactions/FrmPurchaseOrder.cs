using AttaChakiPOS.Common;
using AttaChakiPOS.Database;

namespace AttaChakiPOS.Forms.Transactions
{
    public class FrmPurchaseOrder : CrudFormBase
    {
        public FrmPurchaseOrder() : base(
            "Purchase Order",
            "PurchaseOrder",
            "PoId",
            new List<FieldDef>
            {
                FieldDef.Text("PoNo", "PO Number"),
                FieldDef.Combo("SupplierId", "Supplier", "SELECT SupplierId, SupplierName FROM Suppliers WHERE IsActive=1", "SupplierId", "SupplierName"),
                FieldDef.Number("TotalAmount", "Estimated Amount"),
                FieldDef.Text("Status", "Status", required: false)
            })
        { }

        protected override void SaveNew()
        {
            var txtPoNo = Fields.First(f => f.ColumnName == "PoNo").BoundControl as TextBox;
            if (txtPoNo != null && string.IsNullOrWhiteSpace(txtPoNo.Text))
                txtPoNo.Text = DbHelper.NextDocumentNumber("PO", "PurchaseOrder", "PoNo");
            base.SaveNew();
        }
    }
}
