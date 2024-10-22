import { buscarClientePorId, listadoMembresias } from './busquedaService.js';

let clienteSeleccionado = null;


const registrarBtn = document.getElementById('registrarClienteBtn');
const registrarModal = document.getElementById('registrarClienteModal');

registrarBtn.addEventListener('click', () => registrarModal.showModal());
registrarModal.querySelector('.cancel-btn').addEventListener('click', () => registrarModal.close());



const eliminarModal = document.getElementById('eliminarClienteModal');
async function mostrarEliminarClienteModal(idCliente) {
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

}
window.mostrarEliminarClienteModal = mostrarEliminarClienteModal; //agrego la funcion al window para que sea accesible globalmente
eliminarModal.querySelector('.cancel-btn').addEventListener('click', () => eliminarModal.close());



const modificarModal = document.getElementById('modificarClienteModal');
async function mostrarModificarClienteModal(idCliente) {
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

}
window.mostrarModificarClienteModal = mostrarModificarClienteModal; //agrego la funcion al window para que sea accesible globalmente
modificarModal.querySelector('.cancel-btn').addEventListener('click', () => modificarModal.close());


const detalleModal = document.getElementById('detalleClienteModal');
async function mostrarDetalleClienteModal(idCliente) {
    const cliente = await buscarClientePorId(idCliente);

    detalleModal.querySelector('input[name="IdCliente"]').value = cliente.id;
    detalleModal.querySelector('input[name="IdMembresia"]').value = cliente.idMembresia ?? 'Sin membresía';
    detalleModal.querySelector('input[name="Nombre"]').value = cliente.nombre;
    detalleModal.querySelector('input[name="Apellido"]').value = cliente.apellido;
    detalleModal.querySelector('input[name="Dni"]').value = cliente.dni;
    detalleModal.querySelector('input[name="Telefono"]').value = cliente.telefono;
    detalleModal.querySelector('input[name="Email"]').value = cliente.email;

    detalleModal.querySelector('input[name="FechaNacimiento"]').value = convertirFechaDateOnly(cliente.fechaNacimiento);

    detalleModal.querySelector('input[name="FechaVencimiento"]').value = cliente.idMembresia != null ? convertirFechaDateTime(cliente.fechaVencimientoMembresia) : 'No asignado';
    
    
    if (cliente.fechaVencimientoMembresia == null || new Date(cliente.fechaVencimientoMembresia) < new Date()) {
        detalleModal.querySelector('input[name="FechaVencimiento"]').classList.remove('vigente');
        detalleModal.querySelector('input[name="FechaVencimiento"]').classList.add('vencida');
    } else {
        detalleModal.querySelector('input[name="FechaVencimiento"]').classList.remove('vencida');
        detalleModal.querySelector('input[name="FechaVencimiento"]').classList.add('vigente');
    }

    detalleModal.querySelector('input[name="Sexo"]').value = convertirSexo(cliente.sexo);

    clienteSeleccionado = cliente;

    detalleModal.showModal();
}
window.mostrarDetalleClienteModal = mostrarDetalleClienteModal; 

document.getElementById('cerrarDetalleClienteBtn').addEventListener('click', () => detalleModal.close());




const membresiaSelect = document.getElementById('membresiaSelect');

document.addEventListener('DOMContentLoaded', async () => {
    try {
        const defaulOption = document.createElement("option");
        defaulOption.value = "";
        defaulOption.disabled = true;
        defaulOption.selected = true;
        defaulOption.textContent = "Seleccione una membresía";

        membresiaSelect.appendChild(defaulOption);

        const membresias = await listadoMembresias();
        membresias.forEach(membresia => {
            const option = document.createElement('option');
            option.value = membresia.id;
            option.textContent = membresia.tipo;

            option.setAttribute('duracionData', membresia.duracionDias);
            option.setAttribute('precioData', membresia.precio);

            membresiaSelect.appendChild(option);
        });

        mostrarMembresiaSelData();
    }
    catch {
        mostrarError('Error inesperado.');
    }
});

function mostrarMembresiaSelData() {
    if (membresiaSelect.selectedIndex !== 0) {
        const selectedOption = membresiaSelect.options[membresiaSelect.selectedIndex];
        let duracionDias = selectedOption.getAttribute('duracionData');
        let duracionMeses = convertirAMes(duracionDias);

        let precio = parseFloat(selectedOption.getAttribute('precioData'));
        precio = precio.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
        registrarPagoModal.querySelector('input[name="precioMembresia"]').value = precio;


        if (clienteSeleccionado != null) {

            if (clienteSeleccionado.fechaVencimientoMembresia != null) {

                const fechaVencimientoNueva = new Date(clienteSeleccionado.fechaVencimientoMembresia);
                fechaVencimientoNueva.setDate(fechaVencimientoNueva.getDate() + parseInt(duracionDias));

                registrarPagoModal.querySelector('input[name="vencimientoNuevo"]').value = convertirFechaDateTime(fechaVencimientoNueva);
            } else {
                let fecha = new Date();
                fecha.setDate(fecha.getDate() + parseInt(duracionDias));

                registrarPagoModal.querySelector('input[name="vencimientoNuevo"]').value = convertirFechaDateTime(fecha);
            }

        }
    }



}
document.getElementById('membresiaSelect').addEventListener('change', mostrarMembresiaSelData);



const registrarPagoModal = document.getElementById('registrarPagoModal');
const registrarPagoBtn = document.getElementById('mostrarRegistrarPagoBtn');

async function mostrarRegistrarPagoModal() {
    if (clienteSeleccionado) {
        registrarPagoModal.querySelector('input[name="DniCliente"]').value = clienteSeleccionado.dni;
        registrarPagoModal.querySelector('input[name="vencimientoActual"]').value = convertirFechaDateTime(clienteSeleccionado.fechaVencimientoMembresia);
        registrarPagoModal.showModal();
    }

}

registrarPagoBtn.addEventListener('click', mostrarRegistrarPagoModal);
registrarPagoModal.querySelector('.cancel-btn').addEventListener('click', () => registrarPagoModal.close());