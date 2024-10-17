import { buscarMembresiaPorId } from './busquedaService.js';


const registrarBtn = document.getElementById('registrarMembresiaBtn');
const registrarModal = document.getElementById('registrarMembresiaModal');

registrarBtn.addEventListener('click', () => registrarModal.showModal());
registrarModal.querySelector('.cancel-btn').addEventListener('click', () => registrarModal.close());


registrarModal.querySelector('input[name="DuracionDias"]').addEventListener('input', (e) => {
    registrarModal.querySelector('input[name="DuracionMeses"]').value = convertirAMes(e.target.value);
})

registrarModal.querySelector('input[name="DuracionMeses"]').addEventListener('input', (e) => {
    registrarModal.querySelector('input[name="DuracionDias"]').value = convertirADias(e.target.value);
})


const eliminarModal = document.getElementById('eliminarMembresiaModal');
async function mostrarEliminarMembresiaModal(idMembresia) {

    const membresia = await buscarMembresiaPorId(idMembresia);
    if (membresia) {
        eliminarModal.querySelector('input[name="Id"]').value = membresia.id;
        eliminarModal.querySelector('input[name="Tipo"]').value = membresia.tipo;
        eliminarModal.querySelector('input[name="DuracionDias"]').value = membresia.duracionDias;
        eliminarModal.querySelector('input[name="DuracionMeses"]').value = convertirAMes(membresia.duracionDias);
        eliminarModal.querySelector('input[name="Precio"]').value = membresia.precio;
        eliminarModal.showModal();
    }

}
window.mostrarEliminarMembresiaModal = mostrarEliminarMembresiaModal; //agrego la funcion al window para que sea accesible globalmente
eliminarModal.querySelector('.cancel-btn').addEventListener('click', () => eliminarModal.close());


const modificarModal = document.getElementById('modificarMembresiaModal');
async function mostrarModificarMembresiaModal(idMembresia) {

    const membresia = await buscarMembresiaPorId(idMembresia);

    if (membresia) {
        modificarModal.querySelector('input[name="Id"]').value = membresia.id;
        modificarModal.querySelector('input[name="Tipo"]').value = membresia.tipo;
        modificarModal.querySelector('input[name="DuracionDias"]').value = membresia.duracionDias;
        modificarModal.querySelector('input[name="DuracionMeses"]').value = convertirAMes(membresia.duracionDias);

        modificarModal.querySelector('input[name="Precio"]').value = membresia.precio;
        modificarModal.showModal();
    }


}

modificarModal.querySelector('input[name="DuracionDias"]').addEventListener('input', (e) => {
    modificarModal.querySelector('input[name="DuracionMeses"]').value = convertirAMes(e.target.value);
})

modificarModal.querySelector('input[name="DuracionMeses"]').addEventListener('input', (e) => {
    modificarModal.querySelector('input[name="DuracionDias"]').value = convertirADias(e.target.value);
})

window.mostrarModificarMembresiaModal = mostrarModificarMembresiaModal; //agrego la funcion al window para que sea accesible globalmente
modificarModal.querySelector('.cancel-btn').addEventListener('click', () => modificarModal.close());