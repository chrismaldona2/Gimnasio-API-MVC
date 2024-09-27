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
    }, 2000);
})


const membresiaSelect = document.getElementById('membresiaSelect');
let clienteEncontrado = null;

document.addEventListener('DOMContentLoaded', async () => {
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
});


function mostrarMembresiaSelData() {
    const selectedOption = membresiaSelect.options[membresiaSelect.selectedIndex];

    let duracionDias = selectedOption.getAttribute('duracionData');
    let duracionMeses = (duracionDias / 30).toFixed(1);

    registrarModal.querySelector('span[name="idMembresia"]').textContent = selectedOption.value;
    registrarModal.querySelector('span[name="duracionMembresia"]').textContent = duracionDias;
    registrarModal.querySelector('span[name="duracionMesesMembresia"]').textContent = duracionMeses;

    let precio = parseFloat(selectedOption.getAttribute('precioData'));
    precio = precio.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
    registrarModal.querySelector('input[name="precioMembresia"]').value = precio;

    
    if (clienteEncontrado != null) {
        console.log('xd:' + clienteEncontrado.fechaVencimientoMembresia);
        if (clienteEncontrado.fechaVencimientoMembresia != null) {

            const fechaVencimientoNueva = new Date(clienteEncontrado.fechaVencimientoMembresia);
            fechaVencimientoNueva.setDate(fechaVencimientoNueva.getDate() + parseInt(duracionDias));

            registrarModal.querySelector('input[name="vencimientoNuevo"]').value = convertirFechaDateTime(fechaVencimientoNueva);
        } else {
            let fecha = new Date(); 
            fecha.setDate(fecha.getDate() + parseInt(duracionDias));
            console.log(fecha)
            registrarModal.querySelector('input[name="vencimientoNuevo"]').value = convertirFechaDateTime(fecha);
        }

    }

}
document.getElementById('membresiaSelect').addEventListener('change', mostrarMembresiaSelData);



const buscarClienteBtn = document.getElementById('buscarClienteBtn');

buscarClienteBtn.addEventListener('click', async () => {
    const dniCliente = document.getElementById('dniClienteInput').value;


    try {
        const cliente = await buscarClientePorDni(dniCliente);
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
    try {
        const pago = await buscarPagoPorId(id);
        const cliente = await buscarClientePorId(pago.idCliente);
        const membresia = await buscarMembresiaPorId(pago.idMembresia);

        eliminarModal.querySelector('input[name="Id"]').value = pago.id;
        eliminarModal.querySelector('input[name="IdCliente"]').value = cliente.id;
        eliminarModal.querySelector('input[name="IdMembresia"]').value = membresia.id;
        eliminarModal.querySelector('span[name="nombreCliente"]').textContent = `${cliente.nombre} ${cliente.apellido}`;
        eliminarModal.querySelector('span[name="dniCliente"]').textContent = cliente.dni;

        const fechaVencimiento = new Date(cliente.fechaVencimientoMembresia);

        eliminarModal.querySelector('span[name="vencimientoActual"]').textContent = convertirFechaDateTime(fechaVencimiento);

        eliminarModal.querySelector('span[name="tipoMembresia"]').textContent = membresia.tipo;
        const duracionDias = membresia.duracionDias;
        const duracionMeses = (duracionDias / 30).toFixed(1);
        eliminarModal.querySelector('span[name="duracionDiasMembresia"]').textContent = duracionDias;
        eliminarModal.querySelector('span[name="duracionMesesMembresia"]').textContent = duracionMeses;


        let fechaVencimientoNueva = new Date(cliente.fechaVencimientoMembresia);
        fechaVencimientoNueva.setDate(fechaVencimientoNueva.getDate() - membresia.duracionDias);
        console.log(fechaVencimientoNueva)

        eliminarModal.querySelector('input[name="vencimientoNuevoCliente"]').value = convertirFechaDateTime(fechaVencimientoNueva);

        eliminarModal.querySelector('input[name="FechaPago"]').value = convertirFechaDateTime(pago.fechaPago);
        eliminarModal.querySelector('input[name="Monto"]').value = pago.monto;

        eliminarModal.showModal();
    }
    catch {
        mostrarError('Error inesperado.');
    }
}
window.mostrarEliminarPagoModal = mostrarEliminarPagoModal;