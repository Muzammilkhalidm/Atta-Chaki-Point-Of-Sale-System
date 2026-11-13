using AttaChakiPOS.Common;

namespace AttaChakiPOS.Forms.Masters
{
    public class FrmPaymentMethodMaster : CrudFormBase
    {
        public FrmPaymentMethodMaster() : base(
            "Payment Methods",
            "PaymentMethods",
            "PaymentMethodId",
            new List<FieldDef>
            {
                FieldDef.Text("MethodName", "Method Name")
            })
        { }
    }
}
