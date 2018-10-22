using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using AthenaSearch.Items;
using Newtonsoft.Json;

namespace AthenaSearch.Common
{
    /// <summary>
    /// An object that contains the list of possible search result items, as well as facilitates
    /// retrieving a list of them based on the search string.
    /// </summary>
    public class SearchController
    {
        private List<ISearchableItem> _databaseItems = new List<ISearchableItem>();
        private Dictionary<string, ISearchableItem> _aliasDictionary = new Dictionary<string, ISearchableItem>();

        public SearchController()
        {
            var itemsFromFile = JsonConvert.DeserializeObject<List<FileItem>>(File.ReadAllText("items.json"));

            foreach (var item in itemsFromFile)
            {
                _databaseItems.Add(item.ToSearchableItem());

                foreach (var alias in item.Aliases)
                {
                    if (_aliasDictionary.ContainsKey(alias.ToLower()))
                    {
                        MessageBox.Show($"Error: Alias \"{alias}\" is referenced multiple times in items.json.");
                        throw new Exception();
                    }
                }
            }

            _databaseItems = _databaseItems.OrderBy(x => x.Name).ToList();
        }

        public SearchResult Search(string inputString)
        {
            var searchResult = new SearchResult();
            var list = new ObservableCollection<SearchItem>();

            if (string.IsNullOrWhiteSpace(inputString))
            {
                // No text input in search box, show all options.

                int index = 0;
                foreach (var item in _databaseItems)
                {
                    var si = new SearchItem
                    {
                        ID = index,
                        Image = item.Image,
                        Name = item.Name,
                        SourceItem = item
                    };

                    index++;

                    list.Add(si);
                }

                searchResult.SearchItemList = list;
                return searchResult;
            }
            else
            {
                _databaseItems = _databaseItems.OrderBy(x => x.Name).ToList();

                var items = _databaseItems.Where(x => x.Name.StartsWith(inputString, StringComparison.InvariantCultureIgnoreCase)).ToArray();

                int index = 0;
                foreach (var item in items)
                {
                    var si = new SearchItem
                    {
                        ID = index,
                        Image = item.Image,
                        Name = item.Name,
                        SourceItem = item
                    };

                    index++;

                    list.Add(si);
                }

                var nonDirectItems = _databaseItems.Where(x => !items.Contains(x)).Where(x => x.Name.ToLower().Contains(inputString.ToLower()));

                foreach (var item in nonDirectItems)
                {
                    var si = new SearchItem
                    {
                        ID = index,
                        Image = item.Image,
                        Name = item.Name,
                        SourceItem = item
                    };

                    index++;

                    list.Add(si);
                }

                searchResult.SearchItemList = list;
                return searchResult;
            }
        }
    }
}