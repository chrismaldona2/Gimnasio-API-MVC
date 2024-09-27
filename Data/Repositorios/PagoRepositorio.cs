using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Repositorios.Contratos;
using Data.Contexts;
using Core.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositorios
{
    public class PagoRepositorio : Repositorio<Pago>, IRepositorio<Pago>, IPagoRepositorio
    {
        public PagoRepositorio(GimnasioContext context) : base(context) { }

        public async Task<IEnumerable<Pago>> ObtenerPagoPorClienteAsync(int idCliente)
        {
            return await this._context.Set<Pago>().Where(c => c.IdCliente == idCliente).ToListAsync();
        }

    }
}
