using AttaChakiPOS.Common;
using AttaChakiPOS.Database;

namespace AttaChakiPOS.Forms.Transactions
{
    public class FrmSalesReturn : CrudFormBase
    {
        public FrmSalesReturn() : base(
            "Sales Return",
            "SalesReturn",
            "ReturnId",
            new List<FieldDef>
            {
                FieldDef.Text("ReturnNo", "Return No"),
                FieldDef.Combo("SaleId", "Original Invoice", "SELECT SaleId, InvoiceNo FROM SalesInvoice ORDER BY SaleId DESC", "SaleId", "InvoiceNo"),
                FieldDef.Number("TotalAmount", "Return Amount"),
                FieldDef.Text("Reason", "Reason", required: false)
            })
        { }

        protected override void SaveNew()
        {
            var txtReturnNo = Fields.First(f => f.ColumnName == "ReturnNo").BoundControl as TextBox;
            if (txtReturnNo != null && string.IsNullOrWhiteSpace(txtReturnNo.Text))
                txtReturnNo.Text = DbHelper.NextDocumentNumber("SRET", "SalesReturn", "ReturnNo");
            base.SaveNew();
        }
    }
}
