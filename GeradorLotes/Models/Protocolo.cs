using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeradorLotes.Models
{
    public class Protocolo
    {
        public string Numero { get; set; }

        public Protocolo(string numero)
        {
            Numero = numero;
        }

        public string ToSring() => Numero;
    }
}
