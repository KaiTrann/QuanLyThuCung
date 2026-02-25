using MySqlConnector;
using System;
using System.Data;
using System.Windows;

namespace Nhóm_7
{
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
            txtMessage.Text = "Vui lòng nhập tài khoản.";
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            var username = txtUsername.Text?.Trim();
            var password = pbPassword.Password ?? "";

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                txtMessage.Text = "Vui lòng nhập tên đăng nhập và mật khẩu.";
                return;
            }

            try
            {
                // Demo đơn giản: password_hash đang lưu mật khẩu thường để test
                const string sql =
                    "SELECT user_id, username, full_name, role, is_active " +
                    "FROM users " +
                    "WHERE username=@u AND password_hash=@p " +
                    "LIMIT 1;";

                DataTable dt = Db.Query(sql,
                    new MySqlParameter("@u", username),
                    new MySqlParameter("@p", password)
                );

                if (dt.Rows.Count == 0)
                {
                    txtMessage.Text = "Sai tài khoản hoặc mật khẩu.";
                    return;
                }

                int isActive = Convert.ToInt32(dt.Rows[0]["is_active"]);
                if (isActive != 1)
                {
                    txtMessage.Text = "Tài khoản đang bị khóa.";
                    return;
                }

                // (Optional) lấy thông tin user
                string fullName = dt.Rows[0]["full_name"]?.ToString() ?? username;
                string role = dt.Rows[0]["role"]?.ToString() ?? "";

                // Mở KhungApp
                var home = new KhungApp();

                // (Optional) nếu KhungApp bạn có TextBlock hiển thị tên/role thì truyền qua constructor,
                // hoặc set static CurrentUser. Tạm thời cứ mở.
                home.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                txtMessage.Text = "Lỗi kết nối/SQL: " + ex.Message;
            }
        }
    }
}