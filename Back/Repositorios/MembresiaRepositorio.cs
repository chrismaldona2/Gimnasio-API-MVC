using Back.Entidades;
using Back.Repositorios.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Back.Repositorios
{
    public class MembresiaRepositorio : Repositorio<Membresia>, IRepositorio<Membresia>
    {
        public MembresiaRepositorio(GimnasioContext context) : base(context) { }
    }
}
