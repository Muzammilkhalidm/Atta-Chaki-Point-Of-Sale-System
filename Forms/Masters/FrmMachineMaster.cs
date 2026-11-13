using AttaChakiPOS.Common;

namespace AttaChakiPOS.Forms.Masters
{
    public class FrmMachineMaster : CrudFormBase
    {
        public FrmMachineMaster() : base(
            "Chaki Machines",
            "Machines",
            "MachineId",
            new List<FieldDef>
            {
                FieldDef.Text("MachineName", "Machine Name"),
                FieldDef.Text("Capacity", "Capacity (per hour)", required: false),
                FieldDef.Date("PurchaseDate", "Purchase Date", required: false),
                FieldDef.Text("Status", "Status", required: false)
            })
        { }
    }
}
