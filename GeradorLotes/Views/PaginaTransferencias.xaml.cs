using GeradorLotes.Services;
using GeradorLotes.ViewModels;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System.Threading.Tasks;
using Windows.System;
using static GeradorLotes.Services.MessageService;

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

            MessageService.Instance.MessageRequested += (title, message, severity) =>
            {
                DispatcherQueue.TryEnqueue(() => MostrarMensagem(title, message, severity));
            };

            DataContext = ViewModel;
        }

        private async void MostrarMensagem(string title, string message, MessageSeverity severity)
        {
            ExportInfoBar.Title = title;
            ExportInfoBar.Message = message;

            ExportInfoBar.Severity = severity switch
            {
                MessageSeverity.Success => InfoBarSeverity.Success,
                MessageSeverity.Warning => InfoBarSeverity.Warning,
                MessageSeverity.Error => InfoBarSeverity.Error,
                _ => InfoBarSeverity.Informational
            };

            ExportInfoBar.IsOpen = true;
            await Task.Delay(6000);
            ExportInfoBar.IsOpen = false;
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
