namespace RestAPI.Models.Entidades
{
    public enum SexoModel
    {
        Masculino = 0,
        Femenino = 1,
        Otro = 2
    }

    //creado para mostrar admins ocultando contraseñas para mayor seguridad.
    public class AdministradorModel
    {
        public int Id { get; set; }
        public string Usuario { get; set; }
        public string Dni { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public DateOnly FechaNacimiento { get; set; }
        public SexoModel Sexo { get; set; }
    }
}
