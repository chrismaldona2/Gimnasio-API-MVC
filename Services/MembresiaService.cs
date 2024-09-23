using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Contratos;
using Data.Repositorios;
using Data.Repositorios.Contratos;
using Core.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class MembresiaService : IMembresiaService
    {
        private readonly IMembresiaRepositorio _membresiaRepository;

        public MembresiaService(IMembresiaRepositorio membresiaRepository)
        {
            _membresiaRepository = membresiaRepository;
        }

        //alta
        public async Task RegistrarMembresiaAsync(string tipo, int duraciondias, double precio)
        {
            if (precio < 0)
            {
                throw new ArgumentException("El precio debe ser un número mayor o igual a cero.");
            }

            if (duraciondias <= 0)
            {
                throw new ArgumentException("La duración debe ser mayor a 0 días.");
            }

            var membresia = new Membresia(tipo, duraciondias, precio);
            
            try
            {
                await _membresiaRepository.CrearAsync(membresia);
                await _membresiaRepository.GuardarCambiosAsync();
            }
            catch (DbUpdateException)
            {
                throw new InvalidOperationException("Se produjo un error al intentar registrar el tipo de membresia.");
            }
            catch (Exception ex)
            {
                throw new Exception("Se produjo un error inesperado al intentar realizar la acción:", ex);
            }
        }

        //baja
        public async Task EliminarMembresiaAsync(int idMembresiaEliminar)
        {
            try
            {
                var membresiaExistente = await _membresiaRepository.EncontrarPorIDAsync(idMembresiaEliminar);
                if (membresiaExistente == null)
                {
                    throw new KeyNotFoundException("La membresía con el Id especificado no existe.");
                }
                await _membresiaRepository.EliminarAsync(idMembresiaEliminar);
                await _membresiaRepository.GuardarCambiosAsync();
            }
            catch (DbUpdateException)
            {
                throw new InvalidOperationException("Se produjo un error al intentar eliminar el tipo de membresia.");
            }
            catch (Exception ex)
            {
                throw new Exception("Se produjo un error inesperado al intentar realizar la acción:", ex);
            }
        }

        //modificacion
        public async Task ModificarMembresiaAsync(Membresia membresiaModificar)
        {
            if (membresiaModificar == null)
            {
                throw new ArgumentNullException("La membresía a modificar no puede ser nula.");
            }

            if (membresiaModificar.Precio < 0)
            {
                throw new ArgumentException("El precio debe ser un número mayor o igual a cero.");
            }

            if (membresiaModificar.DuracionDias <= 0)
            {
                throw new ArgumentException("La duración debe ser mayor a 0 días.");
            }

            try
            {
                var membresiaExistente = await _membresiaRepository.EncontrarPorIDAsync(membresiaModificar.Id);
                if (membresiaExistente == null)
                {
                    throw new KeyNotFoundException("La membresía con el Id especificado no existe.");
                }

                membresiaExistente.Tipo = membresiaModificar.Tipo;
                membresiaExistente.Precio = membresiaModificar.Precio;
                membresiaExistente.DuracionDias = membresiaModificar.DuracionDias;

                await _membresiaRepository.ModificarAsync(membresiaExistente);
                await _membresiaRepository.GuardarCambiosAsync();
            }
            catch (DbUpdateException)
            {
                throw new InvalidOperationException("Se produjo un error al intentar modificar el tipo de membresia.");
            }
            catch (Exception ex)
            {
                throw new Exception("Se produjo un error inesperado al intentar realizar la acción:", ex);
            }
        }

        //lista
        public async Task<IEnumerable<Membresia>> ObtenerMembresiasAsync()
        {
            try
            {
                return await _membresiaRepository.ReturnListaAsync();
            }
            catch (DbUpdateException)
            {
                throw new InvalidOperationException("Se produjo un error al intentar retornar la lista de membresías.");
            }
            catch (Exception ex)
            {
                throw new Exception("Se produjo un error inesperado al intentar realizar la acción:", ex);
            }
        }


        public async Task<Membresia> BuscarMembresiaPorIdAsync(int id)
        {
            try
            {
                var membresia = await _membresiaRepository.EncontrarPorIDAsync(id);
                if (membresia == null)
                {
                    return null;
                }
                return membresia;
            }
            catch (Exception ex)
            {
                throw new Exception($"Se produjo un error inesperado al intentar realizar la acción: {ex.Message}");
            }
        }
    }
}

