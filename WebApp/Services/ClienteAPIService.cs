using Newtonsoft.Json;
using System.Net;
using System.Text;
using WebApp.Models.DTOs;
using WebApp.Models.ViewModels;
using WebApp.Services.Contracts;

namespace WebApp.Services
{
    public class ClienteAPIService : APIService, IClienteAPIService
    {
        public ClienteAPIService(HttpClient httpClient) : base(httpClient) { }

        public async Task<APIResponse> RegistrarClienteAsync(ClienteRegistroDTO datosCliente)
        {
            var jsonContent = JsonConvert.SerializeObject(datosCliente);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/ClienteService/Registrar", content);

            var responseMessage = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return new APIResponse
                {
                    Exitoso = true,
                    Mensaje = responseMessage
                };
            }
            else if (response.StatusCode == HttpStatusCode.Conflict)
            {
                return new APIResponse
                {
                    Exitoso = false,
                    Mensaje = responseMessage
                };
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                return new APIResponse
                {
                    Exitoso = false,
                    Mensaje = responseMessage
                };
            }
            else
            {
                return new APIResponse
                {
                    Exitoso = false,
                    Mensaje = responseMessage
                };
            }
        }

        public async Task<List<ClientesViewModel>> ListaClientes()
        {
            List<ClientesViewModel> listaClientes = new List<ClientesViewModel>();
            var response = await _httpClient.GetAsync($"api/ClienteService/Lista");

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                listaClientes = JsonConvert.DeserializeObject<List<ClientesViewModel>>(data);

                return listaClientes;
            }
            return listaClientes;
        }
    }
}
