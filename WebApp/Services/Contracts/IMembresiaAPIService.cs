using WebApp.Models.DTOs;
using WebApp.Models.ViewModels;

namespace WebApp.Services.Contracts
{
    public interface IMembresiaAPIService
    {
        Task<APIResponse> RegistrarMembresiaAsync(MembresiaRegistroDTO datosCliente);
        Task<List<MembresiasViewModel>> ListaMembresias();
    }
}
