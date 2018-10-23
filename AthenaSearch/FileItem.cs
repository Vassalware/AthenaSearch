using AthenaSearch.Helpers;
using AthenaSearch.Items;

namespace AthenaSearch
{
    public class FileItem
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string IconPath { get; set; }
        public string Command { get; set; }
        public string Arguments { get; set; }

        // TODO: Implement aliases
        public string[] Aliases { get; set; } = new string[0];

        public ISearchableItem ToSearchableItem()
        {
            var name = Has(Name) ? Name : throw new System.Exception("Item in json file does not have 'Name' field.");

            if (Has(IconPath) && Has(Path))
                return new ApplicationItem(name, Path, IconHelper.GetIconFromExePath(IconPath), Arguments ?? "");

            if (Has(IconPath) && Has(Command))
                return new CommandItem(name, Command, IconHelper.GetIconFromExePath(IconPath));

            return new ApplicationItem(name, Path, IconHelper.GetIconFromExePath(Path), Arguments ?? "");
        }

        private bool Has(string value)
        {
            return !string.IsNullOrEmpty(value);
        }
    }
}