namespace WebApp.Models.ViewModels
{
    public enum SexoEnum
    {
        Masculino = 0,
        Femenino = 1,
        Otro = 2
    }

    public class AdministradoresViewModel
    {
        public int Id { get; set; }
        public string Usuario { get; set; }
        public string Dni { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public DateOnly FechaNacimiento { get; set; }
        public SexoEnum Sexo { get; set; }
    }
}
