using GimnasioWebApp.Models;
using static System.Net.WebRequestMethods;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Azure;

namespace WebApp.Services
{
    public class AdminAPIService
    {

        private readonly HttpClient _httpClient;

        public AdminAPIService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://localhost:5229");
        }

        public async Task<string> AutenticarAdmin(IniciarSesionAdminModel adminData)
        {
            
            try
            {
                var response = await _httpClient.PostAsJsonAsync("/api/AdministradorService/Autenticar", adminData);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var jsonObject = JObject.Parse(content);
                    var nombreAdmin = jsonObject["nombre"]?.ToString();
                    return nombreAdmin;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> RegistrarAdmin(RegistrarAdminModel adminData)
        {

            try
            {
                var response = await _httpClient.PostAsJsonAsync("/api/AdministradorService/Registrar", adminData);
                response.EnsureSuccessStatusCode();

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                return false;
                
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}