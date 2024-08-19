document.addEventListener("DOMContentLoaded", function () {

    const fadeOutTime = 3500; 

    setTimeout(function () {
        const alerts = document.querySelectorAll('.alert');
        alerts.forEach(alert => {
            alert.classList.add('fade-out');
        });
    }, fadeOutTime);

});