using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entidades
{
    public class Asistencia
    {
        public int Id { get; set; }
        public int IdCliente { get; set; }
        public DateTime FechaAsistencia { get; set; }
        public Asistencia() { }
        public Asistencia(int idCliente, DateTime fechaAsistencia)
        {
            IdCliente = idCliente;
            FechaAsistencia = fechaAsistencia;
        }
    }
}
