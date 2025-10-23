using GeradorLotes.Views;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using System;
using System.Threading.Tasks;

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
    }
}
