using System;
using System.Data;
using System.Data.SqlClient;

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
                "SELECT TOP 1 user_id, username, full_name, role, is_active " +
                "FROM users " +
                "WHERE username=@u AND password_hash=@p;";

            DataTable dt = Db.Query(
                sql,
                new SqlParameter("@u", username),
                new SqlParameter("@p", password)
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
        public bool UsernameExists(string username)
        {
            var obj = Db.Scalar(
                "SELECT COUNT(*) FROM dbo.[users] WHERE username=@u",
                new SqlParameter("@u", username)
            );
            return Convert.ToInt32(obj) > 0;
        }

        public bool Register(string username, string password, string fullName, string role)
        {
            int rows = Db.Execute(@"
                INSERT INTO dbo.[users] (username,password_hash,full_name,role,is_active,created_at)
                VALUES (@u,@p,@f,@r,1,GETDATE())",
                new SqlParameter("@u", username),
                new SqlParameter("@p", password),
                new SqlParameter("@f", string.IsNullOrWhiteSpace(fullName) ? (object)DBNull.Value : fullName),
                new SqlParameter("@r", role)
            );

            return rows == 1;
        }
    }
}