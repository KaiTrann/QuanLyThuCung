using System;
using System.Windows;

namespace Nhóm_7
{
    public partial class Login : Window
    {
        private readonly LoginRepository repo = new LoginRepository();

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
                var result = repo.CheckLogin(username, password);

                if (result == null)
                {
                    txtMessage.Text = "Sai tài khoản hoặc mật khẩu.";
                    return;
                }

                if (result.IsActive != 1)
                {
                    txtMessage.Text = "Tài khoản đang bị khóa.";
                    return;
                }

                string fullName = string.IsNullOrWhiteSpace(result.FullName) ? result.Username : result.FullName;
                string role = result.Role ?? "";

                // Mở KhungApp
                var home = new KhungApp();
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