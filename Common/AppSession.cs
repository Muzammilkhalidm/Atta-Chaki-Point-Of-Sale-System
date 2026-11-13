namespace AttaChakiPOS.Common
{
    /// <summary>Holds the currently logged-in user for the whole application session.</summary>
    public static class AppSession
    {
        public static int UserId { get; set; }
        public static string FullName { get; set; } = string.Empty;
        public static string UserName { get; set; } = string.Empty;
        public static string RoleName { get; set; } = string.Empty;
        public static int RoleId { get; set; }

        public static bool IsAdmin => RoleId == 1;
    }
}
