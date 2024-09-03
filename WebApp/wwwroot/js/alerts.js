document.addEventListener("DOMContentLoaded", function () {

    const fadeOutTime = 4000; 

    setTimeout(function () {
        const alerts = document.querySelectorAll('.alert');
        alerts.forEach(alert => {
            alert.classList.add('fade-out');
        });
    }, fadeOutTime);

});