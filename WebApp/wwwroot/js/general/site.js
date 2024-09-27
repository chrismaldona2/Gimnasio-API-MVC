function convertirFechaDateOnly(fecha) {
    const [year, month, day] = fecha.split('-');
    return `${day}/${month}/${year}`;
}

function convertirFechaDateTime(fecha) {
    const fechaConvertir = new Date(fecha);

    const day = String(fechaConvertir.getDate()).padStart(2, '0');
    const month = String(fechaConvertir.getMonth() + 1).padStart(2, '0');
    const year = fechaConvertir.getFullYear();

    const hours = String(fechaConvertir.getHours()).padStart(2, '0');
    const minutes = String(fechaConvertir.getMinutes()).padStart(2, '0');

    return `${day}/${month}/${year} ${hours}:${minutes}`;

}

function convertirSexo(sexo) {
    return sexo === 0 ? 'Masculino' : sexo === 1 ? 'Femenino' : sexo === 2 ? 'Otro' : 'Desconocido';
}

