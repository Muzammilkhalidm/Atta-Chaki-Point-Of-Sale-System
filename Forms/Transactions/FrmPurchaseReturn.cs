using AttaChakiPOS.Common;
using AttaChakiPOS.Database;

namespace AttaChakiPOS.Forms.Transactions
{
    public class FrmPurchaseReturn : CrudFormBase
    {
        public FrmPurchaseReturn() : base(
            "Purchase Return",
            "PurchaseReturn",
            "ReturnId",
            new List<FieldDef>
            {
                FieldDef.Text("ReturnNo", "Return No"),
                FieldDef.Combo("PurchaseId", "Original Purchase Invoice", "SELECT PurchaseId, InvoiceNo FROM PurchaseInvoice ORDER BY PurchaseId DESC", "PurchaseId", "InvoiceNo"),
                FieldDef.Number("TotalAmount", "Return Amount"),
                FieldDef.Text("Reason", "Reason", required: false)
            })
        { }

        protected override void SaveNew()
        {
            var txtReturnNo = Fields.First(f => f.ColumnName == "ReturnNo").BoundControl as TextBox;
            if (txtReturnNo != null && string.IsNullOrWhiteSpace(txtReturnNo.Text))
                txtReturnNo.Text = DbHelper.NextDocumentNumber("PRET", "PurchaseReturn", "ReturnNo");
            base.SaveNew();
        }
    }
}
