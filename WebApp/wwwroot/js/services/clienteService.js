const registrarClienteBtn = document.getElementById('registrarClienteBtn');
const registrarClienteForm = document.getElementById('registrarClienteForm');
registrarClienteBtn.addEventListener('click', () => registrarClienteForm.showModal());
document.querySelector('#registrarClienteForm .cancel-btn').addEventListener('click', () => registrarClienteForm.close());


const modificarClienteForm = document.getElementById('modificarClienteForm');
function mostrarModificarClienteForm(id, nombre, apellido, dni, telefono, email, diaNac, mesNac, añoNac, sexo) {
    modificarClienteForm.querySelector('input[name="Id"]').value = id;
    modificarClienteForm.querySelector('input[name="Nombre"]').value = nombre;
    modificarClienteForm.querySelector('input[name="Apellido"]').value = apellido;
    modificarClienteForm.querySelector('input[name="Dni"]').value = dni;
    modificarClienteForm.querySelector('input[name="Telefono"]').value = telefono;
    modificarClienteForm.querySelector('input[name="Email"]').value = email;
    modificarClienteForm.querySelector('select[name="FechaNacimientoDTO.Dia"]').value = diaNac;
    modificarClienteForm.querySelector('select[name="FechaNacimientoDTO.Mes"]').value = mesNac;
    modificarClienteForm.querySelector('select[name="FechaNacimientoDTO.Año"]').value = añoNac;
    modificarClienteForm.querySelector('select[name="Sexo"]').selectedIndex = sexo;
    modificarClienteForm.showModal();
}
document.querySelector('#modificarClienteForm .cancel-btn').addEventListener('click', () => modificarClienteForm.close());



const eliminarClienteForm = document.getElementById('eliminarClienteForm');
function mostrarEliminarClienteForm(id, nombre, apellido, dni, telefono, email, diaNac, mesNac, añoNac, sexo) {
    eliminarClienteForm.querySelector('input[name="Id"]').value = id;
    eliminarClienteForm.querySelector('input[name="Nombre"]').value = nombre;
    eliminarClienteForm.querySelector('input[name="Apellido"]').value = apellido;
    eliminarClienteForm.querySelector('input[name="Dni"]').value = dni;
    eliminarClienteForm.querySelector('input[name="Telefono"]').value = telefono;
    eliminarClienteForm.querySelector('input[name="Email"]').value = email;
    eliminarClienteForm.querySelector('input[name="FechaNacimientoDTO.Dia"]').value = diaNac;
    eliminarClienteForm.querySelector('input[name="FechaNacimientoDTO.Mes"]').value = mesNac;
    eliminarClienteForm.querySelector('input[name="FechaNacimientoDTO.Año"]').value = añoNac;
    eliminarClienteForm.querySelector('input[name="Sexo"]').value = sexo;
    eliminarClienteForm.showModal();
}
document.querySelector('#eliminarClienteForm .cancel-btn').addEventListener('click', () => eliminarAdminForm.close());


