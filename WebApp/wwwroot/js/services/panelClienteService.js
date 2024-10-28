import { buscarClientePorId, buscarMembresiaPorId, listadoMembresias, listadoPagosCliente } from './busquedaService.js';

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
    eliminarModal.querySelector('input[name="FechaRegistro"]').value = convertirFechaDateOnly(cliente.fechaRegistro);
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
    modificarModal.querySelector('select[name="Sexo"]').selectedIndex = cliente.sexo + 1;
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
    detalleModal.querySelector('input[name="FechaRegistro"]').value = convertirFechaDateOnly(cliente.fechaRegistro);

    clienteSeleccionado = cliente;

    detalleModal.showModal();
}
window.mostrarDetalleClienteModal = mostrarDetalleClienteModal; 

document.getElementById('cerrarDetalleClienteBtn').addEventListener('click', () => detalleModal.close());




const detalleMembresia = document.getElementById('infoMembresiaModal');
const mostrarDetalleMembresiaBtn = document.getElementById('mostrarDetalleMembresiaBtn');
document.getElementById('cerrarDetalleMembresiaBtn').addEventListener('click', () => detalleMembresia.close());


mostrarDetalleMembresiaBtn.addEventListener('click', async () => {
    const idMembresia = detalleModal.querySelector('input[name="IdMembresia"]').value;
    const numeroMembresia = Number(idMembresia);
    if (!isNaN(numeroMembresia) && idMembresia.trim() !== '') {
        const membresiaBuscada = await buscarMembresiaPorId(idMembresia);
        if (membresiaBuscada) {

            detalleMembresia.querySelector('input[name="IdDetalleMembresia"]').value = membresiaBuscada.id;
            detalleMembresia.querySelector('input[name="Tipo"]').value = membresiaBuscada.tipo;
            detalleMembresia.querySelector('input[name="DuracionDias"]').value = membresiaBuscada.duracionDias;
            detalleMembresia.querySelector('input[name="DuracionMeses"]').value = convertirAMes(membresiaBuscada.duracionDias);
            detalleMembresia.querySelector('input[name="Precio"]').value = membresiaBuscada.precio;
            detalleMembresia.showModal();
            
        } else {
            detalleModal.close();
        }
    }

})




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
                registrarPagoModal.querySelector('input[name="vencimientoNuevo"]').classList.add('vigente');

            } else {
                let fecha = new Date();
                fecha.setDate(fecha.getDate() + parseInt(duracionDias));

                registrarPagoModal.querySelector('input[name="vencimientoNuevo"]').value = convertirFechaDateTime(fecha);
            }

        }
    } else {
        registrarPagoModal.querySelector('input[name="precioMembresia"]').value = null;
        registrarPagoModal.querySelector('input[name="vencimientoNuevo"]').value = null;
        registrarPagoModal.querySelector('input[name="vencimientoNuevo"]').classList.remove('vigente');
    }



}
document.getElementById('membresiaSelect').addEventListener('change', mostrarMembresiaSelData);



const registrarPagoModal = document.getElementById('registrarPagoModal');
const registrarPagoBtn = document.getElementById('mostrarRegistrarPagoBtn');

async function mostrarRegistrarPagoModal() {
    if (clienteSeleccionado) {
        membresiaSelect.selectedIndex = 0;
        mostrarMembresiaSelData();
        registrarPagoModal.querySelector('input[name="DniCliente"]').value = clienteSeleccionado.dni;
        registrarPagoModal.querySelector('input[name="vencimientoActual"]').value = clienteSeleccionado.idMembresia != null ? convertirFechaDateTime(clienteSeleccionado.fechaVencimientoMembresia) : 'Sin membresía';

        if (clienteSeleccionado.fechaVencimientoMembresia == null || new Date(clienteSeleccionado.fechaVencimientoMembresia) < new Date()) {
            registrarPagoModal.querySelector('input[name="vencimientoActual"]').classList.add('vencida');
        } else {
            registrarPagoModal.querySelector('input[name="vencimientoActual"]').classList.remove('vencida');
        }
        registrarPagoModal.showModal();
    }

}

registrarPagoBtn.addEventListener('click', mostrarRegistrarPagoModal);
registrarPagoModal.querySelector('.cancel-btn').addEventListener('click', () => registrarPagoModal.close());



const pagosClienteModal = document.getElementById('pagosClienteModal');
const mostrarPagosClienteBtn = document.getElementById('mostrarPagosClienteBtn');

async function mostrarPagosClienteModal() {
    if (clienteSeleccionado) {
        const pagosCliente = await listadoPagosCliente(clienteSeleccionado.id);
        const pagosTableBody = document.getElementById('pagosClienteTableBody');
        pagosTableBody.innerHTML = "";

        if (pagosCliente.length === 0) {
            const tr = document.createElement('tr');
            const td = document.createElement('td');
            td.colSpan = 6;
            td.textContent = "No se encontraron pagos registrados.";
            tr.appendChild(td);
            pagosTableBody.appendChild(tr);
        } else {
            pagosCliente.forEach((pago) => {
                const tr = document.createElement('tr');

                const idTd = document.createElement('td');
                idTd.setAttribute('data-label', 'Id de Pago');
                idTd.textContent = pago.id;
                tr.appendChild(idTd);

                const idMembresiaTd = document.createElement('td');
                idMembresiaTd.setAttribute('data-label', 'Id de Membresía');
                idMembresiaTd.textContent = pago.idMembresia;
                tr.appendChild(idMembresiaTd);

                const fechaTd = document.createElement('td');
                fechaTd.setAttribute('data-label', 'Fecha de pago');
                fechaTd.textContent = convertirFechaDateTime(pago.fechaPago);
                tr.appendChild(fechaTd);

                const montoTd = document.createElement('td');
                montoTd.setAttribute('data-label', 'Monto');
                montoTd.textContent = `${pago.monto} ARS`;
                tr.appendChild(montoTd);

                pagosTableBody.appendChild(tr);
            });
        }

        
        pagosClienteModal.showModal();
    }
}
mostrarPagosClienteBtn.addEventListener('click', mostrarPagosClienteModal);
document.getElementById('cerrarPagosClienteModal').addEventListener('click', () => pagosClienteModal.close());




const tipobusquedaSelect = document.getElementById('tipoBusqueda');
const busquedaInput = document.getElementById('busqueda');

function actualizarBusquedaInputPlaceholder() {
    if (tipobusquedaSelect.value === 'FechaNacimiento' || tipobusquedaSelect.value === "FechaRegistro") {

        busquedaInput.placeholder = 'yyyy/mm/dd';
    }
    else if (tipobusquedaSelect.value === "FechaVencimientoMembresia")
    {
        busquedaInput.placeholder = 'yyyy/mm/dd hh:mm';
    }

    else {
        busquedaInput.placeholder = 'Buscar registro...';
    }
}
tipobusquedaSelect.addEventListener('change', actualizarBusquedaInputPlaceholder);
actualizarBusquedaInputPlaceholder();
