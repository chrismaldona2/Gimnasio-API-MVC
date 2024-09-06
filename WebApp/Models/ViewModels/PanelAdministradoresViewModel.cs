using WebApp.Models.DTOs;

namespace WebApp.Models.ViewModels
{
    public class PanelAdministradoresViewModel
    {
        public IEnumerable<AdministradoresViewModel> ListaAdministradores { get; set; }
        public AdministradorRegistroDTO AdministradorRegistroDto { get; set; }
    }
}
