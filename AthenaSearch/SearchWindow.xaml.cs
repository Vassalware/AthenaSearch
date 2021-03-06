﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using AthenaSearch.Common;
using AthenaSearch.Helpers;

namespace AthenaSearch
{
    public partial class SearchWindow : Window, INotifyPropertyChanged
    {
        private SearchController _searchController;
        private string _searchSuggestionString;
        private bool _isClosing = false;

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
            // Enable Blur
            BlurHelper.EnableBlur(this);

            // Ensure the window gains focus.
            this.Activate();

            // Center the window.
            Left = SystemParameters.WorkArea.Width / 2f - Width / 2f;
            Top = SystemParameters.WorkArea.Height / 2f - Height / 2f;

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

            UpdateWindowHeight();
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

            UpdateSearchSuggestionString();
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Return immediately when called from InitializeComponent
            if (_searchController == null)
                return;

            var searchResult = _searchController.Search(SearchBox.Text);
            DisplaySearchItems = searchResult.SearchItemList;

            if (ResultListBox.Items.Count > 0)
            {
                ResultListBox.SelectedIndex = 0;
                UpdateSearchSuggestionString();
            }
            else
            {
                SearchSuggestionString = "";
            }

            UpdateWindowHeight();

            // Call UpdateWindowHeight after a short delay, so ResultListBox has time to update
            // its height after the DisplaySearchItems were changed. (Better way to do this?)
            Task.Delay(1).ContinueWith(_ => Dispatcher.Invoke(UpdateWindowHeight));
        }

        private void UpdateSearchSuggestionString()
        {
            if (ResultListBox.SelectedItem is SearchItem searchItem)
            {
                if (searchItem.Name.ToLower().StartsWith(SearchBox.Text.ToLower()))
                {
                    SearchSuggestionString = SearchBox.Text + searchItem.Name.Substring(SearchBox.Text.Length, searchItem.Name.Length - SearchBox.Text.Length);
                }
                else
                {
                    var loc = searchItem.Name.ToLower().IndexOf(SearchBox.Text.ToLower(), StringComparison.Ordinal);
                    var str = searchItem.Name.Substring(loc);
                    SearchSuggestionString = SearchBox.Text + str.Substring(SearchBox.Text.Length, str.Length - SearchBox.Text.Length);
                }
            }
        }

        private void ResultListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OnTraverse();
            UpdateWindowHeight();
        }

        private void SearchBox_Loaded(object sender, RoutedEventArgs e)
        {
            // Focus the Search Box when it's loaded.
            if (e.Source is TextBox textBox)
                textBox.Focus();
        }

        private void CloseWindow()
        {
            // Set _isClosing so we don't Close twice 
            _isClosing = true;
            Close();
        }

        private void UpdateWindowHeight()
        {
            // Update the height of the window so the transparency blur effect doesn't go beyond the ListBox area.
            Height = 68 + ResultListBox.ActualHeight;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void AthenaSearchWindow_Deactivated(object sender, EventArgs e)
        {
            if (!_isClosing && this.IsKeyboardFocusWithin)
            {
                this.Close();
            }
        }
    }
}
