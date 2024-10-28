using Newtonsoft.Json;
using System.Text;
using WebApp.Models.Cliente;
using WebApp.Models.Membresia;
using WebApp.Services.Contracts;

namespace WebApp.Services
{
    public class MembresiaAPIService : APIService, IMembresiaAPIService
    {
        public MembresiaAPIService(HttpClient httpClient) : base(httpClient) { }


        public async Task<List<MembresiaModel>> ListaMembresias()
        {
            List<MembresiaModel> listaMembresias = new();
            var response = await _httpClient.GetAsync($"api/MembresiaService/Lista");

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                listaMembresias = JsonConvert.DeserializeObject<List<MembresiaModel>>(data);

                return listaMembresias;
            }
            return listaMembresias;
        }


        public async Task<APIResponse> RegistrarMembresiaAsync(MembresiaModel datosMembresia)
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


        public async Task<APIResponse> ModificarMembresiaAsync(MembresiaModel datosMembresia)
        {

            var jsonContent = JsonConvert.SerializeObject(datosMembresia);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/MembresiaService/Modificar?Id={datosMembresia.Id}", content);
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



        public async Task<APIResponse> EliminarMembresiaAsync(int id)
        {

            var response = await _httpClient.DeleteAsync($"api/MembresiaService/Eliminar?Id={id}");
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

        public async Task<MembresiaModel> BuscarMembresiaPorId(int id)
        {

            var response = await _httpClient.GetAsync($"api/MembresiaService/BuscarID?Id={id}");
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<MembresiaModel>(jsonResponse);
            }
            return null;

        }



        public async Task<APIResponse> FiltrarMembresiasPorPropiedad(string propiedad, string prefijo)
        {
            string encodedPrefijo = Uri.EscapeDataString(prefijo);
            var response = await _httpClient.GetAsync($"api/MembresiaService/FiltrarMembresiasPorPropiedad?propiedad={propiedad}&prefijo={encodedPrefijo}");
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
