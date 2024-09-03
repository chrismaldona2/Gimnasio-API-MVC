using Core.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositorios.Contratos
{
    public interface IAdministradorRepositorio : IRepositorio<Administrador>
    {
        Task<Administrador> ObtenerAdminConUsuarioAsync(string usuario);
        Task<Administrador> ObtenerAdminConDniAsync(string dni);
    }
}
