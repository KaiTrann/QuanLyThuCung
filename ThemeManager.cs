using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Nhóm_7
{
    public static class ThemeManager
    {
        public static bool IsDark { get; private set; }

        public static void Apply(bool dark)
        {
            IsDark = dark;

            // Palette (giữ Accent của bạn)
            var bg = dark ? Color.FromRgb(0x0B, 0x12, 0x20) : (Color)ColorConverter.ConvertFromString("#F6F7FB");
            var surface = dark ? Color.FromRgb(0x10, 0x1A, 0x2D) : Colors.White;
            var stroke = dark ? Color.FromRgb(0x22, 0x2F, 0x45) : (Color)ColorConverter.ConvertFromString("#E6E8F0");
            var text = dark ? Color.FromRgb(0xF2, 0xF5, 0xFA) : (Color)ColorConverter.ConvertFromString("#111827");
            var text2 = dark ? Color.FromRgb(0xA8, 0xB3, 0xC7) : (Color)ColorConverter.ConvertFromString("#6B7280");
            var accent = (Color)ColorConverter.ConvertFromString("#6D28D9"); // giữ nguyên

            // Update tất cả window đang mở (vì KhungApp đặt Resource trong Window.Resources)
            foreach (Window w in Application.Current.Windows)
            {
                SetBrushIfExists(w.Resources, "BgBrush", bg);
                SetBrushIfExists(w.Resources, "SurfaceBrush", surface);
                SetBrushIfExists(w.Resources, "StrokeBrush", stroke);
                SetBrushIfExists(w.Resources, "TextBrush", text);
                SetBrushIfExists(w.Resources, "Text2Brush", text2);
                SetBrushIfExists(w.Resources, "AccentBrush", accent);

                // Một số chỗ bạn hard-code "#F6F7FB" trong style InputBox/Menu hover
                // => không thể thay hết nếu không đổi XAML.
                // Nhưng phần nền/khung/chữ sẽ đổi đúng.
            }
        }

        private static void SetBrushIfExists(ResourceDictionary res, string key, Color c)
        {
            if (res.Contains(key))
                res[key] = new SolidColorBrush(c);
        }
    }
}