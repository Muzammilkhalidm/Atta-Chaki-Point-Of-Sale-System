using AttaChakiPOS.Common;

namespace AttaChakiPOS.Forms.Masters
{
    public class FrmVehicleMaster : CrudFormBase
    {
        public FrmVehicleMaster() : base(
            "Delivery Vehicles",
            "Vehicles",
            "VehicleId",
            new List<FieldDef>
            {
                FieldDef.Text("VehicleNumber", "Vehicle Number"),
                FieldDef.Text("VehicleType", "Vehicle Type", required: false),
                FieldDef.Text("DriverName", "Driver Name", required: false)
            })
        { }
    }
}
