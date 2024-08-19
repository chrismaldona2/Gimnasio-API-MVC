using Back.Entidades;
using Back.Repositorios.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Back.Repositorios
{
    public class ClienteRepositorio : Repositorio<Cliente>, IRepositorio<Cliente>
    {
        public ClienteRepositorio(GimnasioContext context) : base(context) { }
    }
}
