using Core.Entidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using WebApp.Models.DTOs;
using WebApp.Models.ViewModels;
using WebApp.Services;
using WebApp.Services.Contracts;
using WebApp.ViewPermissions;

namespace WebApp.Controllers
{
    [ValidarSesionAdmin]
    public class AdministradorController : Controller
    {
        private readonly IAdministradorAPIService _administradorService;
        private readonly IClienteAPIService _clienteService;
        private readonly IMembresiaAPIService _membresiaService;

        public AdministradorController(IAdministradorAPIService administradorService, IClienteAPIService clienteService, IMembresiaAPIService membresiaService)
        {
            _administradorService = administradorService;
            _clienteService = clienteService;
            _membresiaService = membresiaService;
        }

        public IActionResult Inicio()
        {
            HttpContext.Session.Clear();
            return View();
        }

        public IActionResult PanelInicio()
        {
            var adminLogueado = HttpContext.Session.GetString("AdminLogueado");
            string nombreAdmin = JsonConvert.DeserializeObject<AdministradoresViewModel>(adminLogueado).Nombre;
            return View("PanelInicio", nombreAdmin);
        }

        public async Task<IActionResult> PanelAdministradores()
        {
            var model = new PanelAdministradoresViewModel
            {
                ListaAdministradores = await _administradorService.ListaAdministradores(),
                AdministradorRegistroDto = new AdministradorRegistroDTO()
            };
            return View(model);
        }

        public async Task<IActionResult> PanelClientes()
        {
            var model = new PanelClientesViewModel
            {
                ListaClientes = await _clienteService.ListaClientes(),
                ClienteRegistroDto = new ClienteRegistroDTO()
            };
            return View(model);
        }

        public async Task<IActionResult> PanelMembresias()
        {
            var model = new PanelMembresiasViewModel
            {
                ListaMembresias = await _membresiaService.ListaMembresias(),
                MembresiaRegistroDto = new MembresiaRegistroDTO()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarAdmin(AdministradorRegistroDTO administradorRegistroDto)
        {
            if (!ModelState.IsValid)
            {
                var model = new PanelAdministradoresViewModel
                {
                    ListaAdministradores = await _administradorService.ListaAdministradores(),
                    AdministradorRegistroDto = administradorRegistroDto 
                };
                return View("PanelAdministradores", model);
            }

            var respuesta = await _administradorService.RegistrarAdminAsync(administradorRegistroDto);
            if (respuesta.Exitoso)
            {
                TempData["SuccessMessage"] = respuesta.Mensaje;
                return RedirectToAction("PanelAdministradores");
            }
            else
            {
                TempData["ErrorMessage"] = respuesta.Mensaje;
                var model = new PanelAdministradoresViewModel
                {
                    ListaAdministradores = await _administradorService.ListaAdministradores(),
                    AdministradorRegistroDto = administradorRegistroDto

                };
                return View("PanelAdministradores", model);
            }
        }


        [HttpPost]
        public async Task<IActionResult> RegistrarCliente(ClienteRegistroDTO clienteRegistroDto)
        {
            if (!ModelState.IsValid)
            {
                var model = new PanelClientesViewModel
                {
                    ListaClientes = await _clienteService.ListaClientes(),
                    ClienteRegistroDto = clienteRegistroDto
                };
                return View("PanelClientes", model);
            }

            var respuesta = await _clienteService.RegistrarClienteAsync(clienteRegistroDto);
            if (respuesta.Exitoso)
            {
                TempData["SuccessMessage"] = respuesta.Mensaje;
                return RedirectToAction("PanelClientes");
            }
            else
            {
                TempData["ErrorMessage"] = respuesta.Mensaje;
                var model = new PanelClientesViewModel
                {
                    ListaClientes = await _clienteService.ListaClientes(),
                    ClienteRegistroDto = clienteRegistroDto
                };
                return View("PanelClientes", model);
            }
        }
    }
}
