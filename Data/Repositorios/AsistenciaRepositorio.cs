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
    public class AsistenciaRepositorio : Repositorio<Asistencia>, IRepositorio<Asistencia>, IAsistenciaRepositorio
    {
        public AsistenciaRepositorio(GimnasioContext context) : base(context) { }

        public async Task<IEnumerable<Asistencia>> ObtenerAsistenciaPorClienteAsync(int idCliente)
        {
            return await this._context.Set<Asistencia>().Where(c => c.IdCliente == idCliente).ToListAsync();
        }
    }
}
