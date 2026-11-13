using AttaChakiPOS.Common;
using AttaChakiPOS.Database;

namespace AttaChakiPOS.Forms.Reports
{
    public class FrmExpenseReport : ReportFormBase
    {
        public FrmExpenseReport() : base(
            "Expense Report",
            (from, to) => DbHelper.GetDataTable(
                @"SELECT e.ExpenseDate AS Date, ec.ExpenseCategoryName AS Category, e.Description, e.Amount
                  FROM ExpenseEntry e LEFT JOIN ExpenseCategories ec ON e.ExpenseCategoryId = ec.ExpenseCategoryId
                  WHERE e.ExpenseDate BETWEEN @f AND @t ORDER BY e.ExpenseDate DESC",
                new() { { "@f", from.ToString("yyyy-MM-dd HH:mm:ss") }, { "@t", to.ToString("yyyy-MM-dd HH:mm:ss") } }),
            "Amount")
        { }
    }
}
