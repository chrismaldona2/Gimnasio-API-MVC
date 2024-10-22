import { buscarClientePorDni } from './busquedaService.js';

const buscarClienteForm = document.getElementById('buscarClienteForm');
const modal = document.getElementById('user-info-card-modal');

buscarClienteForm.addEventListener('submit', async function (event) {
    event.preventDefault();

    const dniCliente = buscarClienteForm.querySelector('input[name="dniInput"]').value;
    const cliente = await buscarClientePorDni(dniCliente);

    if (cliente) {
        modal.querySelector('#dni-field span').textContent = cliente.dni;
        modal.querySelector('#sex-field span').textContent = convertirSexo(cliente.sexo);
        modal.querySelector('#name-field span').textContent = cliente.nombre;
        modal.querySelector('#lastname-field span').textContent = cliente.apellido;
        modal.querySelector('#email-field span').textContent = cliente.email;
        modal.querySelector('#tel-field span').textContent = cliente.telefono;
        modal.querySelector('#birthdate-field span').textContent = convertirFechaDateOnly(cliente.fechaNacimiento);

        if (cliente.fechaVencimientoMembresia) {
            const fechaVencimiento = new Date(cliente.fechaVencimientoMembresia);
            modal.querySelector('#vencimiento-field span').textContent = convertirFechaDateTime(fechaVencimiento);
            if (fechaVencimiento < new Date()) {
                modal.querySelector('#vencimiento-field span').style.color = 'var(--red-color)';
            } else {
                modal.querySelector('#vencimiento-field span').style.color = 'var(--green-color)';
            }
        } else {
            modal.querySelector('#vencimiento-field span').style.color = 'var(--red-color)';
            modal.querySelector('#vencimiento-field span').textContent = "Sin membresía";
        }

        modal.showModal();
    }


})

modal.querySelector('.close-btn').addEventListener('click', () => modal.close());