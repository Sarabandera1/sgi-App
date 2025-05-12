📦 Sistema de Inventario - Gestión Integral
Este módulo de inventario es parte de un sistema de gestión comercial completo que abarca productos, proveedores, clientes, empleados, facturación, ventas, compras y movimientos de caja, todo interrelacionado con una estructura geográfica de países, regiones y ciudades.

🧱 Estructura General
La base de datos incluye los siguientes bloques de información:

🌍 Ubicación Geográfica
Países

Regiones (vinculadas a países)

Ciudades (vinculadas a regiones)

🏢 Entidades Comerciales
Empresas (con ciudad de operación)

Clientes, Proveedores, Empleados (heredados desde una entidad tercero común)

📦 Inventario de Productos
Tabla de productos con control de:

Stock mínimo y máximo

Códigos de barras

Fecha de creación y actualización

Asociación de productos a proveedores (producto_proveedor)

Asociación de productos a planes (plan_producto)

💼 Planes y Servicios
Planes con fecha de vigencia y descuentos aplicables

Relación entre planes y productos disponibles

💰 Operaciones Financieras
Facturación: manejo de rangos, resolución y factura actual

Movimientos de caja: ingresos y egresos clasificados

Ventas y Compras:

Ventas a clientes con detalle por producto

Compras a proveedores con detalle y empleados responsables

👥 Información de Personas
Terceros (base para clientes, proveedores, empleados)

Identificación, email, teléfonos, ciudad

Clientes: fecha de nacimiento y de compra

Proveedores: descuento aplicado y día de pago

Empleados: salario, EPS y ARL asignados
