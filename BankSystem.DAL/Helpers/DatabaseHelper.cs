using System.Data;
using Microsoft.Data.SqlClient;
using BankSystem.DAL.Settings;

namespace BankSystem.DAL.Helpers
{
    public static class DatabaseHelper
    {
        private static readonly string _connectionString = DataSettings.ConnectionString;

        public static async Task<SqlConnection> GetOpenConnectionAsync()
        {
            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }
        public static SqlParameter CreateParameter(string name, SqlDbType type, object value)
        {
            return new SqlParameter(name, type) { Value = value ?? DBNull.Value };
        }
        public static SqlParameter CreateOutputParameter(string name, SqlDbType type)
        {
            return new SqlParameter(name, type) { Direction = ParameterDirection.Output };
        }
        public static SqlParameter CreateDecimalOutputParameter(string name, SqlDbType type, byte precision, byte scale)
        {
            return new SqlParameter(name, type)
            {
                Precision = precision,
                Scale = scale,
                Direction = ParameterDirection.Output,
            };
        }
        //
        public static Int32? GetInt32OrNull(this SqlDataReader reader, string columnName)
        {
            int idx = reader.GetOrdinal(columnName);
            return (reader.IsDBNull(idx)) ? null : reader.GetInt32(idx);
        }
        public static string? GetStringOrNull(this SqlDataReader reader, string columnName)
        {
            int idx = reader.GetOrdinal(columnName);
            return (reader.IsDBNull(idx)) ? null : reader.GetString(idx);
        }
        public static decimal? GetDecimalOrNull(this SqlDataReader reader, string columnName)
        {
            int idx = reader.GetOrdinal(columnName);
            return (reader.IsDBNull(idx)) ? null : reader.GetDecimal(idx);
        }
        public static DateTime? GetDateTimeOrNull(this SqlDataReader reader, string columnName)
        {
            int idx = reader.GetOrdinal(columnName);
            return (reader.IsDBNull(idx)) ? null : reader.GetDateTime(idx);
        }
    }
}