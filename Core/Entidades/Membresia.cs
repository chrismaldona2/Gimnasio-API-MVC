using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entidades
{
    public class Membresia
    {
        public int Id { get; set; }
        public string Tipo { get; set; }
        public int DuracionDias { get; set; }
        public double Precio { get; set; }
        public Membresia() { }
        public Membresia(string tipo, int duracionDias, double precio)
        {
            Tipo = tipo;
            DuracionDias = duracionDias;
            Precio = precio;
        }
    }
}
