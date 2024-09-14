function configurarFecha(selectDia, selectMes, selectAno) {
    const anoInicio = 1960;
    const fechaActual = new Date();
    const anoActual = fechaActual.getFullYear();
    const mesActual = fechaActual.getMonth() + 1;
    const diaActual = fechaActual.getDate();

    // Llenar select de año
    for (let ano = anoInicio; ano <= anoActual; ano++) {
        let opcion = document.createElement('option');
        opcion.value = ano;
        opcion.text = ano;
        if (ano === anoActual) {
            opcion.selected = true;
        }
        selectAno.appendChild(opcion);
    }

    // Llenar select de mes
    const meses = [
        'Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio',
        'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'
    ];
    meses.forEach((mes, indice) => {
        let opcion = document.createElement('option');
        opcion.value = indice + 1;
        opcion.text = mes;
        if (indice + 1 === mesActual) {
            opcion.selected = true;
        }
        selectMes.appendChild(opcion);
    });

    function actualizarDias() {
        const anoSeleccionado = parseInt(selectAno.value);
        const mesSeleccionado = parseInt(selectMes.value);

        const diasEnMes = new Date(anoSeleccionado, mesSeleccionado, 0).getDate();

        selectDia.innerHTML = ''; // Limpiar el select de días

        for (let dia = 1; dia <= diasEnMes; dia++) {
            let opcion = document.createElement('option');
            opcion.value = dia;
            opcion.text = dia;
            if (dia === diaActual && anoSeleccionado === anoActual && mesSeleccionado === mesActual) {
                opcion.selected = true;
            }
            selectDia.appendChild(opcion);
        }
    }

    selectMes.addEventListener('change', actualizarDias);
    selectAno.addEventListener('change', actualizarDias);

    actualizarDias(); // Llamar la función para inicializar los días
}


const selectoresFecha = [
    { dia: 'dia-input-c', mes: 'mes-input-c', ano: 'año-input-c' },
    { dia: 'dia-input-u', mes: 'mes-input-u', ano: 'año-input-u' }
];

selectoresFecha.forEach((selector) => {
    const selectDia = document.getElementById(selector.dia);
    const selectMes = document.getElementById(selector.mes);
    const selectAno = document.getElementById(selector.ano);

    configurarFecha(selectDia, selectMes, selectAno);
});
