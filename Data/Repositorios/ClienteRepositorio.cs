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
    public class ClienteRepositorio : Repositorio<Cliente>, IRepositorio<Cliente>, IClienteRepositorio
    {
        public ClienteRepositorio(GimnasioContext context) : base(context) { }

        public async Task<IEnumerable<Cliente>> ObtenerClientesConMembresiaVencidaAsync()
        {
            return await this._context.Set<Cliente>().Where(c => c.FechaVencimientoMembresia <= DateTime.Now).ToListAsync();
        }

        public async Task<IEnumerable<Cliente>> ObtenerClientesConTipoMembresiaAsync(int idMembresia)
        {
            return await this._context.Set<Cliente>().Where(c => c.IdMembresia == idMembresia).ToListAsync();
        }

        public async Task<Cliente> ObtenerClienteConIdAsync(int id)
        {
            return await this._context.Set<Cliente>().Where(c => c.Id == id).FirstOrDefaultAsync();
        }
        public async Task<Cliente> ObtenerClienteConDniAsync(string dni)
        {
            return await this._context.Set<Cliente>().Where(c => c.Dni == dni).FirstOrDefaultAsync();
        }
    }

}
