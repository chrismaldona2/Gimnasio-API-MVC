using Azure;
using Newtonsoft.Json;
using RestAPI.Models.Entidades;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using WebApp.Models.ViewModels;
using WebApp.Services.Contracts;

namespace WebApp.Services
{
    public class ApiRespuesta
    {
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; }
    }
    public class AdministradorAPIService : APIService, IAdministradorAPIService
    {
        public AdministradorAPIService(HttpClient httpClient) : base(httpClient) { }

        public async Task<ApiRespuesta> AutenticarAdminAsync(string usuario, string contraseña)
        {
            var response = await _httpClient.GetAsync($"api/AdministradorService/Autenticar?Usuario={usuario}&Contraseña={contraseña}");

            if (response.IsSuccessStatusCode)
            {
                return new ApiRespuesta
                {
                    Exitoso = true,
                    Mensaje = "Inicio de sesión exitoso."
                };
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new ApiRespuesta
                {
                    Exitoso = false,
                    Mensaje = "Nombre de usuario o contraseña incorrectos."
                };
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                return new ApiRespuesta
                {
                    Exitoso = false,
                    Mensaje = errorMessage
                };
            }
            else
            {
                return new ApiRespuesta
                {
                    Exitoso = false,
                    Mensaje = "Error inesperado del servidor."
                };
            }
        }

        public async Task<Models.Entidades.AdministradorModel> BuscarAdminAsync(string usuario)
        {
            var response = await _httpClient.GetAsync($"api/AdministradorService/Buscar?Usuario={usuario}");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Models.Entidades.AdministradorModel>(jsonResponse);
            }
            else
            {
                return new Models.Entidades.AdministradorModel
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
            else
            {
                return listaAdministradores;
            }
        }
    }
}
