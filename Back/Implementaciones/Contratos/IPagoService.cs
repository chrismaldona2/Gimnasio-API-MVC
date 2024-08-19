using Back.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Back.Implementaciones.Contratos
{
    public interface IPagoService
    {
        Task RegistrarPagoAsync(int idCliente, int idMembresia, bool actualizarVencimientoCliente);
        Task<IEnumerable<Pago>> ObtenerPagosPorClienteAsync(int idCliente);
        Task EliminarPagoAsync(int id);
    }
}
