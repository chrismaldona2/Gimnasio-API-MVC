using WebApp.Models.Membresias;

namespace WebApp.Services.Contracts
{
    public interface IMembresiaAPIService
    {
        Task<List<MembresiaModel>> ListaMembresias();
        Task<APIResponse> RegistrarMembresiaAsync(MembresiaModel datosMembresia);
        Task<APIResponse> ModificarMembresiaAsync(MembresiaModel datosMembresia);
        Task<APIResponse> EliminarMembresiaAsync(int idMembresia);
    }
}
