using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sgi-App.infrastructure.repositorios
    public class Repocompras : IRepository<Compra>
    {
        private readonly Repoterceros _repoterceros;
        private readonly Repoproductos _repoproductos;

        public CompraRepository()
        {
            _repoterceros = new Repoterceros();
            _Repoproductos= new Repoproductos();
        }

        public async Task<IEnumerable<Compra>> GetAllAsync()
        {
            List<Compra> compras = new List<Compra>();
            
            using (var dbContext = new DbContext())
            {
                using var command = new MySqlCommand(
                    "SELECT c.*, " +
                    "tp.nombre as proveedor_nombre, tp.apellidos as proveedor_apellidos, " +
                    "te.nombre as empleado_nombre, te.apellidos as empleado_apellidos " +
                    "FROM compra c " +
                    "LEFT JOIN tercero tp ON c.tercero_proveedor_id = tp.id " +
                    "LEFT JOIN tercero te ON c.tercero_empleado_id = te.id " +
                    "ORDER BY c.fecha DESC",
                    dbContext.Connection);
                
                using var reader = await command.ExecuteReaderAsync();
                
                while (await reader.ReadAsync())
                {
                    var compra = new Compra
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        TerceroProveedorId = reader["tercero_proveedor_id"].ToString()!,
                        TerceroEmpleadoId = reader["tercero_empleado_id"].ToString()!,
                        Fecha = Convert.ToDateTime(reader["fecha"]),
                        DocCompra = reader["doc_compra"].ToString()!,
                        Total = Convert.ToDecimal(reader["total"]),
                        Detalles = new List<DetalleCompra>()
                    };

                    // Cargar datos del proveedor
                    compra.Proveedor = new Tercero
                    {
                        Id = compra.TerceroProveedorId,
                        Nombre = reader["proveedor_nombre"].ToString()!,
                        Apellidos = reader["proveedor_apellidos"].ToString()!
                    };

                    // Cargar datos del empleado
                    compra.Empleado = new Tercero
                    {
                        Id = compra.TerceroEmpleadoId,
                        Nombre = reader["empleado_nombre"].ToString()!,
                        Apellidos = reader["empleado_apellidos"].ToString()!
                    };

                    compras.Add(compra);
                }
            }
            
            // Cargar detalles para cada compra
            foreach (var compra in compras)
            {
                compra.Detalles = (List<DetalleCompra>)await GetDetallesCompraAsync(compra.Id);
            }
            
            return compras;
        }

        public async Task<Compra?> GetByIdAsync(object id)
        {
            Compra? compra = null;
            
            using (var dbContext = new DbContext())
            {
                using var command = new MySqlCommand(
                    "SELECT c.*, " +
                    "tp.nombre as proveedor_nombre, tp.apellidos as proveedor_apellidos, " +
                    "te.nombre as empleado_nombre, te.apellidos as empleado_apellidos " +
                    "FROM compra c " +
                    "LEFT JOIN tercero tp ON c.tercero_proveedor_id = tp.id " +
                    "LEFT JOIN tercero te ON c.tercero_empleado_id = te.id " +
                    "WHERE c.id = @Id",
                    dbContext.Connection);
                
                command.Parameters.AddWithValue("@Id", id);
                
                using var reader = await command.ExecuteReaderAsync();
                
                if (await reader.ReadAsync())
                {
                    compra = new Compra
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        TerceroProveedorId = reader["tercero_proveedor_id"].ToString()!,
                        TerceroEmpleadoId = reader["tercero_empleado_id"].ToString()!,
                        Fecha = Convert.ToDateTime(reader["fecha"]),
                        DocCompra = reader["doc_compra"].ToString()!,
                        Total = Convert.ToDecimal(reader["total"]),
                        Detalles = new List<DetalleCompra>()
                    };

                    // Cargar datos del proveedor
                    compra.Proveedor = new Tercero
                    {
                        Id = compra.TerceroProveedorId,
                        Nombre = reader["proveedor_nombre"].ToString()!,
                        Apellidos = reader["proveedor_apellidos"].ToString()!
                    };

                    // Cargar datos del empleado
                    compra.Empleado = new Tercero
                    {
                        Id = compra.TerceroEmpleadoId,
                        Nombre = reader["empleado_nombre"].ToString()!,
                        Apellidos = reader["empleado_apellidos"].ToString()!
                    };
                }
            }
            
            // Cargar detalles de la compra
            if (compra != null)
            {
                compra.Detalles = (List<DetalleCompra>)await GetDetallesCompraAsync(compra.Id);
            }
            
            return compra;
        }

        public async Task<IEnumerable<DetalleCompra>> GetDetallesCompraAsync(int compraId)
        {
            List<DetalleCompra> detalles = new List<DetalleCompra>();
            
            using (var dbContext = new DbContext())
            {
                using var command = new MySqlCommand(
                    "SELECT dc.*, p.nombre as producto_nombre, p.precio as producto_precio " +
                    "FROM detalle_compra dc " +
                    "LEFT JOIN producto p ON dc.producto_id = p.id " +
                    "WHERE dc.compra_id = @CompraId",
                    dbContext.Connection);
                
                command.Parameters.AddWithValue("@CompraId", compraId);
                
                using var reader = await command.ExecuteReaderAsync();
                
                while (await reader.ReadAsync())
                {
                    var detalle = new DetalleCompra
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        CompraId = compraId,
                        ProductoId = reader["producto_id"].ToString()!,
                        Cantidad = Convert.ToInt32(reader["cantidad"]),
                        Valor = Convert.ToDecimal(reader["valor"]),
                        Fecha = Convert.ToDateTime(reader["fecha"])
                    };

                    // Cargar datos del producto
                    detalle.Producto = new Producto
                    {
                        Id = Convert.ToInt32(detalle.ProductoId),
                        Nombre = reader["producto_nombre"].ToString()!,
                        Precio = Convert.ToDecimal(reader["producto_precio"])
                    };

                    // Calcular subtotal
                    detalle.Subtotal = detalle.Cantidad * detalle.Valor;

                    detalles.Add(detalle);
                }
            }
            
            return detalles;
        }

        public async Task<bool> InsertAsync(Compra compra)
        {
            using (var dbContext = new DbContext())
            {
                // Iniciar transacción
                using var transaction = await dbContext.Connection.BeginTransactionAsync();
                
                try
                {
                    // Calcular total de la compra
                    decimal total = 0;
                    foreach (var detalle in compra.Detalles)
                    {
                        total += detalle.Cantidad * detalle.Valor;
                    }
                    compra.Total = total;

                    // Insertar la compra
                    using (var command = new MySqlCommand(
                        "INSERT INTO compra (tercero_proveedor_id, tercero_empleado_id, fecha, doc_compra, total) " +
                        "VALUES (@TerceroProveedorId, @TerceroEmpleadoId, @Fecha, @DocCompra, @Total); " +
                        "SELECT LAST_INSERT_ID();",
                        dbContext.Connection))
                    {
                        command.Transaction = transaction;
                        command.Parameters.AddWithValue("@TerceroProveedorId", compra.TerceroProveedorId);
                        command.Parameters.AddWithValue("@TerceroEmpleadoId", compra.TerceroEmpleadoId);
                        command.Parameters.AddWithValue("@Fecha", compra.Fecha);
                        command.Parameters.AddWithValue("@DocCompra", compra.DocCompra);
                        command.Parameters.AddWithValue("@Total", compra.Total);
                        
                        // Obtener el ID de la compra insertada
                        compra.Id = Convert.ToInt32(await command.ExecuteScalarAsync());
                    }

                    // Insertar los detalles de la compra
                    foreach (var detalle in compra.Detalles)
                    {
                        detalle.CompraId = compra.Id;
                        detalle.Fecha = compra.Fecha;
                        
                        using (var command = new MySqlCommand(
                            "INSERT INTO detalle_compra (compra_id, producto_id, cantidad, valor, fecha) " +
                            "VALUES (@CompraId, @ProductoId, @Cantidad, @Valor, @Fecha)",
                            dbContext.Connection))
                        {
                            command.Transaction = transaction;
                            command.Parameters.AddWithValue("@CompraId", detalle.CompraId);
                            command.Parameters.AddWithValue("@ProductoId", detalle.ProductoId);
                            command.Parameters.AddWithValue("@Cantidad", detalle.Cantidad);
                            command.Parameters.AddWithValue("@Valor", detalle.Valor);
                            command.Parameters.AddWithValue("@Fecha", detalle.Fecha);
                            
                            await command.ExecuteNonQueryAsync();
                        }

                        // Actualizar el stock del producto
                        await _productoRepository.ActualizarStockAsync(Convert.ToInt32(detalle.ProductoId), detalle.Cantidad);
                        
                        // Registrar el movimiento de inventario
                        var movimientoRepository = new MovimientoRepository();
                        await movimientoRepository.InsertAsync(new Movimiento
                        {
                            ProductoId = detalle.ProductoId,
                            Fecha = compra.Fecha,
                            TipoMovimiento = "ENTRADA",
                            Cantidad = detalle.Cantidad,
                            Referencia = $"Compra #{compra.Id}",
                            Observacion = $"Compra a {compra.Proveedor?.NombreCompleto}"
                        });
                    }

                    // Confirmar la transacción
                    await transaction.CommitAsync();
                    return true;
                }
                catch (Exception)
                {
                    // Revertir la transacción en caso de error
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<bool> UpdateAsync(Compra compra)
        {
            // En este caso, no permitimos actualizar compras para mantener la integridad
            // de los movimientos de inventario y el stock
            throw new NotImplementedException("La actualización de compras no está permitida para mantener la integridad del inventario.");
        }

        public async Task<bool> DeleteAsync(object id)
        {
            // En este caso, no permitimos eliminar compras para mantener la integridad
            // de los movimientos de inventario y el stock
            throw new NotImplementedException("La eliminación de compras no está permitida para mantener la integridad del inventario.");
        }
    }