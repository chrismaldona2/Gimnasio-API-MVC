using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entidades;

namespace Services.Contratos
{
    public interface IClienteService
    {
        Task<IEnumerable<Cliente>> ObtenerClientesAsync();
        Task<IEnumerable<Cliente>> ObtenerClientesConTipoMembresiaAsync(int idMembresia);
        Task<IEnumerable<Cliente>> ObtenerClientesConMembresiaVencidaAsync();
        Task<Cliente> InicioClientePorDniAsync(string dni, bool registrarAsistencia);
        Task RegistrarClienteAsync(string dni, string nombre, string apellido, string email, string telefono, DateOnly fechanacimiento, Sexo sexo);
        Task EliminarClienteAsync(int id);
        Task ModificarClienteAsync(Cliente cliente);
    }
}
