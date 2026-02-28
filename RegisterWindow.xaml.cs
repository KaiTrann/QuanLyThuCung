using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace Nhóm_7
{
    public partial class RegisterWindow : Window
    {
        public string RegisteredUsername { get; private set; }

        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            var username = (txtUsername.Text ?? "").Trim();
            var fullName = (txtFullName.Text ?? "").Trim();
            var password = txtPassword.Password ?? "";
            var confirm = txtConfirm.Password ?? "";

            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(fullName) ||
                string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.",
                    "Thiếu thông tin", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (password != confirm)
            {
                MessageBox.Show("Mật khẩu nhập lại không khớp.",
                    "Sai xác nhận", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (password.Length < 4)
            {
                MessageBox.Show("Mật khẩu tối thiểu 4 ký tự.",
                    "Mật khẩu yếu", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Kiểm tra trùng username
                var existed = Db.Scalar(
                    "SELECT COUNT(1) FROM dbo.[users] WHERE username = @u",
                    new SqlParameter("@u", SqlDbType.NVarChar) { Value = username }
                );

                if (Convert.ToInt32(existed) > 0)
                {
                    MessageBox.Show("Tên đăng nhập đã tồn tại.",
                        "Trùng tài khoản", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Insert có full_name
                Db.Execute(
                    "INSERT INTO dbo.[users] (username, password_hash, full_name, role, is_active, created_at) " +
                    "VALUES (@u, @p, @fn, N'staff', 1, GETDATE())",
                    new SqlParameter("@u", SqlDbType.NVarChar) { Value = username },
                    new SqlParameter("@p", SqlDbType.NVarChar) { Value = password },
                    new SqlParameter("@fn", SqlDbType.NVarChar) { Value = fullName }
                );

                RegisteredUsername = username;

                MessageBox.Show("Đăng ký thành công!", "OK",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đăng ký thất bại: " + ex.Message,
                    "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            Close();
        }
    }
}