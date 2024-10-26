import { listadoMembresias, buscarClientePorDni, buscarMembresiaPorId, buscarPagoPorId, buscarClientePorId } from './busquedaService.js';


const registrarBtn = document.getElementById('registrarPagoBtn');
const registrarModal = document.getElementById('registrarPagoModal');
registrarBtn.addEventListener('click', () => registrarModal.showModal());
registrarModal.querySelector('.cancel-btn').addEventListener('click', () => registrarModal.close());

const vencimientoWarning = document.querySelector('.vencimiento-warning-container');
vencimientoWarning.addEventListener('click', () => {
    const warning = document.querySelector('.vencimiento-warning');

    warning.classList.remove('oculto')
    warning.classList.add('vencimiento-warning-animacion');

    setTimeout(() => {
        warning.classList.add('oculto')
        warning.classList.remove('vencimiento-warning-animacion');
    }, 3500);
})


const membresiaSelect = document.getElementById('membresiaSelect');
let clienteEncontrado = null;

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

        registrarModal.querySelector('span[name="idMembresia"]').textContent = selectedOption.value;
        registrarModal.querySelector('span[name="duracionMembresia"]').textContent = duracionDias;
        registrarModal.querySelector('span[name="duracionMesesMembresia"]').textContent = duracionMeses;

        let precio = parseFloat(selectedOption.getAttribute('precioData'));
        precio = precio.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
        registrarModal.querySelector('input[name="precioMembresia"]').value = precio;


        if (clienteEncontrado != null) {

            if (clienteEncontrado.fechaVencimientoMembresia != null) {

                const fechaVencimientoNueva = new Date(clienteEncontrado.fechaVencimientoMembresia);
                fechaVencimientoNueva.setDate(fechaVencimientoNueva.getDate() + parseInt(duracionDias));

                registrarModal.querySelector('input[name="vencimientoNuevo"]').value = convertirFechaDateTime(fechaVencimientoNueva);
            } else {
                let fecha = new Date();
                fecha.setDate(fecha.getDate() + parseInt(duracionDias));

                registrarModal.querySelector('input[name="vencimientoNuevo"]').value = convertirFechaDateTime(fecha);
            }

        }
    }

}
document.getElementById('membresiaSelect').addEventListener('change', mostrarMembresiaSelData);



const buscarClienteBtn = document.getElementById('buscarClienteBtn');

buscarClienteBtn.addEventListener('click', async () => {
    const dniCliente = document.getElementById('dniClienteInput').value;

    try {
        const cliente = await buscarClientePorDni(dniCliente);
        if (!cliente) {
            throw new Error('Cliente no encontrado.');
        }

        registrarModal.querySelector('span[name="nombreCliente"]').textContent = `${cliente.nombre} ${cliente.apellido}`;
        registrarModal.querySelector('span[name="membresiaCliente"]').textContent = "Indefinida";
        clienteEncontrado = cliente;

        if (cliente.idMembresia != null) {
            const membresiaCliente = await buscarMembresiaPorId(cliente.idMembresia);
            registrarModal.querySelector('span[name="membresiaCliente"]').textContent = membresiaCliente.tipo;
            const fechaVencimiento = new Date(cliente.fechaVencimientoMembresia);
            registrarModal.querySelector('span[name="vencimientoCliente"]').textContent = convertirFechaDateTime(fechaVencimiento);
        }
        mostrarMembresiaSelData();
    }
    catch {
        registrarModal.querySelector('span[name="nombreCliente"]').textContent = "Cliente no encontrado."
        registrarModal.querySelector('span[name="membresiaCliente"]').textContent = "...";
        registrarModal.querySelector('span[name="vencimientoCliente"]').textContent = "...";
    }
    
})




//SCRIPT PARA MOSTRAR ELF ORMULARIO DE ELIMINACION DE PAGO
const eliminarModal = document.getElementById('eliminarPagoModal');

document.querySelector('#eliminarPagoModal .cancel-btn').addEventListener('click', () => eliminarModal.close());
async function mostrarEliminarPagoModal(id) {
    const pago = await buscarPagoPorId(id);
    if (pago) {
        const cliente = await buscarClientePorId(pago.idCliente);
        const membresia = await buscarMembresiaPorId(pago.idMembresia);

        if (cliente && membresia) {
            eliminarModal.querySelector('input[name="Id"]').value = pago.id;
            eliminarModal.querySelector('input[name="IdCliente"]').value = cliente.id;
            eliminarModal.querySelector('input[name="IdMembresia"]').value = membresia.id;
            eliminarModal.querySelector('span[name="nombreCliente"]').textContent = `${cliente.nombre} ${cliente.apellido}`;
            eliminarModal.querySelector('span[name="dniCliente"]').textContent = cliente.dni;

            const fechaVencimiento = new Date(cliente.fechaVencimientoMembresia);

            eliminarModal.querySelector('span[name="vencimientoActual"]').textContent = convertirFechaDateTime(fechaVencimiento);

            eliminarModal.querySelector('span[name="tipoMembresia"]').textContent = membresia.tipo;
            const duracionDias = membresia.duracionDias;
            const duracionMeses = convertirAMes(duracionDias);
            eliminarModal.querySelector('span[name="duracionDiasMembresia"]').textContent = duracionDias;
            eliminarModal.querySelector('span[name="duracionMesesMembresia"]').textContent = duracionMeses;


            let fechaVencimientoNueva = new Date(cliente.fechaVencimientoMembresia);
            fechaVencimientoNueva.setDate(fechaVencimientoNueva.getDate() - membresia.duracionDias);

            eliminarModal.querySelector('input[name="vencimientoNuevoCliente"]').value = convertirFechaDateTime(fechaVencimientoNueva);

            eliminarModal.querySelector('input[name="FechaPago"]').value = convertirFechaDateTime(pago.fechaPago);
            eliminarModal.querySelector('input[name="Monto"]').value = pago.monto;

            eliminarModal.showModal();
        }


    }


}
window.mostrarEliminarPagoModal = mostrarEliminarPagoModal;



//SCRIPT PARA MOSTRAR ELF ORMULARIO DE ELIMINACION DE PAGO
const detalleModal = document.getElementById('detallePagoModal');

document.getElementById('cerrarDetallePagoBtn').addEventListener('click', () => detalleModal.close());
async function mostrarDetallePagoModal(id) {

    const pago = await buscarPagoPorId(id);
    if (pago) {
        detallePagoModal.querySelector('input[name="Id"]').value = pago.id;

        detallePagoModal.querySelector('input[name="IdCliente"]').value = pago.idCliente;
        detallePagoModal.querySelector('input[name="IdMembresia"]').value = pago.idMembresia;

        detallePagoModal.querySelector('input[name="FechaPago"]').value = extrarFechaDateTime(pago.fechaPago);
        detallePagoModal.querySelector('input[name="HoraPago"]').value = extrarHoraDateTime(pago.fechaPago);
        let monto = parseFloat(pago.monto);
        monto = monto.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
        detallePagoModal.querySelector('input[name="Monto"]').value = monto;

        detallePagoModal.showModal();
    }


}
window.mostrarDetallePagoModal = mostrarDetallePagoModal;



const detalleMembresia = document.getElementById('infoMembresiaModal');
const mostrarDetalleMembresiaBtn = document.getElementById('mostrarDetalleMembresiaBtn');
document.getElementById('cerrarDetalleMembresiaBtn').addEventListener('click', () => detalleMembresia.close());


mostrarDetalleMembresiaBtn.addEventListener('click', async () => {
    const idMembresia = detallePagoModal.querySelector('input[name="IdMembresia"]').value;
    const membresiaBuscada = await buscarMembresiaPorId(idMembresia);
    if (!membresiaBuscada) {
        detalleModal.close();
    } else {
        detalleMembresia.querySelector('input[name="IdDetalleMembresia"]').value = membresiaBuscada.id;
        detalleMembresia.querySelector('input[name="Tipo"]').value = membresiaBuscada.tipo;
        detalleMembresia.querySelector('input[name="DuracionDias"]').value = membresiaBuscada.duracionDias;
        detalleMembresia.querySelector('input[name="DuracionMeses"]').value = convertirAMes(membresiaBuscada.duracionDias);
        detalleMembresia.querySelector('input[name="Precio"]').value = membresiaBuscada.precio;
        detalleMembresia.showModal();
    }   
})





const detalleCliente = document.getElementById('infoClienteModal');
const mostrarDetalleClienteBtn = document.getElementById('mostrarDetalleClienteBtn');
document.getElementById('cerrarDetalleClienteBtn').addEventListener('click', () => detalleCliente.close());

mostrarDetalleClienteBtn.addEventListener('click', async () => {

    const idCliente = detallePagoModal.querySelector('input[name="IdCliente"]').value;
    const clienteBuscado = await buscarClientePorId(idCliente);
    if (!clienteBuscado) {
        detalleModal.close();
    } else {
        detalleCliente.querySelector('input[name="IdDetalleCliente"]').value = clienteBuscado.id;
        detalleCliente.querySelector('input[name="Nombre"]').value = clienteBuscado.nombre;
        detalleCliente.querySelector('input[name="Apellido"]').value = clienteBuscado.apellido;
        detalleCliente.querySelector('input[name="Dni"]').value = clienteBuscado.dni;
        detalleCliente.querySelector('input[name="Telefono"]').value = clienteBuscado.telefono;
        detalleCliente.querySelector('input[name="Email"]').value = clienteBuscado.email;
        detalleCliente.querySelector('input[name="FechaNacimiento"]').value = convertirFechaDateOnly(clienteBuscado.fechaNacimiento);
        detalleCliente.querySelector('input[name="Sexo"]').value = convertirSexo(clienteBuscado.sexo);

        detalleCliente.showModal();
    }



})