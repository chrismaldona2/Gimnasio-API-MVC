using Back.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Back.Implementaciones.Contratos
{
    public interface IAdministradorService
    {
        Task<Administrador> AutenticarAdminAsync(string nombreusuario, string contraseña);
        Task RegistrarAdminAsync(string nombreusuario, string contraseña, string dni, string nombre, string apellido, DateOnly nacimiento, Sexo sexo);
        Task EliminarAdminAsync(int idAdminEliminar);
        Task ModificarAdminAsync(Administrador adminModificar);
    }
}
