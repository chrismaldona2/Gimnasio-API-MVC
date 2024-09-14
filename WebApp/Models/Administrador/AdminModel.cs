namespace WebApp.Models.Administrador
{
    public class AdminModel : RegistroAdminDTO
    {
        public int Id { get; set; }
        public string Usuario { get; set; }
        public string? Contraseña { get; set; }
        public string Dni { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public DateOnly FechaNacimiento { get; set; }
        public SexoEnum Sexo { get; set; }
    }
}
