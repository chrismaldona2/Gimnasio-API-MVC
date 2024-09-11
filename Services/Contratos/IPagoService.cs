using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entidades;

namespace Services.Contratos
{
    public interface IPagoService
    {
        Task RegistrarPagoAsync(int idCliente, int idMembresia);
        Task EliminarPagoAsync(int idPago);
        Task<IEnumerable<Pago>> ObtenerPagosAsync();
        Task<IEnumerable<Pago>> ObtenerPagosPorIdClienteAsync(int id);  
        Task<IEnumerable<Pago>> ObtenerPagosPorDniClienteAsync(string dni);
    }
}
