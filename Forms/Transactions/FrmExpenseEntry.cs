using AttaChakiPOS.Common;

namespace AttaChakiPOS.Forms.Transactions
{
    public class FrmExpenseEntry : CrudFormBase
    {
        public FrmExpenseEntry() : base(
            "Expense Entry",
            "ExpenseEntry",
            "ExpenseId",
            new List<FieldDef>
            {
                FieldDef.Combo("ExpenseCategoryId", "Expense Category", "SELECT ExpenseCategoryId, ExpenseCategoryName FROM ExpenseCategories WHERE IsActive=1", "ExpenseCategoryId", "ExpenseCategoryName"),
                FieldDef.Number("Amount", "Amount"),
                FieldDef.Text("Description", "Description", required: false)
            })
        { }
    }
}
