namespace RestAPI.Models.DTOs
{
    public class ClienteDTO
    {
        public string Dni { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public FechaNacimiento FechaNacimiento { get; set; }
        public int Sexo { get; set; }
        public int? IdMembresia { get; set; }
        public DateTime? FechaVencimientoMembresia { get; set; }
    }
}
