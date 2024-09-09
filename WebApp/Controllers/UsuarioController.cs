using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApp.Models.DTOs;
using WebApp.Models.ViewModels;
using WebApp.Services.Contracts;

namespace WebApp.Controllers
{
    public class UsuarioController : Controller
    {

        private readonly IAdministradorAPIService _administradorApiService;
        private readonly IClienteAPIService _clienteService;

        public UsuarioController(IAdministradorAPIService administradorApiService, IClienteAPIService clienteService)
        {
            _administradorApiService = administradorApiService;
            _clienteService = clienteService;
        }

        public IActionResult LoginCliente()
        {
            HttpContext.Session.Clear();
            ClientesViewModel? model = null;
            return View(model);
        }

        public IActionResult LoginAdmin()
        {
            HttpContext.Session.Clear();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AutenticarAdmin(AdministradorLoginDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View("LoginAdmin", model);
            }
            var respuesta = await _administradorApiService.AutenticarAdminAsync(model.Usuario, model.Contraseña);

            if (respuesta.Exitoso)
            {
                TempData["SuccessMessage"] = $"{respuesta.Mensaje}";

                var adminLoguado = await _administradorApiService.BuscarAdminAsync(model.Usuario);

                HttpContext.Session.SetString("AdminLogueado", JsonConvert.SerializeObject(adminLoguado));

                return RedirectToAction("PanelInicio", "Administrador");
            }
            else
            {
                TempData["ErrorMessage"] = $"{respuesta.Mensaje}";
                return View("LoginAdmin", model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> VerInformacionCliente(string dniInput)
        {
            var respuesta = await _clienteService.BuscarClientePorDni(dniInput);

            if (respuesta.Exitoso)
            {
                var model = JsonConvert.DeserializeObject<ClientesViewModel>(respuesta.Mensaje);
                return View("LoginCliente", model);
            }
            else
            {
                TempData["ErrorMessage"] = $"{respuesta.Mensaje}";
                return RedirectToAction("LoginCliente");
            }
        }
    }
}
