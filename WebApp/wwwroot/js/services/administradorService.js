const registrarAdminBtn = document.getElementById('registrarAdminBtn');
const registrarAdminForm = document.getElementById('registrarAdminForm');
registrarAdminBtn.addEventListener('click', () => registrarAdminForm.showModal());
document.querySelector('#registrarAdminForm .cancel-btn').addEventListener('click', () => registrarAdminForm.close());




const modificarAdminForm = document.getElementById('modificarAdminForm');
function mostrarModificarAdminForm(id, nombre, apellido, dni, telefono, email, usuario, diaNac, mesNac, añoNac, sexo) {
    modificarAdminForm.querySelector('input[name="Id"]').value = id;
    modificarAdminForm.querySelector('input[name="Nombre"]').value = nombre;
    modificarAdminForm.querySelector('input[name="Apellido"]').value = apellido;
    modificarAdminForm.querySelector('input[name="Dni"]').value = dni;
    modificarAdminForm.querySelector('input[name="Telefono"]').value = telefono;
    modificarAdminForm.querySelector('input[name="Email"]').value = email;
    modificarAdminForm.querySelector('input[name="Usuario"]').value = usuario;
    modificarAdminForm.querySelector('select[name="FechaNacimientoDTO.Dia"]').value = diaNac;
    modificarAdminForm.querySelector('select[name="FechaNacimientoDTO.Mes"]').value = mesNac;
    modificarAdminForm.querySelector('select[name="FechaNacimientoDTO.Año"]').value = añoNac;
    modificarAdminForm.querySelector('select[name="Sexo"]').selectedIndex = sexo;
    modificarAdminForm.showModal();
}
document.querySelector('#modificarAdminForm .cancel-btn').addEventListener('click', () => modificarAdminForm.close());



const eliminarAdminForm = document.getElementById('eliminarAdminForm');
function mostrarEliminarAdminForm(id, nombre, apellido, dni, telefono, email, usuario, diaNac, mesNac, añoNac, sexo) {
    eliminarAdminForm.querySelector('input[name="Id"]').value = id;
    eliminarAdminForm.querySelector('input[name="Nombre"]').value = nombre;
    eliminarAdminForm.querySelector('input[name="Apellido"]').value = apellido;
    eliminarAdminForm.querySelector('input[name="Dni"]').value = dni;
    eliminarAdminForm.querySelector('input[name="Telefono"]').value = telefono;
    eliminarAdminForm.querySelector('input[name="Email"]').value = email;
    eliminarAdminForm.querySelector('input[name="Usuario"]').value = usuario;
    eliminarAdminForm.querySelector('input[name="FechaNacimientoDTO.Dia"]').value = diaNac;
    eliminarAdminForm.querySelector('input[name="FechaNacimientoDTO.Mes"]').value = mesNac;
    eliminarAdminForm.querySelector('input[name="FechaNacimientoDTO.Año"]').value = añoNac;
    eliminarAdminForm.querySelector('input[name="Sexo"]').value = sexo;
    eliminarAdminForm.showModal();
}
document.querySelector('#eliminarAdminForm .cancel-btn').addEventListener('click', () => eliminarAdminForm.close());



