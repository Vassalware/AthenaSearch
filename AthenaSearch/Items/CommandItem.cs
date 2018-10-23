using System.Diagnostics;
using System.Windows.Media;

namespace AthenaSearch.Items
{
    public class CommandItem : ISearchableItem
    {
        private string _name;
        private string _command;
        private ImageSource _image;

        public CommandItem(string name, string command, ImageSource image)
        {
            _name = name;
            _command = command;
            _image = image;
        }

        public string Name => _name;
        public ImageSource Image => _image;

        public void Execute()
        {
            new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    FileName = "cmd.exe",
                    Arguments = $"/C {_command}"
                }
            }.Start();
        }
    }
}