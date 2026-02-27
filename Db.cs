using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Nhóm_7
{
    public static class Db
    {
        private static string ConnStr
        {
            get
            {
                var cs = ConfigurationManager.ConnectionStrings["QuanLyThuCungDb"]?.ConnectionString;
                return string.IsNullOrWhiteSpace(cs)
                    ? throw new ConfigurationErrorsException("Thiếu connectionStrings/QuanLyThuCungDb trong App.config")
                    : cs;
            }
        }

        private static SqlCommand Create(string sql, SqlConnection conn, SqlParameter[] p)
        {
            var cmd = new SqlCommand(sql, conn);
            if (p != null && p.Length > 0) cmd.Parameters.AddRange(p);
            return cmd;
        }

        public static DataTable Query(string sql, params SqlParameter[] p)
        {
            using (var conn = new SqlConnection(ConnStr))
            {
                conn.Open();
                using (var cmd = Create(sql, conn, p))
                using (var r = cmd.ExecuteReader())
                {
                    var dt = new DataTable();
                    dt.Load(r);
                    return dt;
                }
            }
        }

        public static int Execute(string sql, params SqlParameter[] p)
        {
            using (var conn = new SqlConnection(ConnStr))
            {
                conn.Open();
                using (var cmd = Create(sql, conn, p))
                    return cmd.ExecuteNonQuery();
            }
        }

        public static object Scalar(string sql, params SqlParameter[] p)
        {
            using (var conn = new SqlConnection(ConnStr))
            {
                conn.Open();
                using (var cmd = Create(sql, conn, p))
                    return cmd.ExecuteScalar();
            }
        }
    }
}