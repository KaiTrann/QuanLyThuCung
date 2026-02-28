using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace Nhóm_7
{
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
            // Nếu logo có load được thì ẩn icon fallback
            try
            {
                if (imgLoginLogo?.Source != null)
                    txtLoginFallbackIcon.Visibility = Visibility.Collapsed;
                else
                    txtLoginFallbackIcon.Visibility = Visibility.Visible;
            }
            catch
            {
                txtLoginFallbackIcon.Visibility = Visibility.Visible;
            }
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            var username = (txtUsername.Text ?? "").Trim();
            var password = pbPassword.Password ?? "";

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                txtMessage.Text = "Vui lòng nhập tên đăng nhập và mật khẩu.";
                return;
            }

            try
            {
                var dt = Db.Query(
                    "SELECT TOP 1 user_id, username, full_name, role, is_active, created_at " +
                    "FROM dbo.[users] " +
                    "WHERE username = @u AND password_hash = @p",
                    new SqlParameter("@u", SqlDbType.NVarChar) { Value = username },
                    new SqlParameter("@p", SqlDbType.NVarChar) { Value = password }
                );

                if (dt.Rows.Count == 0)
                {
                    txtMessage.Text = "Sai tài khoản hoặc mật khẩu.";
                    return;
                }

                var row = dt.Rows[0];
                var isActive = Convert.ToInt32(row["is_active"]) == 1;
                if (!isActive)
                {
                    txtMessage.Text = "Tài khoản đang bị khóa.";
                    return;
                }

                // ✅ Lưu session
                int userId = Convert.ToInt32(row["user_id"]);
                string un = row["username"]?.ToString();
                string fn = row["full_name"] == DBNull.Value ? "" : row["full_name"].ToString();
                string role = row["role"] == DBNull.Value ? "" : row["role"].ToString();
                DateTime? createdAt = row["created_at"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["created_at"]);

                Session.Set(userId, un, fn, role, createdAt);

                // ✅ mở dashboard (không MessageBox)
                var dash = new KhungApp();
                Application.Current.MainWindow = dash;
                dash.Show();

                Close();
            }
            catch (Exception ex)
            {
                txtMessage.Text = "Can't connect to SQL / Lỗi DB: " + ex.Message;
            }
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var reg = new RegisterWindow { Owner = this };
                var result = reg.ShowDialog();

                if (result == true && !string.IsNullOrWhiteSpace(reg.RegisteredUsername))
                {
                    txtUsername.Text = reg.RegisteredUsername;
                    pbPassword.Clear();
                    pbPassword.Focus();
                    txtMessage.Text = "Đăng ký thành công. Hãy đăng nhập.";
                }
            }
            catch (Exception ex)
            {
                txtMessage.Text = "Không mở được form đăng ký: " + ex.Message;
            }
        }
    }
}