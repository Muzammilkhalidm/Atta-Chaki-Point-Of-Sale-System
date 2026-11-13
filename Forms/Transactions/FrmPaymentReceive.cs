using AttaChakiPOS.Common;

namespace AttaChakiPOS.Forms.Transactions
{
    public class FrmPaymentReceive : CrudFormBase
    {
        public FrmPaymentReceive() : base(
            "Receive Payment From Customer",
            "PaymentReceive",
            "PaymentId",
            new List<FieldDef>
            {
                FieldDef.Combo("CustomerId", "Customer", "SELECT CustomerId, CustomerName FROM Customers WHERE IsActive=1", "CustomerId", "CustomerName"),
                FieldDef.Number("Amount", "Amount Received"),
                FieldDef.Combo("PaymentMethodId", "Payment Method", "SELECT PaymentMethodId, MethodName FROM PaymentMethods WHERE IsActive=1", "PaymentMethodId", "MethodName", required: false),
                FieldDef.Text("Notes", "Notes", required: false)
            })
        { }
    }
}
