using WebApp.Models.DTOs;
using WebApp.Models.ViewModels;

namespace WebApp.Services.Contracts
{
    public interface IClienteAPIService
    {
        Task<APIResponse> RegistrarClienteAsync(ClienteRegistroDTO datosCliente);
        Task<List<ClientesViewModel>> ListaClientes();
    }
}
