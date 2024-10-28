USE GimnasioBD

SELECT * FROM Clientes

UPDATE Clientes
SET
	FechaVencimientoMembresia = '2025-01-01'
WHERE
	DNI = '36925874'