import { buscarAdminPorId } from './busquedaService.js';


const registrarBtn = document.getElementById('registrarAdminBtn');
const registrarModal = document.getElementById('registrarAdminModal');

registrarBtn.addEventListener('click', () => registrarModal.showModal());
registrarModal.querySelector('.cancel-btn').addEventListener('click', () => registrarModal.close());


const detalleModal = document.getElementById('detalleAdminModal');
async function mostrarDetalleAdminModal(idAdmin) {
    const admin = await buscarAdminPorId(idAdmin);
    detalleModal.querySelector('input[name="Id"]').value = admin.id;
    detalleModal.querySelector('input[name="Nombre"]').value = admin.nombre;
    detalleModal.querySelector('input[name="Apellido"]').value = admin.apellido;
    detalleModal.querySelector('input[name="Dni"]').value = admin.dni;
    detalleModal.querySelector('input[name="Telefono"]').value = admin.telefono;
    detalleModal.querySelector('input[name="Email"]').value = admin.email;
    detalleModal.querySelector('input[name="Usuario"]').value = admin.usuario;
    detalleModal.querySelector('input[name="FechaNacimiento"]').value = convertirFechaDateOnly(admin.fechaNacimiento);
    detalleModal.querySelector('input[name="Sexo"]').value = convertirSexo(admin.sexo);

    detalleModal.querySelector('input[name="FechaRegistro"]').value = convertirFechaDateOnly(admin.fechaRegistro);
    detalleModal.showModal();

}
window.mostrarDetalleAdminModal = mostrarDetalleAdminModal; //agrego la funcion al window para que sea accesible globalmente
detalleModal.querySelector('.regular-form-submit-btn').addEventListener('click', () => detalleModal.close());





const eliminarModal = document.getElementById('eliminarAdminModal');
async function mostrarEliminarAdminModal(idAdmin) {

    const admin = await buscarAdminPorId(idAdmin);
    eliminarModal.querySelector('input[name="Id"]').value = admin.id;
    eliminarModal.querySelector('input[name="Nombre"]').value = admin.nombre;
    eliminarModal.querySelector('input[name="Apellido"]').value = admin.apellido;
    eliminarModal.querySelector('input[name="Dni"]').value = admin.dni;
    eliminarModal.querySelector('input[name="Telefono"]').value = admin.telefono;
    eliminarModal.querySelector('input[name="Email"]').value = admin.email;
    eliminarModal.querySelector('input[name="Usuario"]').value = admin.usuario;
    eliminarModal.querySelector('input[name="FechaNacimiento"]').value = convertirFechaDateOnly(admin.fechaNacimiento);
    eliminarModal.querySelector('input[name="Sexo"]').value = convertirSexo(admin.sexo);

    eliminarModal.querySelector('input[name="FechaRegistro"]').value = convertirFechaDateOnly(admin.fechaRegistro);
    eliminarModal.showModal();

}
window.mostrarEliminarAdminModal = mostrarEliminarAdminModal; //agrego la funcion al window para que sea accesible globalmente
eliminarModal.querySelector('.cancel-btn').addEventListener('click', () => eliminarModal.close());



const modificarModal = document.getElementById('modificarAdminModal');
async function mostrarModificarAdminModal(idAdmin) {

    const admin = await buscarAdminPorId(idAdmin);
    modificarModal.querySelector('input[name="Id"]').value = admin.id;
    modificarModal.querySelector('input[name="Nombre"]').value = admin.nombre;
    modificarModal.querySelector('input[name="Apellido"]').value = admin.apellido;
    modificarModal.querySelector('input[name="Dni"]').value = admin.dni;
    modificarModal.querySelector('input[name="Telefono"]').value = admin.telefono;
    modificarModal.querySelector('input[name="Email"]').value = admin.email;
    modificarModal.querySelector('input[name="Usuario"]').value = admin.usuario;
    modificarModal.querySelector('input[name="FechaNacimiento"]').value = admin.fechaNacimiento;
    modificarModal.querySelector('select[name="Sexo"]').selectedIndex = admin.sexo + 1;
    modificarModal.showModal();

}
window.mostrarModificarAdminModal = mostrarModificarAdminModal; //agrego la funcion al window para que sea accesible globalmente
modificarModal.querySelector('.cancel-btn').addEventListener('click', () => modificarModal.close());


const tipobusquedaSelect = document.getElementById('tipoBusqueda');
const busquedaInput = document.getElementById('busqueda');

function actualizarBusquedaInputPlaceholder() {
    if (tipobusquedaSelect.value === 'FechaNacimiento' || tipobusquedaSelect.value === "FechaRegistro") {

        busquedaInput.placeholder = 'yyyy/mm/dd';
    } else {
        busquedaInput.placeholder = 'Buscar registro...';
    }
}
tipobusquedaSelect.addEventListener('change', actualizarBusquedaInputPlaceholder);
actualizarBusquedaInputPlaceholder();
