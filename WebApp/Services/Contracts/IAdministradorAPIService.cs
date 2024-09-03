using WebApp.Models.Entidades;
using WebApp.Models.ViewModels;

namespace WebApp.Services.Contracts
{
    public interface IAdministradorAPIService
    {
        Task<ApiRespuesta> AutenticarAdminAsync(string usuario, string contraseña);
        Task<AdministradorModel> BuscarAdminAsync(string usuario);
        Task<List<AdministradoresViewModel>> ListaAdministradores();
    }
}
