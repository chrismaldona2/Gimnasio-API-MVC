using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositorios.Contratos
{
    public interface IPagoRepositorio : IRepositorio<Pago>
    {
        Task<IEnumerable<Pago>> ObtenerPagoPorClienteAsync(int idCliente);

    }
}
