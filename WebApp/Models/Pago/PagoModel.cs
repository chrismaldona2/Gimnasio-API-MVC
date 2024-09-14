namespace WebApp.Models.Pago
{
    public class PagoModel
    {
        public int Id { get; set; }
        public int IdCliente { get; set; }
        public int IdMembresia { get; set; }
        public DateTime FechaPago { get; set; }
        public double Monto { get; set; }
    }
}