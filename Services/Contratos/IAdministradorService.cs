using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entidades;

namespace Services.Contratos
{
    public interface IAdministradorService
    {
        Task RegistrarAdminAsync(string usuario, string contraseña, string dni, string nombre, string apellido, string email, string telefono, DateOnly fechanacimiento, Sexo sexo);
        Task EliminarAdminAsync(int idAdminEliminar);
        Task ModificarAdminAsync(Administrador adminModificar);
        Task<Administrador> AutenticarAdminAsync(string usuario, string contraseña);
        Task<IEnumerable<Administrador>> ObtenerAdministradoresAsync();
        Task<Administrador> BuscarAdminPorUsuarioAsync(string usuario);
    }
}
