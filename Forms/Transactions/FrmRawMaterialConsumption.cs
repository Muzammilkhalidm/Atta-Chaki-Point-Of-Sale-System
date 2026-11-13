using AttaChakiPOS.Common;

namespace AttaChakiPOS.Forms.Transactions
{
    public class FrmRawMaterialConsumption : CrudFormBase
    {
        public FrmRawMaterialConsumption() : base(
            "Raw Material Consumption",
            "WastageEntry",
            "WastageId",
            new List<FieldDef>
            {
                FieldDef.Combo("ProductId", "Raw Material", "SELECT ProductId, ProductName FROM Products WHERE IsActive=1", "ProductId", "ProductName"),
                FieldDef.Number("Quantity", "Quantity Consumed"),
                FieldDef.Text("Reason", "Consumed For", required: false)
            })
        { }
    }
}
