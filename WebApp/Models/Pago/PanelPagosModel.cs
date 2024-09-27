namespace WebApp.Models.Pago
{
    public class PanelPagosModel : PagoModel
    {
        public string? DniCliente { get; set; }
        public List<PagoModel>? ListaPagos { get; set; }
    }
}
