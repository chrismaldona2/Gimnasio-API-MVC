export async function buscarClientePorDni(dniCliente) {

    const response = await fetch(`/Usuario/BuscarCliente?dniCliente=${dniCliente}`);

    if (!response.ok) {
        throw new Error('Cliente no encontrado.');
    }

    const cliente = await response.json();
    return cliente;
}


export async function buscarClientePorId(idCliente) {

    const response = await fetch(`/Administrador/BuscarCliente?idCliente=${idCliente}`);

    if (!response.ok) {
        throw new Error('Cliente no encontrado.');
    }

    const cliente = await response.json();
    console.log(cliente);
    return cliente;
}

export async function buscarMembresiaPorId(idMembresia) {

    const response = await fetch(`/Administrador/BuscarMembresia?idMembresia=${idMembresia}`);
    console.log(response);
    if (!response.ok) {
        throw new Error('Membresía no encontrada.');
    }

    const membresia = await response.json();
    console.log(membresia);
    return membresia;
}

export async function listadoMembresias() {

    const response = await fetch(`/Administrador/ListadoMembresias`);
    console.log(response);
    if (!response.ok) {
        throw new Error('Listado no encontrado.');
    }

    const membresias = await response.json();
    console.log(membresias);
    return membresias;
}


export async function buscarAdminPorId(idAdmin) {

    const response = await fetch(`/Administrador/BuscarAdmin?idAdmin=${idAdmin}`);

    if (!response.ok) {
        throw new Error('Administrador no encontrado.');
    }

    const admin = await response.json();
    console.log(admin);
    return admin;
}


export async function buscarPagoPorId(idPago) {
    const response = await fetch(`/Administrador/BuscarPago?idPago=${idPago}`);

    if (!response.ok) {
        throw new Error('Pago no encontrado.');
    }

    const pago = await response.json();
    console.log(pago);
    return pago;
}