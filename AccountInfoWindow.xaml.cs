using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace Nhóm_7
{
    public partial class AccountInfoWindow : Window
    {
        public AccountInfoWindow()
        {
            InitializeComponent();
            Loaded += AccountInfoWindow_Loaded;
        }

        private void AccountInfoWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // ưu tiên lấy từ DB cho đúng created_at
                var dt = Db.Query(
                    "SELECT TOP 1 username, full_name, created_at " +
                    "FROM dbo.[users] WHERE user_id = @id",
                    new SqlParameter("@id", SqlDbType.Int) { Value = Session.UserId }
                );

                if (dt.Rows.Count > 0)
                {
                    var r = dt.Rows[0];
                    txtUsername.Text = r["username"]?.ToString() ?? "—";
                    txtFullName.Text = r["full_name"] == DBNull.Value ? "—" : r["full_name"].ToString();

                    if (r["created_at"] != DBNull.Value)
                    {
                        var d = Convert.ToDateTime(r["created_at"]);
                        txtCreatedAt.Text = d.ToString("dd/MM/yyyy HH:mm");
                    }
                    else txtCreatedAt.Text = "—";
                }
                else
                {
                    txtUsername.Text = Session.Username ?? "—";
                    txtFullName.Text = string.IsNullOrWhiteSpace(Session.FullName) ? "—" : Session.FullName;
                    txtCreatedAt.Text = Session.CreatedAt.HasValue ? Session.CreatedAt.Value.ToString("dd/MM/yyyy HH:mm") : "—";
                }
            }
            catch
            {
                txtUsername.Text = Session.Username ?? "—";
                txtFullName.Text = string.IsNullOrWhiteSpace(Session.FullName) ? "—" : Session.FullName;
                txtCreatedAt.Text = Session.CreatedAt.HasValue ? Session.CreatedAt.Value.ToString("dd/MM/yyyy HH:mm") : "—";
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}