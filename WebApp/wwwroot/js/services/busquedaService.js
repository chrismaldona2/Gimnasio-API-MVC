export async function buscarClientePorDni(dniCliente) {
    try {
        const response = await fetch(`/Usuario/BuscarCliente?dniCliente=${dniCliente}`);

        if (!response.ok) {
            throw new Error('Cliente no encontrado.');
        }

        const cliente = await response.json();
        return cliente;
    }
    catch (error) {
        mostrarError(error.message);
    }

}


export async function buscarClientePorId(idCliente) {
    try {
        const response = await fetch(`/Administrador/BuscarCliente?idCliente=${idCliente}`);

        if (!response.ok) {
            throw new Error('Cliente no encontrado.');
        }

        const cliente = await response.json();
        return cliente;
    } catch (error) {
        mostrarError(error.message);
    }

}

export async function buscarMembresiaPorId(idMembresia) {
    try {
        const response = await fetch(`/Administrador/BuscarMembresia?idMembresia=${idMembresia}`);
        if (!response.ok) {
            throw new Error('Membresía no encontrada.');
        }

        const membresia = await response.json();
        return membresia;
    }
    catch (error) {
        mostrarError(error.message);
    }
}

export async function listadoMembresias() {

    const response = await fetch(`/Administrador/ListadoMembresias`);
    if (!response.ok) {
        throw new Error('Listado no encontrado.');
    }

    const membresias = await response.json();
    return membresias;
}


export async function buscarAdminPorId(idAdmin) {
    try {
        const response = await fetch(`/Administrador/BuscarAdmin?idAdmin=${idAdmin}`);

        if (!response.ok) {
            throw new Error('Administrador no encontrado.');
        }

        const admin = await response.json();
        return admin;
    } catch (error) {
        mostrarError(error.message);
    }

}


export async function buscarPagoPorId(idPago) {
    try {
        const response = await fetch(`/Administrador/BuscarPago?idPago=${idPago}`);

        if (!response.ok) {
            throw new Error('Pago no encontrado.');
        }

        const pago = await response.json();
        return pago;
    }
    catch (error) {
        mostrarError(error.message);
    }
}