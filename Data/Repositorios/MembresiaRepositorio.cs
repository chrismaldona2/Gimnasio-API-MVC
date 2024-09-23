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
    public class MembresiaRepositorio : Repositorio<Membresia>, IRepositorio<Membresia>, IMembresiaRepositorio
    {
        public MembresiaRepositorio(GimnasioContext context) : base(context) { }
        public async Task<Membresia> ObtenerMembresiaConIdAsync(int id)
        {
            return await this._context.Set<Membresia>().Where(m => m.Id == id).FirstOrDefaultAsync();
        }

    }
}
