namespace RestAPI.Models.DTOs
{
    public class PagoDTO
    {
        public int IdCliente { get; set; }
        public int IdMembresia { get; set; }
        public DateTime FechaPago { get; set; }
        public double Monto { get; set; }
    }
}
