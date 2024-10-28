using WebApp.Models.Administrador;

namespace WebApp.Services.Contracts
{
    public interface IAdministradorAPIService
    {
        Task<APIResponse> AutenticarAdminAsync(string usuario, string contraseña);
        Task<AdminModel> BuscarAdminPorUsuarioAsync(string usuario);
        Task<AdminModel> BuscarAdminPorDniAsync(string dni);
        Task<AdminModel> BuscarAdminPorIdAsync(int id);
        Task<List<AdminModel>> ListaAdministradores();
        Task<APIResponse> RegistrarAdminAsync(AdminModel datosAdmin);
        Task<APIResponse> ModificarAdminAsync(AdminModel datosAdmin);
        Task<APIResponse> EliminarAdminAsync(int id);
        Task<APIResponse> FiltrarAdministradoresPorPropiedad(string propiedad, string prefijo);

    }
}
