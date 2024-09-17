using WebApp.Models.Cliente;
using WebApp.Models.Membresias;

namespace WebApp.Models.Pago
{
    public class PanelPagosModel : PagoModel
    {
        public List<PagoModel>? ListaPagos { get; set; }
        public List<MembresiaModel>? ListaMembresias { get; set; }
        public List<ClienteModel>? ListaClientes { get; set; }
    }
}
