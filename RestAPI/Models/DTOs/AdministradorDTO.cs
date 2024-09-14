namespace RestAPI.Models.DTOs
{
    public class FechaNacimiento
    {
        public int Dia { get; set; }
        public int Mes {  get; set; }
        public int Año { get; set; }
    }
    public class AdministradorDTO
    {
        public string Usuario { get; set; }
        public string Contraseña { get; set; }
        public string Dni { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public FechaNacimiento FechaNacimientoDTO { get; set; }
        public int Sexo { get; set; }
    }
}
