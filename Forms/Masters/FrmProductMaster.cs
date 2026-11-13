using AttaChakiPOS.Common;
using AttaChakiPOS.Database;

namespace AttaChakiPOS.Forms.Masters
{
    public class FrmProductMaster : CrudFormBase
    {
        public FrmProductMaster() : base(
            "Products / Items",
            "Products",
            "ProductId",
            new List<FieldDef>
            {
                FieldDef.Text("ProductCode", "Product Code"),
                FieldDef.Text("ProductName", "Product Name"),
                FieldDef.Combo("CategoryId", "Category", "SELECT CategoryId, CategoryName FROM Categories WHERE IsActive=1", "CategoryId", "CategoryName"),
                FieldDef.Combo("UnitId", "Unit", "SELECT UnitId, UnitName FROM Units WHERE IsActive=1", "UnitId", "UnitName"),
                FieldDef.Combo("WarehouseId", "Warehouse", "SELECT WarehouseId, WarehouseName FROM Warehouses WHERE IsActive=1", "WarehouseId", "WarehouseName", required: false),
                FieldDef.Number("PurchasePrice", "Purchase Price"),
                FieldDef.Number("SalePrice", "Sale Price"),
                FieldDef.Number("OpeningStock", "Opening Stock", required: false),
                FieldDef.Number("ReorderLevel", "Reorder Level", required: false)
            })
        { }

        protected override void SaveNew()
        {
            base.SaveNew();
            // Keep CurrentStock aligned with OpeningStock for newly created products
            DbHelper.ExecuteNonQuery("UPDATE Products SET CurrentStock = OpeningStock WHERE CurrentStock = 0 AND OpeningStock > 0");
        }
    }
}
