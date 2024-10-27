import { buscarClientePorDni, listadoPagosCliente } from './busquedaService.js';

let cliente = null;

const buscarClienteForm = document.getElementById('buscarClienteForm');
const modal = document.getElementById('user-info-card-modal');

buscarClienteForm.addEventListener('submit', async function (event) {
    event.preventDefault();

    const dniCliente = buscarClienteForm.querySelector('input[name="dniInput"]').value;
    cliente = await buscarClientePorDni(dniCliente);

    if (cliente) {
        modal.querySelector('#dni-field span').textContent = cliente.dni;
        modal.querySelector('#sex-field span').textContent = convertirSexo(cliente.sexo);
        modal.querySelector('#name-field span').textContent = cliente.nombre;
        modal.querySelector('#lastname-field span').textContent = cliente.apellido;
        modal.querySelector('#email-field span').textContent = cliente.email;
        modal.querySelector('#tel-field span').textContent = cliente.telefono;
        modal.querySelector('#birthdate-field span').textContent = convertirFechaDateOnly(cliente.fechaNacimiento);

        if (cliente.fechaVencimientoMembresia) {
            const fechaVencimiento = new Date(cliente.fechaVencimientoMembresia);
            modal.querySelector('#vencimiento-field span').textContent = convertirFechaDateTime(fechaVencimiento);
            if (fechaVencimiento < new Date()) {
                modal.querySelector('#vencimiento-field span').style.color = 'var(--red-color)';
            } else {
                modal.querySelector('#vencimiento-field span').style.color = 'var(--green-color)';
            }
        } else {
            modal.querySelector('#vencimiento-field span').style.color = 'var(--red-color)';
            modal.querySelector('#vencimiento-field span').textContent = "Sin membresía";
        }

        modal.showModal();
    }


})

modal.querySelector('.close-btn').addEventListener('click', () => modal.close());



const pagosClienteModal = document.getElementById('pagosClienteModal');
const mostrarPagosClienteBtn = document.getElementById('mostrarPagosClienteBtn');

async function mostrarPagosClienteModal() {
    if (cliente) {
        const pagosCliente = await listadoPagosCliente(cliente.id, "/Usuario/");
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


