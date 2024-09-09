const registrarClienteBtn = document.getElementById('registrarClienteBtn');
const registrarAdminBtn = document.getElementById('registrarAdminBtn');
const registrarMembresiaBtn = document.getElementById('registrarMembresiaBtn');


const registrarClienteForm = document.getElementById('registrarClienteForm');
const registrarAdminForm = document.getElementById('registrarAdminForm');
const registrarMembresiaForm = document.getElementById('registrarMembresiaForm');

if (registrarAdminBtn) {
    registrarAdminBtn.addEventListener('click', () => registrarAdminForm.showModal());
    document.querySelector('#registrarAdminForm .cancel-btn').addEventListener('click', () => registrarAdminForm.close());
}

if (registrarClienteBtn) {
    registrarClienteBtn.addEventListener('click', () => registrarClienteForm.showModal());
    document.querySelector('#registrarClienteForm .cancel-btn').addEventListener('click', () => registrarClienteForm.close());
}

if (registrarMembresiaBtn) {
    registrarMembresiaBtn.addEventListener('click', () => registrarMembresiaForm.showModal());
    document.querySelector('#registrarMembresiaForm .cancel-btn').addEventListener('click', () => registrarMembresiaForm.close());
}




