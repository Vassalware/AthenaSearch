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
        private readonly string _arguments;
        private readonly ImageSource _image;

        public ApplicationItem(string name, string path, ImageSource image, string arguments = "")
        {
            _name = name;
            _path = path;
            _image = image;
            _arguments = arguments;
        }

        public string Name => _name;
        public ImageSource Image => _image;

        public void Execute()
        {
            new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = _path,
                    Arguments = _arguments
                }
            }.Start();
        }
    }
}