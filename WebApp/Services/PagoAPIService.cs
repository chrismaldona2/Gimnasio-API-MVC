using Newtonsoft.Json;
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

    }
}
