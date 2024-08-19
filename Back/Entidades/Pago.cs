using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Back.Entidades
{
    public class Pago
    {
        [Key]
        public int Id { get; set; }
        public int IdCliente { get; set; }
        public int IdMembresia { get; set; }
        public DateTime FechaPago { get; set; }
        public double Monto {  get; set; }
        public Pago() { }
        public Pago(int idcliente, int idmembresia, double monto)
        {
            IdCliente = idcliente;
            IdMembresia = idmembresia;
            Monto = monto;
            FechaPago = DateTime.Now;
        }
    }
}
