namespace WebApp.Models.DTOs
{
    public class ClienteRegistroDTO
    {
        public string Dni { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public FechaNacimiento FechaNacimiento { get; set; }
        public int Sexo { get; set; }
    }
}
