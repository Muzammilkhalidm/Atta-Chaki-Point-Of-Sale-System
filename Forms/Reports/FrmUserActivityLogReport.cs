using AttaChakiPOS.Common;
using AttaChakiPOS.Database;

namespace AttaChakiPOS.Forms.Reports
{
    public class FrmUserActivityLogReport : ReportFormBase
    {
        public FrmUserActivityLogReport() : base(
            "User Activity Log",
            (from, to) => DbHelper.GetDataTable(
                @"SELECT u.FullName AS User, l.Activity, l.ActivityOn AS Date
                  FROM UserActivityLog l LEFT JOIN Users u ON l.UserId = u.UserId
                  WHERE l.ActivityOn BETWEEN @f AND @t
                  ORDER BY l.ActivityOn DESC",
                new() { { "@f", from.ToString("yyyy-MM-dd HH:mm:ss") }, { "@t", to.ToString("yyyy-MM-dd HH:mm:ss") } }))
        { }
    }
}
