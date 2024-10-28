using WebApp.Models.Cliente;

namespace WebApp.Services.Contracts
{
    public interface IClienteAPIService
    {
        Task<APIResponse> RegistrarClienteAsync(ClienteModel datosCliente);
        Task<APIResponse> ModificarClienteAsync(ClienteModel datosCliente);
        Task<APIResponse> EliminarClienteAsync(int id);
        Task<List<ClienteModel>> ListaClientes();
        Task<ClienteModel> BuscarClientePorDni(string dni);
        Task<ClienteModel> BuscarClientePorId(int id);
        Task<APIResponse> FiltrarClientesPorPropiedad(string propiedad, string prefijo);
    }
}
