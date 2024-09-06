namespace WebApp.Models.ViewModels
{
    public class ClientesViewModel
    {
        public int Id { get; set; }
        public string Dni { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public DateOnly FechaNacimiento { get; set; }
        public SexoEnum Sexo { get; set; }
        public int? IdMembresia { get; set; }
        public DateTime? FechaVencimientoMembresia { get; set; }
    }
}
