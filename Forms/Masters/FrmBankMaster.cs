using AttaChakiPOS.Common;

namespace AttaChakiPOS.Forms.Masters
{
    public class FrmBankMaster : CrudFormBase
    {
        public FrmBankMaster() : base(
            "Bank Accounts",
            "Banks",
            "BankId",
            new List<FieldDef>
            {
                FieldDef.Text("BankName", "Bank Name"),
                FieldDef.Text("AccountTitle", "Account Title", required: false),
                FieldDef.Text("AccountNumber", "Account Number", required: false)
            })
        { }
    }
}
