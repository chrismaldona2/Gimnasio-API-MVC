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
using System.Net;

namespace Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepositorio _clienteRepository;
        private readonly IRepositorio<Membresia> _membresiaRepository;
        private readonly IAsistenciaRepositorio _asistenciaRepository;

        public ClienteService(IClienteRepositorio clienteRepository, IRepositorio<Membresia> membresiaRepository, IAsistenciaRepositorio asistenciaRepository)
        {
            _clienteRepository = clienteRepository;
            _membresiaRepository = membresiaRepository;
            _asistenciaRepository = asistenciaRepository;
        }

        //alta
        public async Task RegistrarClienteAsync(string dni, string nombre, string apellido, string email, string telefono, DateOnly fechaNacimiento, Sexo sexo)
        {

            if (fechaNacimiento > DateOnly.FromDateTime(DateTime.Now))
            {
                throw new FechaNacimientoException();
            }

            if (await _clienteRepository.ObtenerClienteConDniAsync(dni) != null)
            {
                throw new DniRegistradoException();
            }

            var cliente = new Cliente(dni, nombre, apellido, email, telefono, fechaNacimiento, sexo);
            try
            {
                await _clienteRepository.CrearAsync(cliente);
                await _clienteRepository.GuardarCambiosAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Se produjo un error inesperado al intentar realizar la acción: {ex.Message}");
            }
        }

        //baja
        public async Task EliminarClienteAsync(int idClienteEliminar)
        {
            try
            {
                var clienteExistente = await _clienteRepository.EncontrarPorIDAsync(idClienteEliminar);
                if (clienteExistente == null)
                {
                    throw new KeyNotFoundException("El cliente con el Id especificado no existe.");
                }
                await _clienteRepository.EliminarAsync(idClienteEliminar);
                await _clienteRepository.GuardarCambiosAsync();
            }
            catch (DbUpdateException)
            {
                throw new InvalidOperationException("Se produjo un error al intentar eliminar el cliente.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Se produjo un error inesperado al intentar realizar la acción: {ex.Message}");
            }
        }

        //modificacion
        public async Task ModificarClienteAsync(Cliente clienteModificar)
        {
            if (clienteModificar == null)
            {
                throw new ArgumentNullException("El cliente a modificar no puede ser nulo.");
            }

            if (clienteModificar.FechaNacimiento > DateOnly.FromDateTime(DateTime.Now))
            {
                throw new FechaNacimientoException();
            }

            var dniUsado = await _clienteRepository.ObtenerClienteConDniAsync(clienteModificar.Dni);

            if (dniUsado != null && dniUsado.Id != clienteModificar.Id)
            {
                throw new DniRegistradoException();
            };

            try
            {
                var clienteExistente = await _clienteRepository.EncontrarPorIDAsync(clienteModificar.Id);
                if (clienteExistente == null)
                {
                    throw new KeyNotFoundException("El cliente con el Id especificado no existe.");
                }

                clienteExistente.Dni = clienteModificar.Dni;
                clienteExistente.Nombre = clienteModificar.Nombre;
                clienteExistente.Apellido = clienteModificar.Apellido;
                clienteExistente.Email = clienteModificar.Email;
                clienteExistente.Telefono = clienteModificar.Telefono;
                clienteExistente.FechaNacimiento = clienteModificar.FechaNacimiento;
                clienteExistente.Sexo = clienteModificar.Sexo;

                await _clienteRepository.ModificarAsync(clienteExistente);
                await _clienteRepository.GuardarCambiosAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Se produjo un error inesperado al intentar realizar la acción: {ex.Message}");
            }
        }

        //listas
        public async Task<IEnumerable<Cliente>> ObtenerClientesAsync()
        {
            try
            {
                return await _clienteRepository.ReturnListaAsync();
            }
            catch (DbUpdateException)
            {
                throw new InvalidOperationException("Se produjo un error al intentar retornar la lista de clientes.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Se produjo un error inesperado al intentar realizar la acción: {ex.Message}");
            }
        }

        public async Task<IEnumerable<Cliente>> ObtenerClientesConTipoMembresiaAsync(int idMembresia)
        {
            try
            {
                return await _clienteRepository.ObtenerClientesConTipoMembresiaAsync(idMembresia);
            }
            catch (DbUpdateException)
            {
                throw new InvalidOperationException("Se produjo un error al intentar retornar la lista de clientes.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Se produjo un error inesperado al intentar realizar la acción: {ex.Message}");
            }
        }

        public async Task<IEnumerable<Cliente>> ObtenerClientesConMembresiaVencidaAsync()
        {
            try
            {
                return await _clienteRepository.ObtenerClientesConMembresiaVencidaAsync();
            }
            catch (DbUpdateException)
            {
                throw new InvalidOperationException("Se produjo un error al intentar retornar la lista de clientes.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Se produjo un error inesperado al intentar realizar la acción: {ex.Message}");
            }
        }


        //buscar por dni
        public async Task<Cliente> BuscarClientePorDniAsync(string dni)
        {

            try
            {
                var cliente = await _clienteRepository.ObtenerClienteConDniAsync(dni);
                if (cliente == null)
                {
                    return null;
                }
                return cliente;
            }
            catch (Exception ex)
            {
                throw new Exception($"Se produjo un error inesperado al intentar realizar la acción: {ex.Message}");
            }
        }

        //registrar asistencia
        public async Task RegistrarAsistenciaAsync(string dni)
        {
            var cliente = await _clienteRepository.ObtenerClienteConDniAsync(dni);

            if (cliente == null)
            {
                throw new KeyNotFoundException("La cliente especificado no fue encontrado.");
            }

            var asistencia = new Asistencia(cliente.Id, DateTime.Now);

            try
            {
                await _asistenciaRepository.CrearAsync(asistencia);
                await _asistenciaRepository.GuardarCambiosAsync();

            }
            catch (DbUpdateException)
            {
                throw new InvalidOperationException("Se produjo un error al intentar registrar la asistencia.");
            }
            catch (Exception ex)
            {
                throw new Exception("Se produjo un error inesperado al intentar realizar la acción:", ex);
            }
        }

    }
}
