using WebApp.Models.DTOs;

namespace WebApp.Models.ViewModels
{
    public class PanelClientesViewModel
    {
        public IEnumerable<ClientesViewModel> ListaClientes { get; set; }
        public ClienteRegistroDTO ClienteRegistroDto { get; set; }
    }
}
