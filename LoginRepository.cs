using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace Nhóm_7
{
    public class LoginRepository
    {
        public class LoginResult
        {
            public int UserId { get; set; }
            public string Username { get; set; } = "";
            public string FullName { get; set; } = "";
            public string Role { get; set; } = "";
            public int IsActive { get; set; }
        }

        public LoginResult CheckLogin(string username, string password)
        {
            const string sql =
                "SELECT user_id, username, full_name, role, is_active " +
                "FROM users " +
                "WHERE username=@u AND password_hash=@p " +
                "LIMIT 1;";

            DataTable dt = Db.Query(sql,
                new MySqlParameter("@u", username),
                new MySqlParameter("@p", password)
            );

            if (dt.Rows.Count == 0) return null;

            var row = dt.Rows[0];

            return new LoginResult
            {
                UserId = Convert.ToInt32(row["user_id"]),
                Username = row["username"]?.ToString() ?? "",
                FullName = row["full_name"]?.ToString() ?? "",
                Role = row["role"]?.ToString() ?? "",
                IsActive = Convert.ToInt32(row["is_active"])
            };
        }
    }
}