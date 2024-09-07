using WebApp.Models.DTOs;

namespace WebApp.Models.ViewModels
{
    public class PanelMembresiasViewModel
    {
        public IEnumerable<MembresiasViewModel> ListaMembresias { get; set; }
        public MembresiaRegistroDTO MembresiaRegistroDto { get; set; }
    }
}
