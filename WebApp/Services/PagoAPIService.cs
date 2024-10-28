using Core.Entidades;
using Newtonsoft.Json;
using System.Text;
using WebApp.Models.Membresia;
using WebApp.Models.Pago;
using WebApp.Services.Contracts;

namespace WebApp.Services
{
    public class PagoAPIService : APIService, IPagoAPIService
    {
        public PagoAPIService(HttpClient httpClient) : base(httpClient) { }

        public async Task<List<PagoModel>> ListaPagos()
        {
            List<PagoModel> listaPagos = new();
            var response = await _httpClient.GetAsync($"api/PagoService/Lista");

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                listaPagos = JsonConvert.DeserializeObject<List<PagoModel>>(data);

                return listaPagos;
            }
            return listaPagos;
        }


        public async Task<APIResponse> RegistrarPagoAsync(PagoModel datosPago)
        {
            var response = await _httpClient.PostAsync($"api/PagoService/Registrar?IdCliente={datosPago.IdCliente}&IdMembresia={datosPago.IdMembresia}", null);
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

        public async Task<APIResponse> EliminarPagoAsync(int id)
        {

            var response = await _httpClient.DeleteAsync($"api/PagoService/Eliminar?Id={id}");
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


        public async Task<PagoModel> BuscarPagoPorId(int id)
        {

            var response = await _httpClient.GetAsync($"api/PagoService/BuscarID?Id={id}");
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<PagoModel>(jsonResponse);
            }
            return null;

        }


        public async Task<List<PagoModel>> ListaPagosCliente(int idCliente)
        {
            List<PagoModel> listaPagos = new();
            var response = await _httpClient.GetAsync($"api/PagoService/PagosClientePorId?IdCliente={idCliente}");

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                listaPagos = JsonConvert.DeserializeObject<List<PagoModel>>(data);

                return listaPagos;
            }
            return listaPagos;
        }


        public async Task<APIResponse> FiltrarPagosPorPropiedad(string propiedad, string prefijo)
        {
            string encodedPrefijo = Uri.EscapeDataString(prefijo);
            var response = await _httpClient.GetAsync($"api/PagoService/FiltrarPagosPorPropiedad?propiedad={propiedad}&prefijo={encodedPrefijo}");
            string responseMessage = await response.Content.ReadAsStringAsync();

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
    }
}
