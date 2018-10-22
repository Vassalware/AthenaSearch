using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AthenaSearch.Common
{
    public class SearchResult
    {
        public ObservableCollection<SearchItem> SearchItemList { get; set; }
    }
}