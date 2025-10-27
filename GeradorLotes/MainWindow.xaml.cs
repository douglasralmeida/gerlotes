using GeradorLotes.Views;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using WinRT.Interop;

namespace GeradorLotes
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public Action NavigationViewLoaded { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            ExtendsContentIntoTitleBar = true; // Extend the content into the title bar and hide the default title bar
            AppWindow.TitleBar.PreferredHeightOption = Microsoft.UI.Windowing.TitleBarHeightOption.Tall;
            SetTitleBar(TitleBar); // Set the custom title bar

            AppWindow.Resize(new Windows.Graphics.SizeInt32(1100, 660));

            var hwnd = WindowNative.GetWindowHandle(this);
            Microsoft.UI.WindowId windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hwnd);
            var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);

            appWindow.SetIcon("Assets/AppIcon.ico");
        }

        private void OnNavigationViewControlLoaded(object sender, RoutedEventArgs e)
        {
            // Delay necessary to ensure NavigationView visual state can match navigation
            Task.Delay(500).ContinueWith(_ => this.NavigationViewLoaded?.Invoke(), TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void OnNavigationViewSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            var selectedItem = args.SelectedItemContainer;
            if (selectedItem == TransferenciasItem)
            {
                if (quadroRaiz.CurrentSourcePageType != typeof(PaginaTransferencias))
                {
                    Navigate(typeof(PaginaTransferencias));
                }
            }
        }

        public void Navigate(Type pageType, object targetPageArguments = null, NavigationTransitionInfo navigationTransitionInfo = null)
        {
            quadroRaiz.Navigate(pageType, targetPageArguments, navigationTransitionInfo);
        }

        private void TitleBar_PaneToggleRequested(TitleBar sender, object args)
        {
            NavigationViewPadrao.IsPaneOpen = !NavigationViewPadrao.IsPaneOpen;
        }

        private void TitleBar_BackRequested(TitleBar sender, object args)
        {
            quadroRaiz.GoBack();
        }
    }
}
