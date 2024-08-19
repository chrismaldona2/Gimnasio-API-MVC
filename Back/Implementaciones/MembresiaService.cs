using Back.Entidades;
using Back.Implementaciones.Contratos;
using Back.Repositorios.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Back.Implementaciones
{
    public class MembresiaService : IMembresiaService
    {
        private readonly IRepositorio<Membresia> _membresiaRepository;

        public MembresiaService(IRepositorio<Membresia> membresiaRepository)
        {
            _membresiaRepository = membresiaRepository;
        }

        //LISTA
        public async Task<IEnumerable<Membresia>> ObtenerTodosLasMembresiasAsync()
        {
            return await _membresiaRepository.ReturnListaAsync();
        }

        //ALTA BAJA MODIFICACION
        public async Task RegistrarMembresiaAsync(string descripcion, double duracionmeses, double precio)
        {
            if (precio < 0)
            {
                throw new ArgumentException("El precio debe ser un número mayor o igual a cero");
            }

            if (duracionmeses <= 0)
            {
                throw new ArgumentException("La duración en meses debe ser mayor a 0.");
            }

            var membresiaNueva = new Membresia(descripcion, duracionmeses, precio);
            await _membresiaRepository.CrearAsync(membresiaNueva);
            await _membresiaRepository.GuardarCambiosAsync();
        }

        public async Task EliminarMembresiaAsync(int idMembresiaEliminar)
        {
            await _membresiaRepository.EliminarAsync(idMembresiaEliminar);
            await _membresiaRepository.GuardarCambiosAsync();
        }

        public async Task ModificarMembresiaAsync(Membresia membresiaModificar)
        {
            if (membresiaModificar != null)
            {
                if (membresiaModificar.Precio < 0)
                {
                    throw new ArgumentException("El precio debe ser un número mayor o igual a cero");
                }

                if (membresiaModificar.DuracionEnMeses <= 0)
                {
                    throw new ArgumentException("La duración en meses debe ser mayor a 0.");
                }

                await _membresiaRepository.ModificarAsync(membresiaModificar);
                await _membresiaRepository.GuardarCambiosAsync();
            }
        }


    }
}

