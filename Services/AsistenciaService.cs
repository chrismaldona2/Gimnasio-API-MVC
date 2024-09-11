using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Services.Contratos;
using Data.Repositorios;
using Data.Repositorios.Contratos;
using Core.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class AsistenciaService : IAsistenciaService
    {
        private readonly IAsistenciaRepositorio _asistenciaRepository;
        private readonly IClienteRepositorio _clienteRepository;

        public AsistenciaService(IAsistenciaRepositorio asistenciaRepository, IClienteRepositorio clienteRepository)
        {
            _asistenciaRepository = asistenciaRepository;
            _clienteRepository = clienteRepository;
        }

        //listas
        public async Task<IEnumerable<Asistencia>> ObtenerAsistenciasAsync()
        {
            try
            {
                return await _asistenciaRepository.ReturnListaAsync();
            }
            catch (DbUpdateException)
            {
                throw new InvalidOperationException("Se produjo un error al intentar retornar la lista de asistencias.");
            }
            catch (Exception ex)
            {
                throw new Exception("Se produjo un error inesperado al intentar realizar la acción:", ex);
            }
        }

        public async Task<IEnumerable<Asistencia>> ObtenerAsistenciasIdClienteAsync(int idCliente)
        {
            try
            {
                var clienteExistente = await _clienteRepository.EncontrarPorIDAsync(idCliente);
                if (clienteExistente == null)
                {
                    throw new KeyNotFoundException("El cliente con el Id especificado no existe.");
                }

                return await _asistenciaRepository.ObtenerAsistenciaPorClienteAsync(idCliente);

            }
            catch (DbUpdateException)
            {
                throw new InvalidOperationException("Se produjo un error al intentar retornar la lista de asistencias del cliente.");
            }
            catch (Exception ex)
            {
                throw new Exception("Se produjo un error inesperado al intentar realizar la acción:", ex);
            }
        }


        public async Task<IEnumerable<Asistencia>> ObtenerAsistenciasDniClienteAsync(string dniCliente)
        {

            try
            {
                var cliente = await _clienteRepository.ObtenerClienteConDniAsync(dniCliente);
                if (cliente == null)
                {
                    throw new KeyNotFoundException("El cliente con el DNI especificado no existe.");
                }

                return await _asistenciaRepository.ObtenerAsistenciaPorClienteAsync(cliente.Id);
            }
            catch (DbUpdateException)
            {
                throw new InvalidOperationException("Se produjo un error al intentar retornar la lista de asistencias del cliente.");
            }
            catch (Exception ex)
            {
                throw new Exception("Se produjo un error inesperado al intentar realizar la acción:", ex);
            }
        }


        //baja
        public async Task EliminarAsistenciaAsync(int idAsistenciaEliminar)
        {
            try
            {
                var asistenciaExistente = await _asistenciaRepository.EncontrarPorIDAsync(idAsistenciaEliminar);
                if (asistenciaExistente == null)
                {
                    throw new KeyNotFoundException("La asistencia seleccionada no existe.");
                }
                await _asistenciaRepository.EliminarAsync(idAsistenciaEliminar);
                await _asistenciaRepository.GuardarCambiosAsync();
            }
            catch (DbUpdateException)
            {
                throw new InvalidOperationException("Se produjo un error al eliminar la asistencia seleccionada.");
            }
            catch (Exception ex)
            {
                throw new Exception("Se produjo un error inesperado al intentar realizar la acción:", ex);
            }
        }
    }
}
