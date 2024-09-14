using WebApp.Models.Cliente;

namespace WebApp.Services.Contracts
{
    public interface IClienteAPIService
    {
        Task<APIResponse> RegistrarClienteAsync(ClienteModel datosCliente);
        Task<APIResponse> ModificarClienteAsync(ClienteModel datosCliente);
        Task<APIResponse> EliminarClienteAsync(int id);
        Task<List<ClienteModel>> ListaClientes();
        Task<APIResponse> BuscarClientePorDni(string dni);
    }
}
