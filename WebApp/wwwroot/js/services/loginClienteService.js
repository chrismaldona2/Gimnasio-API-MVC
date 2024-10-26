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
            const trHtml = `                        
                        <tr>
                            <td>
                                No hay pagos registrados
                            </td>
                            <td></td
                            <td></td>
                            <td></td>
                            <td></td>
                        </tr>`;
            pagosTableBody.innerHTML += trHtml;
        } else {
            pagosCliente.forEach((pago) => {
                const trHtml = `                        
                        <tr>
                            <td data-label="Id de Pago">
                                ${pago.id}
                            </td>

                            <td data-label="Id de Membresía">
                               ${pago.idMembresia}
                            </td>
                            <td data-label="Fecha de pago">
                                ${convertirFechaDateTime(pago.fechaPago)}
                            </td>
                            <td data-label="Monto">
                                ${pago.monto} ARS
                            </td>

                        </tr>`;
                pagosTableBody.innerHTML += trHtml;
            })
        }

        pagosClienteModal.showModal();
    }
}
mostrarPagosClienteBtn.addEventListener('click', mostrarPagosClienteModal);
document.getElementById('cerrarPagosClienteModal').addEventListener('click', () => pagosClienteModal.close());