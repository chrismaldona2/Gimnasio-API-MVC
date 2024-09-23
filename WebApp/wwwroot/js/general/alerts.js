document.addEventListener("DOMContentLoaded", function () {

    const fadeOutTime = 4000; 

    setTimeout(function () {
        const alerts = document.querySelectorAll('.alert');
        alerts.forEach(alert => {
            alert.classList.add('fade-out');
        });
    }, fadeOutTime);

});


function mostrarError (errorMsg) {
    const errorDiv = document.createElement('div');
    errorDiv.className = 'alert warning';
    errorDiv.innerHTML = `
                        <i class="fa-solid fa-circle-xmark"></i>
                        <span>${errorMsg}</span>
                    `;

    document.body.appendChild(errorDiv);

    const fadeOutTime = 4000;

    setTimeout(function () {
        const alerts = document.querySelectorAll('.alert');
        alerts.forEach(alert => {
            alert.classList.add('fade-out');
        });

    }, fadeOutTime);
}

function mostrarAdvertencia (warningMsg) {
    const errorDiv = document.createElement('div');
    errorDiv.className = 'alert warning';
    errorDiv.innerHTML = `
                            <i class="fa-solid fa-circle-exclamation"></i>
                            <span>${warningMsg}</span>
                        `;

    document.body.appendChild(errorDiv);

    const fadeOutTime = 4000;

    setTimeout(function () {
        const alerts = document.querySelectorAll('.alert');
        alerts.forEach(alert => {
            alert.classList.add('fade-out');
        });

    }, fadeOutTime);
}


