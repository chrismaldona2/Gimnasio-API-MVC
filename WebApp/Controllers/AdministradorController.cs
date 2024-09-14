using Core.Entidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using WebApp.Models;
using WebApp.Models.Administrador;
using WebApp.Models.Cliente;
using WebApp.Models.Membresias;
using WebApp.Models.Pago;
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
        private readonly IPagoAPIService _pagoService;
        public AdministradorController(IAdministradorAPIService administradorService, IClienteAPIService clienteService, IMembresiaAPIService membresiaService, IPagoAPIService pagoService)
        {
            _administradorService = administradorService;
            _clienteService = clienteService;
            _membresiaService = membresiaService;
            _pagoService = pagoService;
        }



        //PANTALLA DE INICIO
        public IActionResult PanelInicio()
        {
            var adminLogueado = HttpContext.Session.GetString("AdminLogueado");
            string nombreAdmin = JsonConvert.DeserializeObject<AdminModel>(adminLogueado).Nombre;
            return View("PanelInicio", nombreAdmin);
        }





        //PANTALLA DE ADMINISTRADORES
        public async Task<IActionResult> PanelAdministradores()
        {
            var listaAdministradores = await _administradorService.ListaAdministradores();

            PanelAdminModel model = new()
            {
                ListaAdministradores = listaAdministradores != null ? listaAdministradores : new List<AdminModel>()
            };
            return View("PanelAdministradores", model);
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarAdmin(PanelAdminModel model)
        {
            if (ModelState.IsValid)
            {

                AdminModel data = new()
                {
                    Usuario = model.Usuario,
                    Contraseña = model.Contraseña,
                    Dni = model.Dni,
                    Nombre = model.Nombre,
                    Apellido = model.Apellido,
                    Telefono = model.Telefono,
                    Email = model.Email,
                    FechaNacimientoDTO = model.FechaNacimientoDTO,
                    Sexo = model.Sexo
                };
                var respuesta = await _administradorService.RegistrarAdminAsync(data);

                if (respuesta.Exitoso)
                {
                    TempData["SuccessMessage"] = respuesta.Mensaje;
                    return RedirectToAction("PanelAdministradores");
                }
                else
                {
                    TempData["ErrorMessage"] = respuesta.Mensaje;
                    return RedirectToAction("PanelAdministradores");
                }
            }
            TempData["ErrorMessage"] = "Error inesperado.";
            return RedirectToAction("PanelAdministradores");
        }

        [HttpPost]
        public async Task<IActionResult> ModificarAdmin(PanelAdminModel model)
        {
            if (ModelState.IsValid)
            {
                AdminModel data = new()
                {
                    Id = model.Id,
                    Usuario = model.Usuario,
                    Contraseña = model.Contraseña,
                    Dni = model.Dni,
                    Nombre = model.Nombre,
                    Apellido = model.Apellido,
                    Telefono = model.Telefono,
                    Email = model.Email,
                    FechaNacimientoDTO = model.FechaNacimientoDTO,
                    Sexo = model.Sexo
                };
                var respuesta = await _administradorService.ModificarAdminAsync(data);

                if (respuesta.Exitoso)
                {
                    TempData["SuccessMessage"] = respuesta.Mensaje;
                    return RedirectToAction("PanelAdministradores");
                }
                else
                {
                    TempData["ErrorMessage"] = respuesta.Mensaje;
                    return RedirectToAction("PanelAdministradores");
                }
            }
            TempData["ErrorMessage"] = "Error inesperado.";
            return RedirectToAction("PanelAdministradores");
        }


        [HttpPost]
        public async Task<IActionResult> EliminarAdmin(PanelAdminModel model)
        {
            if (ModelState.IsValid)
            {
                var respuesta = await _administradorService.EliminarAdminAsync(model.Id);

                if (respuesta.Exitoso)
                {
                    TempData["SuccessMessage"] = respuesta.Mensaje;
                    return RedirectToAction("PanelAdministradores");
                }
                else
                {
                    TempData["ErrorMessage"] = respuesta.Mensaje;
                    return RedirectToAction("PanelAdministradores");
                }
            }
            TempData["ErrorMessage"] = "Error inesperado.";
            return RedirectToAction("PanelAdministradores");
        }

        

        //PANTALLA DE MEMBRESIAS
        public async Task<IActionResult> PanelMembresias()
        {

            var listaMembresias = await _membresiaService.ListaMembresias();
            PanelMembresiasModel model = new()
            {
                ListaMembresias = listaMembresias != null ? listaMembresias : new List<MembresiaModel>()
            };
            return View("PanelMembresias", model);
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarMembresia(PanelMembresiasModel model)
        {
            if (ModelState.IsValid)
            {

                MembresiaModel data = new()
                {
                    Tipo = model.Tipo,
                    DuracionDias = model.DuracionDias,
                    Precio = model.Precio,
                };
                var respuesta = await _membresiaService.RegistrarMembresiaAsync(data);

                if (respuesta.Exitoso)
                {
                    TempData["SuccessMessage"] = respuesta.Mensaje;
                    return RedirectToAction("PanelMembresias");
                }
                else
                {
                    TempData["ErrorMessage"] = respuesta.Mensaje;
                    return RedirectToAction("PanelMembresias");
                }
            }
            TempData["ErrorMessage"] = "Error inesperado.";
            return RedirectToAction("PanelMembresias");
        }

        [HttpPost]
        public async Task<IActionResult> ModificarMembresia(PanelMembresiasModel model)
        {
            if (ModelState.IsValid)
            {
                MembresiaModel data = new()
                {
                    Id = model.Id,
                    Tipo = model.Tipo,
                    DuracionDias = model.DuracionDias,
                    Precio = model.Precio,
                };
                var respuesta = await _membresiaService.ModificarMembresiaAsync(data);

                if (respuesta.Exitoso)
                {
                    TempData["SuccessMessage"] = respuesta.Mensaje;
                    return RedirectToAction("PanelMembresias");
                }
                else
                {
                    TempData["ErrorMessage"] = respuesta.Mensaje;
                    return RedirectToAction("PanelMembresias");
                }
            }
            TempData["ErrorMessage"] = "Error inesperado.";
            return RedirectToAction("PanelMembresias");
        }


        [HttpPost]
        public async Task<IActionResult> EliminarMembresia(PanelMembresiasModel model)
        {
            if (ModelState.IsValid)
            {
                var respuesta = await _membresiaService.EliminarMembresiaAsync(model.Id);

                if (respuesta.Exitoso)
                {
                    TempData["SuccessMessage"] = respuesta.Mensaje;
                    return RedirectToAction("PanelMembresias");
                }
                else
                {
                    TempData["ErrorMessage"] = respuesta.Mensaje;
                    return RedirectToAction("PanelMembresias");
                }
            }
            TempData["ErrorMessage"] = "Error inesperado.";
            return RedirectToAction("PanelMembresias");
        }





        //PANEL DE CLIENTES
        public async Task<IActionResult> PanelClientes()
        {
            var listaClientes = await _clienteService.ListaClientes();

            PanelClientesModel model = new()
            {
                ListaClientes = listaClientes != null ? listaClientes : new List<ClienteModel>()
            };
            return View("PanelClientes", model);
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarCliente(PanelClientesModel model)
        {
            if (ModelState.IsValid)
            {
                ClienteModel data = new()
                {
                    Dni = model.Dni,
                    Nombre = model.Nombre,
                    Apellido = model.Apellido,
                    Telefono = model.Telefono,
                    Email = model.Email,
                    FechaNacimientoDTO = model.FechaNacimientoDTO,
                    Sexo = model.Sexo
                };
                var respuesta = await _clienteService.RegistrarClienteAsync(data);

                if (respuesta.Exitoso)
                {
                    TempData["SuccessMessage"] = respuesta.Mensaje;
                    return RedirectToAction("PanelClientes");
                }
                else
                {
                    TempData["ErrorMessage"] = respuesta.Mensaje;
                    return RedirectToAction("PanelClientes");
                }
            }
            TempData["ErrorMessage"] = "Error inesperado.";
            return RedirectToAction("PanelClientes");
        }


        [HttpPost]
        public async Task<IActionResult> ModificarCliente(PanelClientesModel model)
        {
            if (ModelState.IsValid)
            {
                ClienteModel data = new()
                {
                    Id = model.Id,
                    Dni = model.Dni,
                    Nombre = model.Nombre,
                    Apellido = model.Apellido,
                    Telefono = model.Telefono,
                    Email = model.Email,
                    FechaNacimientoDTO = model.FechaNacimientoDTO,
                    Sexo = model.Sexo
                };
                var respuesta = await _clienteService.ModificarClienteAsync(data);

                if (respuesta.Exitoso)
                {
                    TempData["SuccessMessage"] = respuesta.Mensaje;
                    return RedirectToAction("PanelClientes");
                }
                else
                {
                    TempData["ErrorMessage"] = respuesta.Mensaje;
                    return RedirectToAction("PanelClientes");
                }
            }
            TempData["ErrorMessage"] = "Error inesperado.";
            return RedirectToAction("PanelClientes");
        }


        [HttpPost]
        public async Task<IActionResult> EliminarCliente(PanelClientesModel model)
        {
            if (ModelState.IsValid)
            {
                var respuesta = await _clienteService.EliminarClienteAsync(model.Id);

                if (respuesta.Exitoso)
                {
                    TempData["SuccessMessage"] = respuesta.Mensaje;
                    return RedirectToAction("PanelClientes");
                }
                else
                {
                    TempData["ErrorMessage"] = respuesta.Mensaje;
                    return RedirectToAction("PanelClientes");
                }
            }
            TempData["ErrorMessage"] = "Error inesperado.";
            return RedirectToAction("PanelClientes");
        }








        //PANEL DE PAGOS
        public async Task<IActionResult> PanelPagos()
        {
            var listaPagos = await _pagoService.ListaPagos();

            PanelPagosModel model = new()
            {
                ListaPagos = listaPagos != null ? listaPagos : new List<PagoModel>()
            };
            return View("PanelPagos", model);
        }

    }
}
