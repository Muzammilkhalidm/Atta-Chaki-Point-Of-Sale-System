using System.Data;
using Microsoft.Data.Sqlite;

namespace AttaChakiPOS.Database
{
    /// <summary>
    /// Central database access helper. Every form in the system talks to
    /// SQLite through this single class so connection handling and error
    /// behavior stays consistent across all 50+ forms.
    /// </summary>
    public static class DbHelper
    {
        private static string _dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AttaChakiPOS.db");
        public static string ConnectionString => $"Data Source={_dbPath}";

        public static void Initialize()
        {
            bool freshDb = !File.Exists(_dbPath);
            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();

            string schemaPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database", "Schema.sql");
            if (File.Exists(schemaPath))
            {
                string script = File.ReadAllText(schemaPath);
                using var cmd = conn.CreateCommand();
                cmd.CommandText = script;
                cmd.ExecuteNonQuery();
            }
        }

        public static DataTable GetDataTable(string sql, Dictionary<string, object>? parameters = null)
        {
            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            AddParams(cmd, parameters);

            using var reader = cmd.ExecuteReader();
            var table = new DataTable();
            table.Load(reader);
            return table;
        }

        public static object? ExecuteScalar(string sql, Dictionary<string, object>? parameters = null)
        {
            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            AddParams(cmd, parameters);
            return cmd.ExecuteScalar();
        }

        public static int ExecuteNonQuery(string sql, Dictionary<string, object>? parameters = null)
        {
            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            AddParams(cmd, parameters);
            return cmd.ExecuteNonQuery();
        }

        public static long ExecuteInsertReturnId(string sql, Dictionary<string, object>? parameters = null)
        {
            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            AddParams(cmd, parameters);
            cmd.ExecuteNonQuery();

            using var idCmd = conn.CreateCommand();
            idCmd.CommandText = "SELECT last_insert_rowid();";
            return (long)(idCmd.ExecuteScalar() ?? 0L);
        }

        private static void AddParams(SqliteCommand cmd, Dictionary<string, object>? parameters)
        {
            if (parameters == null) return;
            foreach (var kv in parameters)
                cmd.Parameters.AddWithValue(kv.Key, kv.Value ?? DBNull.Value);
        }

        public static string NextDocumentNumber(string prefix, string tableName, string columnName)
        {
            var count = ExecuteScalar($"SELECT COUNT(*) FROM {tableName}");
            long next = Convert.ToInt64(count ?? 0) + 1;
            return $"{prefix}-{DateTime.Now:yyyyMMdd}-{next:D4}";
        }
    }
}
