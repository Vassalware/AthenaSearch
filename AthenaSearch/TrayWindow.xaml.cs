using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using AthenaSearch.Common;
using GlobalHotKey;
using ContextMenu = System.Windows.Forms.ContextMenu;

namespace AthenaSearch
{
    /// <summary>
    /// Interaction logic for StartWindow.xaml
    /// </summary>
    public partial class TrayWindow : Window
    {
        private SearchController _searchController;
        private NotifyIcon _notifyIcon;
        private ContextMenu _contextMenu;
        private SearchWindow _searchWindow;
        private HotKeyManager _hotKeyManager;

        public TrayWindow()
        {
            InitializeComponent();
            _searchController = new SearchController();
            this.Hide();
        }

        protected override void OnInitialized(EventArgs e)
        {
            _notifyIcon = new NotifyIcon();
            _notifyIcon.Text = "AthenaSearch";
            _notifyIcon.DoubleClick += _notifyIcon_DoubleClick;
            _notifyIcon.Icon = new Icon("Resources/tray_image.ico");
            _notifyIcon.Visible = true;

            _contextMenu = new ContextMenu();
            _contextMenu.MenuItems.Add("Edit items.json", _contextMenu_OpenItemsJson);
            _contextMenu.MenuItems.Add("-");
            _contextMenu.MenuItems.Add("Exit", _contextMenu_CloseClicked);

            _notifyIcon.ContextMenu = _contextMenu;

            _hotKeyManager = new HotKeyManager();
            _hotKeyManager.Register(Key.Enter, ModifierKeys.Control | ModifierKeys.Alt);
            _hotKeyManager.KeyPressed += _hotKeyManager_KeyPressed;

            base.OnInitialized(e);
        }

        private void _hotKeyManager_KeyPressed(object sender, KeyPressedEventArgs e)
        {
            if (e.HotKey.Key == Key.Enter)
            {
                OpenSearchWindow();
            }
        }

        private void _notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            OpenSearchWindow();
        }

        private void _contextMenu_OpenItemsJson(object sender, EventArgs e)
        {
            Process.Start("items.json");
        }

        private void _contextMenu_CloseClicked(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OpenSearchWindow()
        {
            _searchWindow = new SearchWindow(_searchController);
            _searchWindow.Show();
        }
    }
}
