using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;

namespace GeradorLotes.Helpers
{
    public static class CsvExporter
    {
        public static async Task ExportAsync(StorageFile file, IEnumerable<string> protocolos, string unidadeDestino, string despacho)
        {
            var linhas = new List<string>
            {
                "12",
                "Protocolo;UO;Despacho"
            };

            foreach (var protocolo in protocolos)
            {
                linhas.Add($"{protocolo};{unidadeDestino};{despacho}");
            }

            await FileIO.WriteLinesAsync(file, linhas);
        }
    }

}
