using Back.Entidades;
using Back.Repositorios.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Back.Repositorios
{
    public class AdministradorRepositorio : Repositorio<Administrador>, IRepositorio<Administrador>
    {
        public AdministradorRepositorio(GimnasioContext context) : base(context) { }
    }
}
