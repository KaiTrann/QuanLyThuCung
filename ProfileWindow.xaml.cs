using System.Windows;

namespace Nhóm_7
{
    public partial class ProfileWindow : Window
    {
        public ProfileWindow()
        {
            InitializeComponent();

            txtUsername.Text = Session.Username ?? "";
            txtFullName.Text = Session.FullName ?? "";
            txtRole.Text = Session.Role ?? "";
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}