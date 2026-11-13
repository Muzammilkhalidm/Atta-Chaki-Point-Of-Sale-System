using AttaChakiPOS.Common;

namespace AttaChakiPOS.Forms.Masters
{
    public class FrmWarehouseMaster : CrudFormBase
    {
        public FrmWarehouseMaster() : base(
            "Warehouses / Godown",
            "Warehouses",
            "WarehouseId",
            new List<FieldDef>
            {
                FieldDef.Text("WarehouseName", "Warehouse Name"),
                FieldDef.Text("Location", "Location", required: false)
            })
        { }
    }
}
