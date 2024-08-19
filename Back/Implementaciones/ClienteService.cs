using Back.Entidades;
using Back.Implementaciones.Contratos;
using Back.Repositorios.Contratos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Back.Implementaciones
{
    public class ClienteService : IClienteService
    {
        private readonly IRepositorio<Cliente> _clienteRepository;

        public ClienteService(IRepositorio<Cliente> clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        //METODOS DE BUSQUEDA
        public async Task<IEnumerable<Cliente>> ObtenerTodosLosClientesAsync()
        {
            return await _clienteRepository.ReturnListaAsync();
        }

        public async Task<Cliente> BuscarClientePorIDAsync(int id)
        {
            return await _clienteRepository.EncontrarPorIDAsync(id);
        }
        public async Task<Cliente> BuscarClientesPorDniAsync(string dni)
        {
            if (string.IsNullOrEmpty(dni) || dni.Length != 8 || !dni.All(char.IsDigit))
            {
                throw new ArgumentException("DNI inválido. Debe ser un número entero de 8 dígitos");
            }

            var cliente = _clienteRepository.EncontrarPorCondicionAsync(c => c.Dni == dni).FirstOrDefaultAsync();
            return await cliente;
        }

        //ALTA BAJA MODIFICACION

        public async Task RegistrarClienteAsync(string dni, string nombre, string apellido, DateOnly nacimiento, Sexo sexo, DateOnly vencimientomembresia)
        {
            if (string.IsNullOrEmpty(dni) || dni.Length != 8 || !dni.All(char.IsDigit))
            {
                throw new ArgumentException("DNI inválido. Debe ser un número entero de 8 dígitos");
            }

            if (_clienteRepository.EncontrarPorCondicionAsync(a => a.Dni == dni).FirstOrDefault() != null)
            {
                throw new InvalidOperationException("Ya existe un cliente registrado con dicho DNI");
            }

            var ClienteNuevo = new Cliente(dni, nombre, apellido, nacimiento, sexo, vencimientomembresia);
            await _clienteRepository.CrearAsync(ClienteNuevo);
            await _clienteRepository.GuardarCambiosAsync();
        }

        public async Task EliminarClienteAsync(int idClienteEliminar)
        {
            await _clienteRepository.EliminarAsync(idClienteEliminar);
            await _clienteRepository.GuardarCambiosAsync();
        }

        public async Task ModificarClienteAsync(Cliente clienteModificar)
        {
            if (clienteModificar != null)
            {
                if (string.IsNullOrEmpty(clienteModificar.Dni) || clienteModificar.Dni.Length != 8 || !clienteModificar.Dni.All(char.IsDigit))
                {
                    throw new ArgumentException("DNI inválido. Debe ser un número entero de 8 dígitos");
                }

                if (_clienteRepository.EncontrarPorCondicionAsync(a => a.Dni == clienteModificar.Dni && a.Id != clienteModificar.Id).FirstOrDefault() != null)
                {
                    throw new InvalidOperationException("El DNI especificado ya está en uso por otro cliente");
                }

                await _clienteRepository.ModificarAsync(clienteModificar);
                await _clienteRepository.GuardarCambiosAsync();
            }
        }


    }
}
