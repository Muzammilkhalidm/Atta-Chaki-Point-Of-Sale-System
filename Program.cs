using AttaChakiPOS.Database;
using AttaChakiPOS.Forms.Login;

namespace AttaChakiPOS
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            DbHelper.Initialize();
            Application.Run(new FrmLogin());
        }
    }
}
