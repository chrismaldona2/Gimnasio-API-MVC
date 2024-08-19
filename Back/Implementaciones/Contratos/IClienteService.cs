using Back.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Back.Implementaciones.Contratos
{
    public interface IClienteService
    {
        Task<IEnumerable<Cliente>> ObtenerTodosLosClientesAsync();
        Task<Cliente> BuscarClientePorIDAsync(int id);
        Task<Cliente> BuscarClientesPorDniAsync(string dni);
        Task RegistrarClienteAsync(string dni, string nombre, string apellido, DateOnly nacimiento, Sexo sexo, DateOnly vencimientomembresia);
        Task EliminarClienteAsync(int id);
        Task ModificarClienteAsync(Cliente cliente);
    }
}
