using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using AthenaSearch.Items;

namespace AthenaSearch.Common
{
    public class SearchItem : INotifyPropertyChanged
    {
        private bool _isSelected = false;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged();
            }
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public ImageSource Image { get; set; } = null;
        public ISearchableItem SourceItem { get; set; } = null;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}