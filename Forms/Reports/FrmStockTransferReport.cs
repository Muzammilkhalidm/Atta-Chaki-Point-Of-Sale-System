using AttaChakiPOS.Common;
using AttaChakiPOS.Database;

namespace AttaChakiPOS.Forms.Reports
{
    public class FrmStockTransferReport : ReportFormBase
    {
        public FrmStockTransferReport() : base(
            "Stock Transfer Report",
            (from, to) => DbHelper.GetDataTable(
                @"SELECT t.TransferDate AS Date, p.ProductName AS Product, f.WarehouseName AS 'From', tw.WarehouseName AS 'To', t.Quantity
                  FROM StockTransfer t
                  LEFT JOIN Products p ON t.ProductId = p.ProductId
                  LEFT JOIN Warehouses f ON t.FromWarehouseId = f.WarehouseId
                  LEFT JOIN Warehouses tw ON t.ToWarehouseId = tw.WarehouseId
                  WHERE t.TransferDate BETWEEN @f AND @t
                  ORDER BY t.TransferDate DESC",
                new() { { "@f", from.ToString("yyyy-MM-dd HH:mm:ss") }, { "@t", to.ToString("yyyy-MM-dd HH:mm:ss") } }),
            "Quantity")
        { }
    }
}
