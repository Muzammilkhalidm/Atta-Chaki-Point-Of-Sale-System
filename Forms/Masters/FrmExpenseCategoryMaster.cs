using AttaChakiPOS.Common;

namespace AttaChakiPOS.Forms.Masters
{
    public class FrmExpenseCategoryMaster : CrudFormBase
    {
        public FrmExpenseCategoryMaster() : base(
            "Expense Categories",
            "ExpenseCategories",
            "ExpenseCategoryId",
            new List<FieldDef>
            {
                FieldDef.Text("ExpenseCategoryName", "Category Name")
            })
        { }
    }
}
