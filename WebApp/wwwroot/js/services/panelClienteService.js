
async function buscarClientePorId(idCliente) {

    const response = await fetch(`/Administrador/BuscarCliente?idCliente=${idCliente}`);

    if (!response.ok) {
        throw new Error('Cliente no encontrado.');
    }

    const cliente = await response.json();
    console.log(cliente);
    return cliente;
}



const registrarBtn = document.getElementById('registrarClienteBtn');
const registrarModal = document.getElementById('registrarClienteModal');

registrarBtn.addEventListener('click', () => registrarModal.showModal());
registrarModal.querySelector('.cancel-btn').addEventListener('click', () => registrarModal.close());



const eliminarModal = document.getElementById('eliminarClienteModal');
async function mostrarEliminarClienteModal(idCliente) {
    try {
        const cliente = await buscarClientePorId(idCliente);
        eliminarModal.querySelector('input[name="Id"]').value = cliente.id;
        eliminarModal.querySelector('input[name="Nombre"]').value = cliente.nombre;
        eliminarModal.querySelector('input[name="Apellido"]').value = cliente.apellido;
        eliminarModal.querySelector('input[name="Dni"]').value = cliente.dni;
        eliminarModal.querySelector('input[name="Telefono"]').value = cliente.telefono;
        eliminarModal.querySelector('input[name="Email"]').value = cliente.email;
        eliminarModal.querySelector('input[name="FechaNacimiento"]').value = convertirFechaDateOnly(cliente.fechaNacimiento);
        eliminarModal.querySelector('input[name="Sexo"]').value = convertirSexo(cliente.sexo);
        eliminarModal.showModal();
    } catch (error) {
        mostrarError(error.message);
    }
}

eliminarModal.querySelector('.cancel-btn').addEventListener('click', () => eliminarModal.close());



const modificarModal = document.getElementById('modificarClienteModal');
async function mostrarModificarClienteModal(idCliente) {
    try {
        const cliente = await buscarClientePorId(idCliente);
        modificarModal.querySelector('input[name="Id"]').value = cliente.id;
        modificarModal.querySelector('input[name="Nombre"]').value = cliente.nombre;
        modificarModal.querySelector('input[name="Apellido"]').value = cliente.apellido;
        modificarModal.querySelector('input[name="Dni"]').value = cliente.dni;
        modificarModal.querySelector('input[name="Telefono"]').value = cliente.telefono;
        modificarModal.querySelector('input[name="Email"]').value = cliente.email;
        modificarModal.querySelector('input[name="FechaNacimiento"]').value = cliente.fechaNacimiento;
        modificarModal.querySelector('select[name="Sexo"]').selectedIndex = cliente.sexo;
        modificarModal.showModal();
    } catch (error) {
        mostrarError(error.message);
    }
}

modificarModal.querySelector('.cancel-btn').addEventListener('click', () => modificarModal.close());