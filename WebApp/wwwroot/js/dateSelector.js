const selectDia = document.getElementById('dia-input');
const selectMes = document.getElementById('mes-input');
const selectAno = document.getElementById('año-input');


const anoInicio = 1960;
const fechaActual = new Date();
const anoActual = fechaActual.getFullYear();
const mesActual = fechaActual.getMonth() + 1;
const diaActual = fechaActual.getDate();


for (let ano = anoInicio; ano <= anoActual; ano++) {
    let opcion = document.createElement('option');
    opcion.value = ano;
    opcion.text = ano;
    if (ano === anoActual) {
        opcion.selected = true;
    }

    selectAno.appendChild(opcion);
}



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


    selectDia.innerHTML = '';

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


actualizarDias();