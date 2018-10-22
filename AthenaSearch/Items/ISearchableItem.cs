using System.Windows.Media;

namespace AthenaSearch.Items
{
    public interface ISearchableItem
    {
        string Name { get; }
        ImageSource Image { get; }
        void Execute();
    }
}