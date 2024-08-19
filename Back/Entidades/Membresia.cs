using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Back.Entidades
{
    public class Membresia
    {
        [Key]
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public double DuracionEnMeses { get; set; }
        public int DuracionEnDias => (int)(DuracionEnMeses * 30);
        public double Precio { get; set; }
        public Membresia() { }
        public Membresia(string descripcion, double duracionmeses, double precio)
        {
            Descripcion = descripcion;
            DuracionEnMeses = duracionmeses;
            Precio = precio;
        }
    }
}
