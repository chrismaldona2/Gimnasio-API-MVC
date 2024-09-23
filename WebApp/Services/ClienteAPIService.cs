using Newtonsoft.Json;
using System.Net;
using System.Text;
using WebApp.Models.Administrador;
using WebApp.Models.Cliente;
using WebApp.Services.Contracts;

namespace WebApp.Services
{
    public class ClienteAPIService : APIService, IClienteAPIService
    {
        public ClienteAPIService(HttpClient httpClient) : base(httpClient) { }

        public async Task<APIResponse> RegistrarClienteAsync(ClienteModel datosCliente)
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
            else
            {
                return new APIResponse
                {
                    Exitoso = false,
                    Mensaje = responseMessage
                };
            }
        }

        public async Task<APIResponse> ModificarClienteAsync(ClienteModel datosCliente)
        {

            var jsonContent = JsonConvert.SerializeObject(datosCliente);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/ClienteService/Modificar?Id={datosCliente.Id}", content);
            var responseMessage = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return new APIResponse
                {
                    Exitoso = true,
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


        public async Task<APIResponse> EliminarClienteAsync(int id)
        {

            var response = await _httpClient.DeleteAsync($"api/ClienteService/Eliminar?Id={id}");
            var responseMessage = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return new APIResponse
                {
                    Exitoso = true,
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

        public async Task<List<ClienteModel>> ListaClientes()
        {
            List<ClienteModel> listaClientes = new();
            var response = await _httpClient.GetAsync($"api/ClienteService/Lista");

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                listaClientes = JsonConvert.DeserializeObject<List<ClienteModel>>(data);

                return listaClientes;
            }
            return listaClientes;
        }


        public async Task<ClienteModel> BuscarClientePorDni(string dni)
        { 
            var response = await _httpClient.GetAsync($"api/ClienteService/BuscarDNI?Dni={dni}");

            var responseMessage = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ClienteModel>(jsonResponse);
            }
            return null;

        }

        public async Task<ClienteModel> BuscarClientePorId(int id)
        {

            var response = await _httpClient.GetAsync($"api/ClienteService/BuscarID?Id={id}");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ClienteModel>(jsonResponse);
            }
            return null;

        }


        
    }
}
