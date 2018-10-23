using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using AthenaSearch.Helpers;

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

            // TODO: Handle this better.
            if (!fileInfo.Exists)
                throw new Exception();

            return new ApplicationItem(name, path, IconHelper.GetIconFromExePath(path));
        }

        public static ApplicationItem FromApplicationPath(string path, string name, string iconFromExecutablePath)
        {
            var fileInfo = new FileInfo(path);

            // TODO: Handle this better.
            if (!fileInfo.Exists)
                throw new Exception();

            return new ApplicationItem(name, path, IconHelper.GetIconFromExePath(iconFromExecutablePath));
        }

        public string Name => _name;
        public ImageSource Image => _image;
        public void Execute()
        {
            Process.Start(_path);
        }
    }
}