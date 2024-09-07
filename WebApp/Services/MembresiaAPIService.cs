using Newtonsoft.Json;
using System.Text;
using WebApp.Models.DTOs;
using WebApp.Models.ViewModels;
using WebApp.Services.Contracts;

namespace WebApp.Services
{
    public class MembresiaAPIService : APIService, IMembresiaAPIService
    {
        public MembresiaAPIService(HttpClient httpClient) : base(httpClient) { }

        public async Task<APIResponse> RegistrarMembresiaAsync(MembresiaRegistroDTO datosMembresia)
        {
            var jsonContent = JsonConvert.SerializeObject(datosMembresia);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/MembresiaService/Registrar", content);

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

        public async Task<List<MembresiasViewModel>> ListaMembresias()
        {
            List<MembresiasViewModel> listaMembresias = new List<MembresiasViewModel>();
            var response = await _httpClient.GetAsync($"api/MembresiaService/Lista");

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                listaMembresias = JsonConvert.DeserializeObject<List<MembresiasViewModel>>(data);

                return listaMembresias;
            }
            return listaMembresias;
        }
    }
}
