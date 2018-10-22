using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using AthenaSearch.Common;

namespace AthenaSearch
{
    public partial class SearchWindow : Window, INotifyPropertyChanged
    {
        private SearchController _searchController;
        private string _searchSuggestionString;
        private double _top = int.MinValue;

        public string SearchSuggestionString
        {
            get => _searchSuggestionString;
            set
            {
                _searchSuggestionString = value;
                SearchSuggestion.Text = _searchSuggestionString;
            }
        }

        private ObservableCollection<SearchItem> _displaySearchItem;
        public ObservableCollection<SearchItem> DisplaySearchItems
        {
            get => _displaySearchItem;
            set
            {
                _displaySearchItem = value;
                OnPropertyChanged();
            }
        }

        public SearchWindow(SearchController searchController)
        {
            InitializeComponent();
            _searchController = searchController;
            var searchResult = _searchController.Search("");
            DisplaySearchItems = searchResult.SearchItemList;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Center the window.
            Left = SystemParameters.WorkArea.Width / 2f - Width / 2f;
            Top = SystemParameters.WorkArea.Height / 2f - Height / 2f;

            // Save centered Top property.
            _top = Top;

            // Start animations
            var topAnimation = new DoubleAnimation(Top - 50, Top, new Duration(TimeSpan.FromSeconds(0.4f)))
            {
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            var opacityAnimation = new DoubleAnimation(0f, 1f, new Duration(TimeSpan.FromSeconds(0.4f)))
            {
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            this.BeginAnimation(Window.TopProperty, topAnimation);
            this.BeginAnimation(Window.OpacityProperty, opacityAnimation);
        }

        private void SearchBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Down:
                    ResultListBox.SelectedIndex = MathHelper.Clamp(ResultListBox.SelectedIndex + 1, 0, ResultListBox.Items.Count - 1);
                    OnTraverse();
                    break;

                case Key.Up:
                    ResultListBox.SelectedIndex = MathHelper.Clamp(ResultListBox.SelectedIndex - 1, 0, ResultListBox.Items.Count - 1);
                    OnTraverse();
                    break;

                case Key.Enter:
                    if (ResultListBox.SelectedItem != null && ResultListBox.SelectedItem is SearchItem searchItem)
                        searchItem.SourceItem.Execute();
                    CloseWindow();
                    break;

                case Key.Escape:
                    CloseWindow();
                    break;
            }
        }

        private void OnTraverse()
        {
            ResultListBox.ScrollIntoView(ResultListBox.SelectedItem);
            SearchBox.Focus();

            foreach (var item in DisplaySearchItems)
            {
                var val = item.ID == ResultListBox.SelectedIndex;
                item.IsSelected = val;
            }

            if (ResultListBox.SelectedItem is SearchItem searchItem)
            {
                SearchSuggestionString = searchItem.Name;
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Return immediately when called from InitializeComponent
            if (_searchController == null)
                return;

            if (e.Source is TextBox textBox)
            {
                var searchResult = _searchController.Search(textBox.Text);
                DisplaySearchItems = searchResult.SearchItemList;

                if (ResultListBox.Items.Count > 0)
                {
                    ResultListBox.SelectedIndex = 0;

                    if (ResultListBox.SelectedItem is SearchItem searchItem)
                    {
                        SearchSuggestionString = SearchBox.Text + searchItem.Name.Substring(SearchBox.Text.Length, searchItem.Name.Length - SearchBox.Text.Length);
                    }
                }
                else
                {
                    SearchSuggestionString = "";
                }
            }
        }

        private void ResultListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OnTraverse();
        }

        private void SearchBox_Loaded(object sender, RoutedEventArgs e)
        {
            // Focus the Search Box when it's loaded.
            if (e.Source is TextBox textBox)
                textBox.Focus();
        }

        private void CloseWindow()
        {
            /*
            var topAnimation = new DoubleAnimation(Top, Top + 10, new Duration(TimeSpan.FromSeconds(0.2f)))
            {
                //EasingFunction = new CubicEase { EasingMode = EasingMode.EaseIn }
            };

            var opacityAnimation = new DoubleAnimation(1f, 0f, new Duration(TimeSpan.FromSeconds(0.2f)))
            {
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseIn }
            };

            topAnimation.Completed += (o, e) =>
            {
                Close();
            };

            this.BeginAnimation(Window.TopProperty, topAnimation);
            this.BeginAnimation(Window.OpacityProperty, opacityAnimation);
            */

            this.Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
