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
    public class AdministradorRepositorio : Repositorio<Administrador>, IRepositorio<Administrador>, IAdministradorRepositorio
    {
        public AdministradorRepositorio(GimnasioContext context) : base(context) { }

        public async Task<Administrador> ObtenerAdminConIdAsync(int id)
        {
            return await this._context.Set<Administrador>().Where(c => c.Id == id).FirstOrDefaultAsync();
        }
        public async Task<Administrador> ObtenerAdminConUsuarioAsync(string usuario)
        {
            return await this._context.Set<Administrador>().Where(c => c.Usuario == usuario).FirstOrDefaultAsync();
        }

        public async Task<Administrador> ObtenerAdminConDniAsync(string dni)
        {
            return await this._context.Set<Administrador>().Where(c => c.Dni == dni).FirstOrDefaultAsync();
        }
    }
}
