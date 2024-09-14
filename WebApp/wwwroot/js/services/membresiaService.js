const registrarMembresiaBtn = document.getElementById('registrarMembresiaBtn');
const registrarMembresiaForm = document.getElementById('registrarMembresiaForm');

registrarMembresiaBtn.addEventListener('click', () => registrarMembresiaForm.showModal());
document.querySelector('#registrarMembresiaForm .cancel-btn').addEventListener('click', () => registrarMembresiaForm.close());



const modificarMembresiaForm = document.getElementById('modificarMembresiaForm');

function mostrarModicarMembresiaForm(id, tipo, dias, precio) {
    modificarMembresiaForm.querySelector('input[name="Id"]').value = id;
    modificarMembresiaForm.querySelector('input[name="Tipo"]').value = tipo;
    modificarMembresiaForm.querySelector('input[name="DuracionDias"]').value = dias;
    modificarMembresiaForm.querySelector('input[name="Precio"]').value = precio;
    modificarMembresiaForm.showModal();
}
document.querySelector('#modificarMembresiaForm .cancel-btn').addEventListener('click', () => modificarMembresiaForm.close());




const eliminarMembresiaForm = document.getElementById('eliminarMembresiaForm');
document.querySelector('#eliminarMembresiaForm .cancel-btn').addEventListener('click', () => eliminarMembresiaForm.close());
function mostrarEliminarMembresiaForm(id, tipo, dias, precio) {
    eliminarMembresiaForm.querySelector('input[name="Id"]').value = id;
    eliminarMembresiaForm.querySelector('input[name="Tipo"]').value = tipo;
    eliminarMembresiaForm.querySelector('input[name="DuracionDias"]').value = dias;
    eliminarMembresiaForm.querySelector('input[name="Precio"]').value = precio;
    eliminarMembresiaForm.showModal();
}