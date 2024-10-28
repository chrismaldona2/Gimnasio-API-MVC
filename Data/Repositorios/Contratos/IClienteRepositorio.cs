using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositorios.Contratos
{
    public interface IClienteRepositorio : IRepositorio<Cliente>
    {
        Task<IEnumerable<Cliente>> ObtenerClientesConMembresiaVencidaAsync();
        Task<Cliente> ObtenerClienteConDniAsync(string dni);
    }
}
