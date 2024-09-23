async function buscarMembresiaPorId(idMembresia) {

    const response = await fetch(`/Administrador/BuscarMembresia?idMembresia=${idMembresia}`);
    console.log(response);
    if (!response.ok) {
        throw new Error('Membresía no encontrada.');
    }

    const membresia = await response.json();
    console.log(membresia);
    return membresia;
}


const registrarBtn = document.getElementById('registrarMembresiaBtn');
const registrarModal = document.getElementById('registrarMembresiaModal');

registrarBtn.addEventListener('click', () => registrarModal.showModal());
registrarModal.querySelector('.cancel-btn').addEventListener('click', () => registrarModal.close());



const eliminarModal = document.getElementById('eliminarMembresiaModal');
async function mostrarEliminarMembresiaModal(idMembresia) {
    try {
        const membresia = await buscarMembresiaPorId(idMembresia);
        eliminarModal.querySelector('input[name="Id"]').value = membresia.id;
        eliminarModal.querySelector('input[name="Tipo"]').value = membresia.tipo;
        eliminarModal.querySelector('input[name="DuracionDias"]').value = membresia.duracionDias;
        eliminarModal.querySelector('input[name="Precio"]').value = membresia.precio;

        eliminarModal.showModal();
    } catch (error) {
        mostrarError(error.message);
    }
}

eliminarModal.querySelector('.cancel-btn').addEventListener('click', () => eliminarModal.close());


const modificarModal = document.getElementById('modificarMembresiaModal');
async function mostrarModificarMembresiaModal(idMembresia) {
    try {
        const membresia = await buscarMembresiaPorId(idMembresia);
        modificarModal.querySelector('input[name="Id"]').value = membresia.id;
        modificarModal.querySelector('input[name="Tipo"]').value = membresia.tipo;
        modificarModal.querySelector('input[name="DuracionDias"]').value = membresia.duracionDias;
        modificarModal.querySelector('input[name="Precio"]').value = membresia.precio;
        modificarModal.showModal();
    } catch (error) {
        mostrarError(error.message);
    }
}

modificarModal.querySelector('.cancel-btn').addEventListener('click', () => modificarModal.close());