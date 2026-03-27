using System.Data;
using Microsoft.Data.SqlClient;

namespace BVS2.Database
{
    public static class DatabaseHelper
    {
        private static readonly string ConnectionString =
            @"Server=localhost\SQLEXPRESS01;Database=BVS_DB2;Integrated Security=True;TrustServerCertificate=True;";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        public static DataTable ExecuteQuery(string spName, SqlParameter[]? parameters = null)
        {
            DataTable dt = new DataTable();
            using SqlConnection conn = GetConnection();
            using SqlCommand cmd = new SqlCommand(spName, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            if (parameters != null) cmd.Parameters.AddRange(parameters);
            conn.Open();
            using SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return dt;
        }

        public static int ExecuteNonQuery(string spName, SqlParameter[]? parameters = null)
        {
            using SqlConnection conn = GetConnection();
            using SqlCommand cmd = new SqlCommand(spName, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            if (parameters != null) cmd.Parameters.AddRange(parameters);
            conn.Open();
            return cmd.ExecuteNonQuery();
        }

        public static DataTable ExecuteRawQuery(string sql, SqlParameter[]? parameters = null)
        {
            DataTable dt = new DataTable();
            using SqlConnection conn = GetConnection();
            using SqlCommand cmd = new SqlCommand(sql, conn);
            if (parameters != null) cmd.Parameters.AddRange(parameters);
            conn.Open();
            using SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return dt;
        }
    }
}