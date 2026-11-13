using AttaChakiPOS.Common;
using AttaChakiPOS.Database;

namespace AttaChakiPOS.Forms.Transactions
{
    public class FrmWastageEntry : CrudFormBase
    {
        public FrmWastageEntry() : base(
            "Wastage Entry",
            "WastageEntry",
            "WastageId",
            new List<FieldDef>
            {
                FieldDef.Combo("ProductId", "Product", "SELECT ProductId, ProductName FROM Products WHERE IsActive=1", "ProductId", "ProductName"),
                FieldDef.Number("Quantity", "Wastage Quantity"),
                FieldDef.Text("Reason", "Reason", required: false)
            })
        { }

        protected override void SaveNew()
        {
            base.SaveNew();
            DbHelper.ExecuteNonQuery(
                @"UPDATE Products SET CurrentStock = CurrentStock - (
                    SELECT Quantity FROM WastageEntry WHERE WastageId = (SELECT MAX(WastageId) FROM WastageEntry)
                  ) WHERE ProductId = (
                    SELECT ProductId FROM WastageEntry WHERE WastageId = (SELECT MAX(WastageId) FROM WastageEntry)
                  )");
        }
    }
}
