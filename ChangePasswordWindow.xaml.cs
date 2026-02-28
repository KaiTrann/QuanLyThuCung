using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace Nhóm_7
{
    public partial class ChangePasswordWindow : Window
    {
        public ChangePasswordWindow()
        {
            InitializeComponent();
            txtError.Text = "";
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            txtError.Text = "";

            if (Session.UserId <= 0)
            {
                txtError.Text = "Phiên đăng nhập không hợp lệ. Vui lòng đăng nhập lại.";
                return;
            }

            var oldPass = pbOld.Password ?? "";
            var newPass = pbNew.Password ?? "";
            var confirm = pbConfirm.Password ?? "";

            if (string.IsNullOrWhiteSpace(oldPass) ||
                string.IsNullOrWhiteSpace(newPass) ||
                string.IsNullOrWhiteSpace(confirm))
            {
                txtError.Text = "Vui lòng nhập đầy đủ thông tin.";
                return;
            }

            if (newPass.Length < 4)
            {
                txtError.Text = "Mật khẩu mới tối thiểu 4 ký tự.";
                return;
            }

            if (newPass != confirm)
            {
                txtError.Text = "Mật khẩu mới và nhập lại không khớp.";
                return;
            }

            try
            {
                // 1) Check mật khẩu cũ đúng không
                var dt = Db.Query(
                    "SELECT TOP 1 user_id FROM dbo.[users] WHERE user_id=@id AND password_hash=@p",
                    new SqlParameter("@id", SqlDbType.Int) { Value = Session.UserId },
                    new SqlParameter("@p", SqlDbType.NVarChar) { Value = oldPass }
                );

                if (dt.Rows.Count == 0)
                {
                    txtError.Text = "Mật khẩu cũ không đúng.";
                    return;
                }

                // 2) Update password_hash (DB của bạn chỉ có cột này, không có updated_at)
                var n = Db.Execute(
                    "UPDATE dbo.[users] SET password_hash=@newPass WHERE user_id=@id",
                    new SqlParameter("@newPass", SqlDbType.NVarChar) { Value = newPass },
                    new SqlParameter("@id", SqlDbType.Int) { Value = Session.UserId }
                );

                if (n <= 0)
                {
                    txtError.Text = "Không thể cập nhật mật khẩu.";
                    return;
                }

                txtError.Text = "✅ Đổi mật khẩu thành công.";
                pbOld.Clear();
                pbNew.Clear();
                pbConfirm.Clear();
                pbOld.Focus();
            }
            catch (Exception ex)
            {
                txtError.Text = "Lỗi: " + ex.Message;
            }
        }
    }
}