-- Inserción de Países
INSERT INTO pais (nombre) VALUES 
('Panamá'),
('Costa Rica'),
('Paraguay'),
('Bolivia'),
('Guatemala'),
('Honduras'),
('Nicaragua'),
('El Salvador'),
('República Dominicana'),
('Puerto Rico');

-- Inserción de Regiones
INSERT INTO region (nombre, pais_id) VALUES 
('Panamá Oeste', 1),
('Colón', 1),
('Alajuela', 2),
('San José', 2),
('Central', 3),
('Asunción', 3),
('La Paz', 4),
('Santa Cruz', 4),
('Quetzaltenango', 5),
('Guatemala Ciudad', 5),
('Tegucigalpa', 6),
('San Pedro Sula', 6),
('León', 7),
('Managua', 7),
('Santa Ana', 8),
('San Salvador', 8),
('Santo Domingo', 9),
('Santiago', 9),
('Bayamón', 10),
('Ponce', 10);

-- Inserción de Ciudades
INSERT INTO ciudad (nombre, region_id) VALUES 
('Arraiján', 1),
('La Chorrera', 1),
('Colón', 2),
('Alajuela', 3),
('San José', 4),
('Encarnación', 5),
('Asunción', 6),
('El Alto', 7),
('Santa Cruz de la Sierra', 8),
('Quetzaltenango', 9),
('Guatemala', 10),
('Tegucigalpa', 11),
('San Pedro Sula', 12),
('León', 13),
('Managua', 14),
('Santa Ana', 15),
('San Salvador', 16),
('Santo Domingo', 17),
('Santiago de los Caballeros', 18),
('Bayamón', 19),
('Ponce', 20);

-- Inserción de Empresas
INSERT INTO empresa (id, nombre, ciudad_id, fecha_registro) VALUES 
('EMP101', 'Soluciones Avanzadas', 1, '2021-06-12'),
('EMP102', 'Red Latinoamericana TI', 4, '2020-09-25'),
('EMP103', 'Grupo Inova', 6, '2022-04-19'),
('EMP104', 'Servicios Digitales SRL', 8, '2019-12-02'),
('EMP105', 'Consultoría Global SAS', 10, '2023-01-30');

-- Inserción de Facturación
INSERT INTO facturacion (fechaResolucion, numInicio, numFinal, factura_actual) VALUES 
('2025-01-01', 3001, 4000, 3001),
('2025-01-01', 4001, 5000, 4001),
('2025-01-01', 5001, 6000, 5001);

-- Inserción de Planes
INSERT INTO plan (nombre, fecha_inicio, fecha_fin, descuento) VALUES 
('Plan Estándar', '2025-01-01', '2025-12-31', 0.05),
('Plan Avanzado', '2025-01-01', '2025-12-31', 0.20),
('Plan Corporativo', '2025-01-01', '2025-12-31', 0.30),
('Plan Elite', '2025-01-01', '2025-12-31', 0.40);

-- Inserción de Productos
INSERT INTO producto (id, nombre, stock, stockMin, stockMax, fecha_creacion, fecha_actualizacion, codigo_barra) VALUES 
('PROD101', 'Tablet Samsung', 60, 10, 120, '2025-01-01', '2025-01-01', '7509876543210'),
('PROD102', 'Monitor LG', 45, 10, 90, '2025-01-01', '2025-01-01', '7509876543211'),
('PROD103', 'Auriculares Bluetooth', 120, 25, 250, '2025-01-01', '2025-01-01', '7509876543212'),
('PROD104', 'Mouse Inalámbrico', 75, 15, 150, '2025-01-01', '2025-01-01', '7509876543213'),
('PROD105', 'Impresora Epson', 35, 10, 60, '2025-01-01', '2025-01-01', '7509876543214');

-- Inserción de Plan-Producto
INSERT INTO plan_producto (plan_id, producto_id) VALUES 
(1, 'PROD101'),
(1, 'PROD102'),
(2, 'PROD101'),
(2, 'PROD102'),
(2, 'PROD103'),
(3, 'PROD101'),
(3, 'PROD102'),
(3, 'PROD103'),
(3, 'PROD104'),
(4, 'PROD101'),
(4, 'PROD102'),
(4, 'PROD103'),
(4, 'PROD104'),
(4, 'PROD105');

-- Inserción de Tipos de Movimiento de Caja
INSERT INTO tipoMovCaja (nombre, tipoMovimiento) VALUES 
('Venta Directa', 'Entrada'),
('Compra Externa', 'Salida'),
('Costo de Operación', 'Salida'),
('Pago de Servicios', 'Salida'),
('Ingreso Extra', 'Entrada');

-- Inserción de Tipos de Documento
INSERT INTO tipo_documento (descripcion) VALUES 
('Documento Nacional de Identidad'),
('Carné de Residencia'),
('Pasaporte Biométrico'),
('RUC'),
('Licencia de Conducir');

-- Inserción de Tipos de Tercero
INSERT INTO tipo_tercero (descripcion) VALUES 
('Cliente Minorista'),
('Proveedor Nacional'),
('Colaborador'),
('Distribuidor Mixto'),
('Aliado Comercial');

-- Inserción de EPS
INSERT INTO eps (nombre) VALUES 
('Panasalud'),
('Salud Total'),
('Vida Sana'),
('Integra EPS'),
('Mi Salud');

-- Inserción de ARL
INSERT INTO arl (nombre) VALUES 
('Protección'),
('AseguraMás'),
('Fortaleza'),
('Prevención ARL'),
('Seguros Latam');

-- Inserción de Terceros
INSERT INTO tercero (id, nombre, apellidos, email, tipo_documento_id, tipo_tercero_id, ciudad_id) VALUES 
('CC9988776655', 'Luis', 'Cabrera', 'luis.cabrera@email.com', 1, 1, 1),
('CC8877665544', 'Andrea', 'Morales', 'andrea.morales@email.com', 1, 2, 4),
('CC7766554433', 'Esteban', 'Ruiz', 'esteban.ruiz@email.com', 1, 3, 6),
('CC6655443322', 'Diana', 'Castro', 'diana.castro@email.com', 1, 4, 8),
('CC5544332211', 'Marco', 'Valle', 'marco.valle@email.com', 1, 5, 10);

-- Inserción de Clientes
INSERT INTO cliente (tercero_id, fecha_nacimiento, fecha_compra) VALUES 
('CC9988776655', '1988-07-21', '2025-01-12'),
('CC6655443322', '1991-10-05', '2025-01-16'),
('CC5544332211', '1993-11-14', '2025-01-21');

-- Inserción de Proveedores
INSERT INTO proveedor (tercero_id, descuento, dia_pago) VALUES 
('CC8877665544', 0.12, 20),
('CC6655443322', 0.18, 28);

-- Inserción de Teléfonos de Terceros
INSERT INTO tercero_telefono (numero, tercero_id, tipo_telefono) VALUES 
('3101234567', 'CC9988776655', 'Movil'),
('5001234567', 'CC9988776655', 'Fijo'),
('3102345678', 'CC8877665544', 'Movil'),
('3103456789', 'CC7766554433', 'Movil'),
('3104567890', 'CC6655443322', 'Movil'),
('3105678901', 'CC5544332211', 'Movil');

-- Inserción de Empleados
INSERT INTO empleado (tercero_id, fecha_ingreso, salario_base, eps_id, arl_id) VALUES 
('CC7766554433', '2024-02-01', 2700000, 1, 1),
('CC5544332211', '2024-07-01', 3100000, 2, 2);

-- Inserción de Movimientos de Caja
INSERT INTO movimientoCaja (fecha, tipoMovimiento_id, valor, concepto, tercero_id) VALUES 
('2025-01-12', 1, 1600000, 'Venta equipos TI', 'CC9988776655'),
('2025-01-13', 2, 900000, 'Compra accesorios', 'CC8877665544'),
('2025-01-14', 3, 250000, 'Servicios eléctricos', NULL),
('2025-01-15', 4, 5500000, 'Pago mensual empleados', NULL),
('2025-01-16', 5, 1200000, 'Soporte técnico a cliente', 'CC6655443322');

-- Inserción de Compras
INSERT INTO compra (terceroProveedor_id, fecha, terceroEmpleado_id, DocCompra) VALUES 
('CC8877665544', '2025-01-13', 'CC7766554433', 'FAC-101'),
('CC6655443322', '2025-01-19', 'CC5544332211', 'FAC-102');

-- Inserción de Detalles de Compra
INSERT INTO detalle_compra (fecha, producto_id, cantidad, valor, compra_id) VALUES 
('2025-01-13', 'PROD101', 6, 2700000, 1),
('2025-01-13', 'PROD102', 4, 1100000, 1),
('2025-01-19', 'PROD103', 12, 600000, 2),
('2025-01-19', 'PROD104', 9, 450000, 2);

-- Inserción de Ventas
INSERT INTO venta (factura_id, fecha, terceroEmpleado_id, terceroCliente_id) VALUES 
(3001, '2025-01-12', 'CC7766554433', 'CC9988776655'),
(3002, '2025-01-15', 'CC5544332211', 'CC6655443322');

-- Inserción de Detalles de Venta
INSERT INTO detalle_venta (factura_id, producto_id, cantidad, valor) VALUES 
(3001, 'PROD101', 1, 650000),
(3001, 'PROD102', 1, 370000),
(3002, 'PROD103', 2, 130000),
(3002, 'PROD104', 1, 90000);

-- Inserción de Producto-Proveedor
INSERT INTO producto_proveedor (tercero_id, producto_id) VALUES 
('CC8877665544', 'PROD101'),
('CC8877665544', 'PROD102'),
('CC6655443322', 'PROD103'),
('CC6655443322', 'PROD104'),
('CC6655443322', 'PROD105');
