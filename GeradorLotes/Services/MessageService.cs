using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeradorLotes.Services
{
    public class MessageService
    {
        public static MessageService Instance { get; } = new MessageService();

        public event Action<string, string, MessageSeverity>? MessageRequested;

        public void Show(string title, string message, MessageSeverity severity = MessageSeverity.Info)
        {
            MessageRequested?.Invoke(title, message, severity);
        }

        public enum MessageSeverity
        {
            Info, 
            Success,
            Warning,
            Error
        }
    }
}
