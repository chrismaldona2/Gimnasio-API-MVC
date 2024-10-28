namespace WebApp.Models.Cliente
{
    public class ClienteModel
    {
        public int Id { get; set; }
        public string? Dni { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Email { get; set; }
        public string? Telefono { get; set; }
        public DateOnly? FechaNacimiento { get; set; }
        public SexoEnum? Sexo { get; set; }
        public DateOnly? FechaRegistro { get; set; }
        public int? IdMembresia { get; set; }
        public DateTime? FechaVencimientoMembresia { get; set; }
    }
}
