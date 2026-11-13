using AttaChakiPOS.Common;
using AttaChakiPOS.Database;

namespace AttaChakiPOS.Forms.Reports
{
    public class FrmProductionReport : ReportFormBase
    {
        public FrmProductionReport() : base(
            "Production Report (Chaki Grinding)",
            (from, to) => DbHelper.GetDataTable(
                @"SELECT p.ProductionNo AS Batch, m.MachineName AS Machine, r.ProductName AS 'Raw Material',
                         p.RawQuantity AS 'Raw Qty', o.ProductName AS Output, p.OutputQuantity AS 'Output Qty',
                         p.WastageQuantity AS Wastage, p.ProductionDate AS Date
                  FROM ProductionEntry p
                  LEFT JOIN Machines m ON p.MachineId = m.MachineId
                  LEFT JOIN Products r ON p.RawProductId = r.ProductId
                  LEFT JOIN Products o ON p.OutputProductId = o.ProductId
                  WHERE p.ProductionDate BETWEEN @f AND @t
                  ORDER BY p.ProductionDate DESC",
                new() { { "@f", from.ToString("yyyy-MM-dd HH:mm:ss") }, { "@t", to.ToString("yyyy-MM-dd HH:mm:ss") } }),
            "Output Qty")
        { }
    }
}
