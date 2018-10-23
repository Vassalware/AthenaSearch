using System.Drawing;
using System.Windows;
using System.Windows.Media.Imaging;

namespace AthenaSearch.Helpers
{
    public class IconHelper
    {
        public static BitmapSource GetIconFromExePath(string path)
        {
            using (var icon = Icon.ExtractAssociatedIcon(path))
            {
                return System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
        }
    }
}