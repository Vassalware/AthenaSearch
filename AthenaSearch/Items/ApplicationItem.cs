using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AthenaSearch.Items
{
    public class ApplicationItem : ISearchableItem
    {
        private string _name;
        private string _path;
        private ImageSource _image;

        public ApplicationItem(string name, string path, ImageSource image)
        {
            _name = name;
            _path = path;
            _image = image;
        }

        public static ApplicationItem FromApplicationPath(string path, string name)
        {
            var fileInfo = new FileInfo(path);
            if (!fileInfo.Exists)
                throw new System.Exception();

            using (var icon = Icon.ExtractAssociatedIcon(path))
            {
                var bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

                return new ApplicationItem(name, path, bitmapSource);
            }
        }

        public string Name => _name;
        public ImageSource Image => _image;
        public void Execute()
        {
            Process.Start(_path);
        }
    }
}