using AttaChakiPOS.Common;

namespace AttaChakiPOS.Forms.Masters
{
    public class FrmPriceListMaster : CrudFormBase
    {
        public FrmPriceListMaster() : base(
            "Price Lists",
            "PriceList",
            "PriceListId",
            new List<FieldDef>
            {
                FieldDef.Combo("ProductId", "Product", "SELECT ProductId, ProductName FROM Products WHERE IsActive=1", "ProductId", "ProductName"),
                FieldDef.Text("PriceListName", "Price List Name"),
                FieldDef.Number("Price", "Price")
            })
        { }
    }
}
