using AthenaSearch.Items;

namespace AthenaSearch
{
    public class FileItem
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string[] Aliases { get; set; } = new string[0];

        public ISearchableItem ToSearchableItem()
        {
            return ApplicationItem.FromApplicationPath(Path, Name);
        }
    }
}