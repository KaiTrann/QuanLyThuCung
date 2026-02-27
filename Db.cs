using MySql.Data.MySqlClient;
using System.Data;

namespace Nhóm_7
{
    public static class Db
    {
        private const string ConnStr =
            "Server=localhost;Port=3307;Database=nhom7;Uid=khanh;Pwd=123456;";

        private static MySqlCommand Create(string sql, MySqlConnection conn, MySqlParameter[] p)
        {
            var cmd = new MySqlCommand(sql, conn);
            if (p != null && p.Length > 0)
                cmd.Parameters.AddRange(p);
            return cmd;
        }

        public static DataTable Query(string sql, params MySqlParameter[] p)
        {
            using (var conn = new MySqlConnection(ConnStr))
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

        public static int Execute(string sql, params MySqlParameter[] p)
        {
            using (var conn = new MySqlConnection(ConnStr))
            {
                conn.Open();
                using (var cmd = Create(sql, conn, p))
                    return cmd.ExecuteNonQuery();
            }
        }

        public static object Scalar(string sql, params MySqlParameter[] p)
        {
            using (var conn = new MySqlConnection(ConnStr))
            {
                conn.Open();
                using (var cmd = Create(sql, conn, p))
                    return cmd.ExecuteScalar();
            }
        }
    }
}