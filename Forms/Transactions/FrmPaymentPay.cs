using AttaChakiPOS.Common;

namespace AttaChakiPOS.Forms.Transactions
{
    public class FrmPaymentPay : CrudFormBase
    {
        public FrmPaymentPay() : base(
            "Pay Supplier",
            "PaymentPay",
            "PaymentId",
            new List<FieldDef>
            {
                FieldDef.Combo("SupplierId", "Supplier", "SELECT SupplierId, SupplierName FROM Suppliers WHERE IsActive=1", "SupplierId", "SupplierName"),
                FieldDef.Number("Amount", "Amount Paid"),
                FieldDef.Combo("PaymentMethodId", "Payment Method", "SELECT PaymentMethodId, MethodName FROM PaymentMethods WHERE IsActive=1", "PaymentMethodId", "MethodName", required: false),
                FieldDef.Text("Notes", "Notes", required: false)
            })
        { }
    }
}
