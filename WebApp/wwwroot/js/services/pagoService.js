
//SCRIPT PARA EL FORMULARIO DE CREACION DE PAGO

const buscarClienteBtn = document.getElementById('buscarClienteBtn');
const dniClienteInput = document.getElementById('dniClienteInput');
let clienteEncontrado = false;
const clientes = document.querySelectorAll('#clientesData > div');
document.getElementById('buscarClienteBtn').addEventListener('click', function () {
    let dniIngresado = dniClienteInput.value;
    clientes.forEach(function (cliente) {
        var dniCliente = cliente.getAttribute('cliente-dni-data');
        if (dniCliente === dniIngresado) {
            clienteEncontrado = true;
            document.getElementById('clienteNombreSel-c').textContent = cliente.getAttribute('cliente-nombre-data');
            document.getElementById('clienteMembresiaIdSel-c').textContent = cliente.getAttribute('cliente-membresia-data');
            document.getElementById('clienteVencimientoSel-c').textContent = cliente.getAttribute('cliente-vencimiento-data');
        }
    })
    if (!clienteEncontrado) {
        document.getElementById('clienteNombreSel-c').textContent = "Cliente no encontrado";
        document.getElementById('clienteMembresiaIdSel-c').textContent = "...";
        document.getElementById('clienteVencimientoSel-c').textContent = "...";
    }
    mostrarMembresiaSelData();
});

function mostrarMembresiaSelData() {
    const selectElement = document.getElementById('membresiaSelect');



    var selectedOption = selectElement.options[selectElement.selectedIndex];

    let duracionDias = selectedOption.getAttribute('membresia-duracion-dias-data');
    let duracionMeses = (duracionDias / 30).toFixed(1);

    document.getElementById('membresiaSelId-c').textContent = selectedOption.value;
    document.getElementById('membresiaSelDias-c').textContent = duracionDias;
    document.getElementById('membresiaSelMeses-c').textContent = duracionMeses;

    let precio = parseFloat(selectedOption.getAttribute('membresia-precio-data'));
    precio = precio.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
    document.getElementById('membresiaSelPrecio-c').value = precio;


    if (clienteEncontrado) {
        let fechaVencimientoStr = document.getElementById('clienteVencimientoSel-c').textContent;

        let partes = fechaVencimientoStr.split('/');
        let fecha = new Date(`${partes[2]}-${partes[1]}-${partes[0]}`);

        /*fecha vencimiento nueva*/
        fecha.setDate(fecha.getDate() + parseInt(duracionDias));
        let fechaVencimientoNueva = fecha.toLocaleDateString('es-ES'); 

        /*mostrar vencimiento en el input disabled*/
        document.getElementById('vencimientoNuevo-c').value = fechaVencimientoNueva;
    }
}


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


window.onload = mostrarMembresiaSelData;
document.getElementById('membresiaSelect').addEventListener('change', mostrarMembresiaSelData);



const registrarPagoBtn = document.getElementById('registrarPagoBtn');
const registrarPagoForm = document.getElementById('registrarPagoForm');
registrarPagoBtn.addEventListener('click', () => registrarPagoForm.showModal());
document.querySelector('#registrarPagoForm .cancel-btn').addEventListener('click', () => registrarPagoForm.close());




//SCRIPT PARA MOSTRAR ELF ORMULARIO DE ELIMINACION DE PAGO
const membresias = document.querySelectorAll('#membresiasData > div');
const eliminarPagoForm = document.getElementById('eliminarPagoForm');

document.querySelector('#eliminarPagoForm .cancel-btn').addEventListener('click', () => eliminarPagoForm.close());
function mostrarEliminarPagoForm(id, idCliente, idMembresia, fechaPago, monto) {
    eliminarPagoForm.querySelector('input[name="Id"]').value = id;
    eliminarPagoForm.querySelector('input[name="IdCliente"]').value = idCliente;
    eliminarPagoForm.querySelector('input[name="IdMembresia"]').value = idMembresia;

    let clientePagoDelEncontrado = false;
    let membresiaPagoDelEncontrado = false;
    clientes.forEach(function (cliente) {
        var idClienteEnLista = cliente.getAttribute('cliente-id-data');
        if (idClienteEnLista === idCliente) {
            clientePagoDelEncontrado = true;
            document.getElementById('clienteNombreSel-d').textContent = cliente.getAttribute('cliente-nombre-data');
            document.getElementById('clienteDniSel-d').textContent = cliente.getAttribute('cliente-dni-data');
            document.getElementById('clienteVencimientoSel-d').textContent = cliente.getAttribute('cliente-vencimiento-data');
        }
    })
    if (!clientePagoDelEncontrado) {
        document.getElementById('clienteNombreSel-d').textContent = "Cliente no encontrado";
        document.getElementById('clienteDniSel-d').textContent = "...";
        document.getElementById('clienteVencimientoSel-d').textContent = "...";
    }


    let duracionMembresiaDel = 0;
    membresias.forEach(function (membresia) {
        var idMembresiaEnLista = membresia.getAttribute('membresia-id-data');


        if (idMembresiaEnLista === idMembresia) {
            membresiaPagoDelEncontrado = true;
            document.getElementById('membresiaSelTipo-d').textContent = membresia.getAttribute('membresia-tipo-data');

            duracionMembresiaDel = membresia.getAttribute('membresia-dias-data');

            document.getElementById('membresiaSelDias-d').textContent = duracionMembresiaDel;
            document.getElementById('membresiaSelMeses-d').textContent = (duracionMembresiaDel / 30).toFixed(1);;
        }
    })
    if (!membresiaPagoDelEncontrado) {
        document.getElementById('membresiaSelTipo-d').textContent = "Membresía no encontrada";
        document.getElementById('membresiaSelDias-d').textContent = "...";
        document.getElementById('membresiaSelMeses-d').textContent = "...";
    }


    if (clientePagoDelEncontrado) {
        let fechaVencimientoStr = document.getElementById('clienteVencimientoSel-d').textContent;

        let partes = fechaVencimientoStr.split('/');
        let fecha = new Date(`${partes[2]}-${partes[1]}-${partes[0]}`);

        fecha.setDate(fecha.getDate() - parseInt(duracionMembresiaDel));
        let fechaVencimientoNueva = fecha.toLocaleDateString('es-ES');

        document.getElementById('vencimientoNuevo-d').value = fechaVencimientoNueva;
    }


    eliminarPagoForm.querySelector('input[name="FechaPagoDel"]').value = fechaPago;
    eliminarPagoForm.querySelector('input[name="Monto"]').value = monto;
    eliminarPagoForm.showModal();

}