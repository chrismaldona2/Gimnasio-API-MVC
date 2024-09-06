using Azure;
using Newtonsoft.Json;
using RestAPI.Models.Entidades;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using WebApp.Models.ViewModels;
using WebApp.Models.DTOs;
using WebApp.Services.Contracts;

namespace WebApp.Services
{

    public class AdministradorAPIService : APIService, IAdministradorAPIService
    {
        public AdministradorAPIService(HttpClient httpClient) : base(httpClient) { }

        public async Task<APIResponse> AutenticarAdminAsync(string usuario, string contraseña)
        {
            var response = await _httpClient.GetAsync($"api/AdministradorService/Autenticar?Usuario={usuario}&Contraseña={contraseña}");

            var responseMessage = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return new APIResponse
                {
                    Exitoso = true,
                    Mensaje = responseMessage
                };
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return new APIResponse
                {
                    Exitoso = false,
                    Mensaje = responseMessage
                };
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                return new APIResponse
                {
                    Exitoso = false,
                    Mensaje = errorMessage
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

        public async Task<AdministradoresViewModel> BuscarAdminAsync(string usuario)
        {
            var response = await _httpClient.GetAsync($"api/AdministradorService/Buscar?Usuario={usuario}");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<AdministradoresViewModel>(jsonResponse);
            }
            else
            {
                return new AdministradoresViewModel
                {
                    Nombre = "Administrador"
                };
            }
        }

        public async Task<List<AdministradoresViewModel>> ListaAdministradores()
        {
            List<AdministradoresViewModel> listaAdministradores = new List<AdministradoresViewModel>();
            var response = await _httpClient.GetAsync($"api/AdministradorService/Lista");

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                listaAdministradores = JsonConvert.DeserializeObject<List<AdministradoresViewModel>>(data);

                return listaAdministradores;
            }
            return listaAdministradores;
        }

        public async Task<APIResponse> RegistrarAdminAsync(AdministradorRegistroDTO datosAdmin)
        {
            var jsonContent = JsonConvert.SerializeObject(datosAdmin);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/AdministradorService/Registrar", content);

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
    }
}
