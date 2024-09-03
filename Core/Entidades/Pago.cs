using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entidades
{
    public class Pago
    {
        public int Id { get; set; }
        public int IdCliente { get; set; }
        public int IdMembresia { get; set; }
        public DateTime FechaPago { get; set; }
        public double Monto { get; set; }
        public Pago() { }
        public Pago(int idCliente, int idMembresia, double monto)
        {
            IdCliente = idCliente;
            IdMembresia = idMembresia;
            Monto = monto;
            FechaPago = DateTime.Now;
        }
    }
}
