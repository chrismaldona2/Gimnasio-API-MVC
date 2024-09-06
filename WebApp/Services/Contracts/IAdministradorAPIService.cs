using WebApp.Models.DTOs;
using WebApp.Models.ViewModels;

namespace WebApp.Services.Contracts
{
    public interface IAdministradorAPIService
    {
        Task<APIResponse> AutenticarAdminAsync(string usuario, string contraseña);
        Task<AdministradoresViewModel> BuscarAdminAsync(string usuario);
        Task<List<AdministradoresViewModel>> ListaAdministradores();

        Task<APIResponse> RegistrarAdminAsync(AdministradorRegistroDTO datosAdmin);
    }
}
