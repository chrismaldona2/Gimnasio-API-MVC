using Azure;
using Newtonsoft.Json;
using RestAPI.Models.Entidades;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using WebApp.Models.Administrador;
using WebApp.Models.Cliente;
using WebApp.Models.Membresias;
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
            else
            {
                return new APIResponse
                {
                    Exitoso = false,
                    Mensaje = responseMessage
                };
            }
        }

        public async Task<AdminModel> BuscarAdminPorUsuarioAsync(string usuario)
        {
            var response = await _httpClient.GetAsync($"api/AdministradorService/BuscarUsuario?Usuario={usuario}");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<AdminModel>(jsonResponse);
            }
            return null;
        }

        public async Task<AdminModel> BuscarAdminPorDniAsync(string dni)
        {
            var response = await _httpClient.GetAsync($"api/AdministradorService/BuscarDNI?Dni={dni}");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<AdminModel>(jsonResponse);
            }
            return null;
        }

        public async Task<List<AdminModel>> ListaAdministradores()
        {
            List<AdminModel> listaAdministradores = new();
            var response = await _httpClient.GetAsync($"api/AdministradorService/Lista");

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                listaAdministradores = JsonConvert.DeserializeObject<List<AdminModel>>(data);

                return listaAdministradores;
            }
            return listaAdministradores;
        }

        public async Task<APIResponse> RegistrarAdminAsync(AdminModel datosAdmin)
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
            else
            {
                return new APIResponse
                {
                    Exitoso = false,
                    Mensaje = responseMessage
                };
            }
        }

        public async Task<APIResponse> ModificarAdminAsync(AdminModel datosAdmin)
        {

            var jsonContent = JsonConvert.SerializeObject(datosAdmin);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/AdministradorService/Modificar?Id={datosAdmin.Id}", content);
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


        public async Task<APIResponse> EliminarAdminAsync(int id)
        {

            var response = await _httpClient.DeleteAsync($"api/AdministradorService/Eliminar?Id={id}");
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
    }
}
