
async function buscarClientePorDni(dniCliente) {

    const response = await fetch(`/Usuario/BuscarCliente?dniCliente=${dniCliente}`);

    if (!response.ok) {
        throw new Error('Cliente no encontrado.');
    }

    const cliente = await response.json();
    return cliente;
}



const buscarClienteForm = document.getElementById('buscarClienteForm');
const modal = document.getElementById('user-info-card-modal');

buscarClienteForm.addEventListener('submit', async function (event) {
    event.preventDefault();
    try {
        const dniCliente = buscarClienteForm.querySelector('input[name="dniInput"]').value;
        const cliente = await buscarClientePorDni(dniCliente);

        modal.querySelector('#dni-field span').textContent = cliente.dni;
        modal.querySelector('#sex-field span').textContent = convertirSexo(cliente.sexo);
        modal.querySelector('#name-field span').textContent = cliente.nombre;
        modal.querySelector('#lastname-field span').textContent = cliente.apellido;
        modal.querySelector('#email-field span').textContent = cliente.email;
        modal.querySelector('#tel-field span').textContent = cliente.telefono;
        modal.querySelector('#birthdate-field span').textContent = convertirFechaDateOnly(cliente.fechaNacimiento);

        const fechaVencimiento = cliente.fechaVencimientoMembresia;

        if (fechaVencimiento == null || fechaVencimiento < new Date()) {
            modal.querySelector('#vencimiento-field span').style.color = 'var(--red-color)';
        } else {
            modal.querySelector('#vencimiento-field span').style.color = '#489f4e';
        }
        modal.querySelector('#vencimiento-field span').textContent = convertirFechaDateTime(fechaVencimiento);
        modal.showModal();

    } catch (error) {
        mostrarError(error.message);
    }
})

modal.querySelector('.close-btn').addEventListener('click', () => modal.close());