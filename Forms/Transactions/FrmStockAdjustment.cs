using AttaChakiPOS.Common;

namespace AttaChakiPOS.Forms.Transactions
{
    public class FrmStockAdjustment : CrudFormBase
    {
        public FrmStockAdjustment() : base(
            "Stock Adjustment",
            "StockAdjustment",
            "AdjustmentId",
            new List<FieldDef>
            {
                FieldDef.Combo("ProductId", "Product", "SELECT ProductId, ProductName FROM Products WHERE IsActive=1", "ProductId", "ProductName"),
                FieldDef.Number("QuantityBefore", "Quantity Before"),
                FieldDef.Number("QuantityAfter", "Quantity After"),
                FieldDef.Text("Reason", "Reason", required: false)
            })
        { }
    }
}
