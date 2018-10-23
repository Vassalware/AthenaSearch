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
        private readonly string _name;
        private readonly string _path;
        private readonly ImageSource _image;

        public ApplicationItem(string name, string path, ImageSource image)
        {
            _name = name;
            _path = path;
            _image = image;
        }

        public string Name => _name;
        public ImageSource Image => _image;

        public void Execute()
        {
            Process.Start(_path);
        }
    }
}