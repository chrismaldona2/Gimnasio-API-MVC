using WebApp.Models.Membresia;

namespace WebApp.Services.Contracts
{
    public interface IMembresiaAPIService
    {
        Task<List<MembresiaModel>> ListaMembresias();
        Task<MembresiaModel> BuscarMembresiaPorId(int id);
        Task<APIResponse> RegistrarMembresiaAsync(MembresiaModel datosMembresia);
        Task<APIResponse> ModificarMembresiaAsync(MembresiaModel datosMembresia);
        Task<APIResponse> EliminarMembresiaAsync(int idMembresia);
    }
}
