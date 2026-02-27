using System;
using System.IO;
using System.Windows;

namespace Nhóm_7
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Lấy thư mục project (không phải bin\Debug)
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string projectDir = Path.GetFullPath(Path.Combine(baseDir, @"..\..\"));

            AppDomain.CurrentDomain.SetData("DataDirectory", projectDir);
        }
    }
}