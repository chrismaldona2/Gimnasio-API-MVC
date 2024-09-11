using WebApp.Models.DTOs;
using WebApp.Models.ViewModels;

namespace WebApp.Services.Contracts
{
    public interface IAdministradorAPIService
    {
        Task<APIResponse> AutenticarAdminAsync(string usuario, string contraseña);
        Task<AdministradoresViewModel> BuscarAdminPorUsuarioAsync(string usuario);
        Task<AdministradoresViewModel> BuscarAdminPorDniAsync(string dni);
        Task<List<AdministradoresViewModel>> ListaAdministradores();
        Task<APIResponse> RegistrarAdminAsync(AdministradorRegistroDTO datosAdmin);

    }
}
