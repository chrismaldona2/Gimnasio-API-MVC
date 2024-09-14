const tableActionBtns = document.querySelectorAll('.table-action-btn');

tableActionBtns.forEach(btn => {
    btn.addEventListener('click', function (event) {
        event.stopPropagation();

        document.querySelectorAll('.action-dropdown-menu').forEach(menu => {
            if (menu !== this.querySelector('.action-dropdown-menu')) {
                menu.classList.remove('show-action-menu');
            }
        });

        const menu = this.querySelector('.action-dropdown-menu');
        menu.classList.toggle('show-action-menu');
    });
});

document.addEventListener('click', function () {
    document.querySelectorAll('.action-dropdown-menu').forEach(menu => {
        menu.classList.remove('show-action-menu');
    });
});