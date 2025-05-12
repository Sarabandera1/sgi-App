CREATE DATABASE IF NOT EXISTS inventario;
USE inventario;


CREATE TABLE IF NOT EXISTS tipos_documento (
  id INT PRIMARY KEY AUTO_INCREMENT,
  descripcion VARCHAR(50)
);


CREATE TABLE IF NOT EXISTS tipo_terceros (
  id INT PRIMARY KEY AUTO_INCREMENT,
  descripcion VARCHAR(50)
);


CREATE TABLE IF NOT EXISTS pais (
  id INT PRIMARY KEY AUTO_INCREMENT,
  nombre VARCHAR(50)
);

CREATE TABLE IF NOT EXISTS region (
  id INT PRIMARY KEY AUTO_INCREMENT,
  nombre VARCHAR(50),
  pais_id INT,
  FOREIGN KEY (pais_id) REFERENCES pais(id)
);


CREATE TABLE IF NOT EXISTS ciudad (
  id INT PRIMARY KEY AUTO_INCREMENT,
  nombre VARCHAR(50),
  region_id INT,
  FOREIGN KEY (region_id) REFERENCES region(id)
);


CREATE TABLE IF NOT EXISTS terceros (
  id INT PRIMARY KEY AUTO_INCREMENT,
  nombre VARCHAR(50),
  apellidos VARCHAR(50),
  email VARCHAR(80),
  tipodoc_id INT,
  tipoTercero_id INT,
  ciudad_id INT,
  FOREIGN KEY (tipodoc_id) REFERENCES tipos_documento(id),
  FOREIGN KEY (tipoTercero_id) REFERENCES tipo_terceros(id),
  FOREIGN KEY (ciudad_id) REFERENCES ciudad(id)
);


CREATE TABLE IF NOT EXISTS tercero_telefonos (
  id INT PRIMARY KEY AUTO_INCREMENT,
  numero VARCHAR(20),
  tipo_tel VARCHAR(20),
  terceros_id INT,
  FOREIGN KEY (terceros_id) REFERENCES terceros(id)
);


CREATE TABLE IF NOT EXISTS cliente (
  id INT PRIMARY KEY AUTO_INCREMENT,
  tercero_id INT,
  fecha_id DATE,
  fechaUCompra DATE,
  FOREIGN KEY (tercero_id) REFERENCES terceros(id)
);


CREATE TABLE IF NOT EXISTS proveedor (
  id INT PRIMARY KEY AUTO_INCREMENT,
  tercero_id INT,
  dcto_double DECIMAL(5,2),
  diaPago INT,
  FOREIGN KEY (tercero_id) REFERENCES terceros(id)
);


CREATE TABLE IF NOT EXISTS eps (
  id INT PRIMARY KEY AUTO_INCREMENT,
  nombre VARCHAR(50)
);


CREATE TABLE IF NOT EXISTS empleado (
  id INT PRIMARY KEY AUTO_INCREMENT,
  tercero_id INT,
  fechaIngreso DATE,
  salarioBase DECIMAL(10,2),
  eps_id INT,
  FOREIGN KEY (tercero_id) REFERENCES terceros(id),
  FOREIGN KEY (eps_id) REFERENCES eps(id)
);


CREATE TABLE IF NOT EXISTS productos (
  id INT PRIMARY KEY AUTO_INCREMENT,
  nombre VARCHAR(50),
  stock INT,
  stockMin INT,
  stockMax INT,
  createdAt DATE,
  updatedAt DATE,
  barcode VARCHAR(50) UNIQUE
);


CREATE TABLE IF NOT EXISTS productos_proveedor (
  id INT PRIMARY KEY AUTO_INCREMENT,
  tercero_id INT,
  productos_id INT,
  FOREIGN KEY (tercero_id) REFERENCES terceros(id),
  FOREIGN KEY (productos_id) REFERENCES productos(id)
);


CREATE TABLE IF NOT EXISTS facturacion (
  id INT PRIMARY KEY AUTO_INCREMENT,
  fechaResolucion DATE,
  numInicio INT,
  numFinal INT,
  factActual INT
);


CREATE TABLE IF NOT EXISTS venta (
  id INT PRIMARY KEY AUTO_INCREMENT,
  fact_id INT,
  fecha DATE,
  terceroEM_id INT,
  terceroCli_id INT,
  FOREIGN KEY (fact_id) REFERENCES facturacion(id),
  FOREIGN KEY (terceroEM_id) REFERENCES terceros(id),
  FOREIGN KEY (terceroCli_id) REFERENCES terceros(id)
);


CREATE TABLE IF NOT EXISTS detalle_venta (
  id INT PRIMARY KEY AUTO_INCREMENT,
  fact_id INT,
  producto_id INT,
  cantidad INT,
  valor DECIMAL(10,2),
  FOREIGN KEY (fact_id) REFERENCES facturacion(id),
  FOREIGN KEY (producto_id) REFERENCES productos(id)
);


CREATE TABLE IF NOT EXISTS empresa (
  id INT PRIMARY KEY AUTO_INCREMENT,
  nombre VARCHAR(50),
  ciudad_id INT,
  fecha_reg DATE,
  FOREIGN KEY (ciudad_id) REFERENCES ciudad(id)
);


CREATE TABLE IF NOT EXISTS planes (
  id INT PRIMARY KEY AUTO_INCREMENT,
  nombre VARCHAR(30),
  fechaInicio DATE,
  fechaFin DATE,
  documento INT
);


CREATE TABLE IF NOT EXISTS planProducto (
  id INT PRIMARY KEY AUTO_INCREMENT,
  plan_id INT,
  producto_id INT,
  FOREIGN KEY (plan_id) REFERENCES planes(id),
  FOREIGN KEY (producto_id) REFERENCES productos(id)
);


CREATE TABLE IF NOT EXISTS tip_movCasa (
  id INT PRIMARY KEY AUTO_INCREMENT,
  nombre VARCHAR(50),
  tipo VARCHAR(20)
);

CREATE TABLE IF NOT EXISTS movCasa (
  id INT PRIMARY KEY AUTO_INCREMENT,
  fecha DATE,
  tipoMov_id INT,
  valor DECIMAL(10,2),
  concepto VARCHAR(50),
  tercero_id INT,
  FOREIGN KEY (tipoMov_id) REFERENCES tip_movCasa(id),
  FOREIGN KEY (tercero_id) REFERENCES terceros(id)
);


CREATE TABLE IF NOT EXISTS compras (
  id INT PRIMARY KEY AUTO_INCREMENT,
  terceroProv_id INT,
  fecha DATE,
  terceroEmp_id INT,
  docCompra VARCHAR(50),
  FOREIGN KEY (terceroProv_id) REFERENCES terceros(id),
  FOREIGN KEY (terceroEmp_id) REFERENCES terceros(id)
);


CREATE TABLE IF NOT EXISTS detalle_compra (
  id INT PRIMARY KEY AUTO_INCREMENT,
  fecha DATE,
  producto_id INT,
  cantidad INT,
  valor DECIMAL(10,2),
  compra_id INT,
  FOREIGN KEY (producto_id) REFERENCES productos(id),
  FOREIGN KEY (compra_id) REFERENCES compras(id)
);