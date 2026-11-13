using AttaChakiPOS.Common;
using AttaChakiPOS.Database;

namespace AttaChakiPOS.Forms.Transactions
{
    public class FrmOpeningStock : CrudFormBase
    {
        public FrmOpeningStock() : base(
            "Opening Stock Entry",
            "OpeningStockEntry",
            "EntryId",
            new List<FieldDef>
            {
                FieldDef.Combo("ProductId", "Product", "SELECT ProductId, ProductName FROM Products WHERE IsActive=1", "ProductId", "ProductName"),
                FieldDef.Number("Quantity", "Opening Quantity")
            })
        { }

        protected override void SaveNew()
        {
            base.SaveNew();
            DbHelper.ExecuteNonQuery(
                @"UPDATE Products SET CurrentStock = CurrentStock + (
                    SELECT Quantity FROM OpeningStockEntry WHERE EntryId = (SELECT MAX(EntryId) FROM OpeningStockEntry)
                  ) WHERE ProductId = (
                    SELECT ProductId FROM OpeningStockEntry WHERE EntryId = (SELECT MAX(EntryId) FROM OpeningStockEntry)
                  )");
        }
    }
}
