//cada vez que se llena el formulario para ver la info del cliente se recarga la pagina enviando el model
//si el model != null entonces "dialog"" existe
document.addEventListener("DOMContentLoaded", function () {
//cada vez que carga la pagina se ejecuta lo siguiente
    var dialog = document.getElementById("user-info-card-modal");
    var closeButton = document.getElementById("close-btn");
    //si dialog existe se lo muestra en forma de Modal
    if (dialog) { dialog.showModal() }

    closeButton.addEventListener("click", () => { dialog.close()});
});