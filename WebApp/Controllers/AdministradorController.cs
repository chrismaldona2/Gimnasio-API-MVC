using Core.Entidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using WebApp.Models.ViewModels;
using WebApp.Services;
using WebApp.Services.Contracts;
using WebApp.ViewPermissions;

namespace WebApp.Controllers
{
    [ValidarSesionAdmin]
    public class AdministradorController : Controller
    {
        private readonly IAdministradorAPIService _administradorApiService;

        public AdministradorController(IAdministradorAPIService administradorApiService)
        {
            _administradorApiService = administradorApiService;
        }

        public IActionResult Inicio()
        {
            HttpContext.Session.Clear();
            return View();
        }

        public IActionResult PanelInicio()
        {
            var adminLogueado = HttpContext.Session.GetString("AdminLogueado");
            string nombreAdmin = JsonConvert.DeserializeObject<Administrador>(adminLogueado).Nombre;
            return View("PanelInicio", nombreAdmin);
        }

        public async Task<IActionResult> PanelAdministradores()
        {
            var listaAdministradores = await _administradorApiService.ListaAdministradores();
            return View(listaAdministradores);
        }
    }
}
