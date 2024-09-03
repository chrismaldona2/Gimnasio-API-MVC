using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApp.Models.ViewModels;
using WebApp.Services.Contracts;

namespace WebApp.Controllers
{
    public class UsuarioController : Controller
    {

        private readonly IAdministradorAPIService _administradorApiService;

        public UsuarioController(IAdministradorAPIService administradorApiService)
        {
            _administradorApiService = administradorApiService;
        }

        public IActionResult LoginCliente()
        {
            HttpContext.Session.Clear();
            return View();
        }

        public IActionResult LoginAdmin()
        {
            HttpContext.Session.Clear();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AutenticarAdmin(AdministradorLoginModel model)
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
    }
}
