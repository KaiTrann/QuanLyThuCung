using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Nhóm_7
{
    public partial class KhungApp : Window
    {
        public KhungApp()
        {
            InitializeComponent();

            try
            {
                var ver = Db.Scalar("SELECT VERSION();")?.ToString();
                MessageBox.Show("Kết nối OK!\nMySQL Version: " + ver);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối:\n" + ex.Message);
            }
        }
    }
}
