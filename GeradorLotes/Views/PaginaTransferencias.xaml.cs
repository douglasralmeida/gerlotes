using GeradorLotes.ViewModels;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Windows.System;

namespace GeradorLotes.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PaginaTransferencias : Page
    {
        public TransferenciaViewModel ViewModel { get; } = new();

        public PaginaTransferencias()
        {
            InitializeComponent();
            DataContext = ViewModel;
        }

        private void ProtocoloBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter && ViewModel.AdicionarCommand.CanExecute(null))
            {
                ViewModel.AdicionarCommand.Execute(null);
                e.Handled = true; // evita som de alerta ou comportamento padrão
            }
        }
    }
}
