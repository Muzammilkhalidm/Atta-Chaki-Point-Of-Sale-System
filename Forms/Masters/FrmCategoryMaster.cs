using AttaChakiPOS.Common;

namespace AttaChakiPOS.Forms.Masters
{
    public class FrmCategoryMaster : CrudFormBase
    {
        public FrmCategoryMaster() : base(
            "Product Categories",
            "Categories",
            "CategoryId",
            new List<FieldDef>
            {
                FieldDef.Text("CategoryName", "Category Name"),
                FieldDef.Text("Description", "Description", required: false)
            })
        { }
    }
}
