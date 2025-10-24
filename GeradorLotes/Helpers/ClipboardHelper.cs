using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;

namespace GeradorLotes.Helpers
{
    public static class ClipboardHelper
    {
        public static async Task<string?> ObterTextoAsync()
        {
            try
            {
                var content = Clipboard.GetContent();
                if (content.Contains(StandardDataFormats.Text))
                {
                    return await content.GetTextAsync();
                }
            }
            catch (Exception ex)
            {
                // Log ou tratamento de erro, se necessário
                System.Diagnostics.Debug.WriteLine($"Erro ao acessar a área de transferência: {ex.Message}");
            }

            return null;
        }
    }

}
