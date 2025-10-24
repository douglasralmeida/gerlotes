using GeradorLotes.Commands;
using GeradorLotes.Helpers;
using GeradorLotes.Models;
using GeradorLotes.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using WinRT.Interop;

namespace GeradorLotes.ViewModels
{
    public class TransferenciaViewModel: INotifyPropertyChanged
    {
        public ObservableCollection<Protocolo> Protocolos { get; } = new();

        private string _despacho = string.Empty;
        public string Despacho
        {
            get => _despacho;
            set
            {
                if (_despacho != value)
                {
                    _despacho = value;
                    OnPropertyChanged();
                    ExportarCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private string _protocoloDigitado = string.Empty;
        public string ProtocoloDigitado
        {
            get => _protocoloDigitado;
            set
            {
                if (_protocoloDigitado != value)
                {
                    _protocoloDigitado = value;
                    OnPropertyChanged();
                    AdicionarCommand.RaiseCanExecuteChanged();
                }   
            }
        }

        private Protocolo? _protocoloSelecionado;
        public Protocolo? ProtocoloSelecionado
        {
            get => _protocoloSelecionado;
            set
            {
                if (_protocoloSelecionado != value)
                {
                    _protocoloSelecionado = value;
                    OnPropertyChanged(nameof(ProtocoloSelecionado));
                    ExcluirCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private string _unidadeDestino = string.Empty;
        public string UnidadeDestino
        {
            get => _unidadeDestino;
            set
            {
                if (_unidadeDestino != value)
                {
                    _unidadeDestino = value;
                    OnPropertyChanged();
                    ExportarCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public RelayCommand AdicionarCommand { get; }
        public RelayCommand ColarCommand { get; }
        public RelayCommand<Protocolo> ExcluirCommand { get; }
        public RelayCommand ExportarCommand { get; }

        public TransferenciaViewModel()
        {
            AdicionarCommand = new RelayCommand(Adicionar, PodeAdicionar);
            ColarCommand = new RelayCommand(async () => await ColarDaAreaDeTransferencia());
            ExcluirCommand = new RelayCommand<Protocolo>(Excluir, PodeExcluir);
            ExportarCommand = new RelayCommand(async () => await ExportarCsv(), PodeExportar);

            // Monitora mudanças na coleção
            Protocolos.CollectionChanged += (s, e) =>
            {
                //AdicionarCommand.RaiseCanExecuteChanged();
                ExportarCommand.RaiseCanExecuteChanged();
            };
        }

        public void Adicionar()
        {
            if (!Protocolos.Any(p => p.Numero == ProtocoloDigitado))
            {
                Protocolos.Add(new Protocolo(ProtocoloDigitado));
                ProtocoloDigitado = string.Empty;
                //ExportarCommand.RaiseCanExecuteChanged();
            }
        }

        private async Task ColarDaAreaDeTransferencia()
        {
            var texto = await ClipboardHelper.ObterTextoAsync();
            if (!string.IsNullOrWhiteSpace(texto))
            {
                var linhas = texto.Split(new[] { '\r', '\n', ';', ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var linha in linhas)
                {
                    var protocolo = linha.Trim();
                    if (!string.IsNullOrWhiteSpace(protocolo) && !Protocolos.Any(p => p.Numero == protocolo))
                    {
                        Protocolos.Add(new Protocolo(protocolo));
                    }
                }
                ExportarCommand.RaiseCanExecuteChanged();
            }
        }

        private async Task ExportarCsv()
        {
            var picker = new FileSavePicker();

            // Inicializa com a janela atual (necessário no WinUI 3)
            var hwnd = WindowNative.GetWindowHandle(App.MainWindow);
            InitializeWithWindow.Initialize(picker, hwnd);

            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            picker.FileTypeChoices.Add("Arquivo CSV", new List<string>() { ".csv" });
            picker.SuggestedFileName = "protocolos";

            StorageFile file = await picker.PickSaveFileAsync();
            if (file != null)
            {
                var lista = Protocolos.Select(p => p.Numero);
                await CsvExporter.ExportAsync(file, lista, UnidadeDestino, Despacho);

                MessageService.Instance.Show(
                    "Exportação concluída",
                    "O arquivo CSV foi gerado com sucesso.",
                    MessageService.MessageSeverity.Success);
            }
        }

        public void Excluir(Protocolo item)
        {
            if (item != null)
            {
                Protocolos.Remove(item);
                if (ProtocoloSelecionado == item)
                    ProtocoloSelecionado = null;
                //ExportarCommand.RaiseCanExecuteChanged();
            }
        }

        public int Importar(IEnumerable<string> numeros)
        {
            int adicionados = 0;
            foreach (var numero in numeros.Distinct())
            {
                if (!Protocolos.Any(p => p.Numero == numero))
                {
                    Protocolos.Add(new Protocolo(numero));
                    adicionados++;
                }
            }
            //ExportarCommand.RaiseCanExecuteChanged();
            return adicionados;
        }

        private bool PodeAdicionar() => !string.IsNullOrWhiteSpace(ProtocoloDigitado) && !Protocolos.Any(p => p.Numero == ProtocoloDigitado);
        private bool PodeExcluir(Protocolo item) => item != null;
        private bool PodeExportar() => Protocolos.Any() &&
            !string.IsNullOrWhiteSpace(UnidadeDestino) && UnidadeDestino.Length > 1 &&
            !string.IsNullOrWhiteSpace(Despacho) && Despacho.Length > 1;

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
