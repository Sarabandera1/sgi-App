using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sgi-App.infrastructure.repositorios
    public class VentaRepository : IRepository<Venta>
    {
        private readonly TerceroRepository _terceroRepository;
        private readonly ProductoRepository _productoRepository;

        public VentaRepository()
        {
            _terceroRepository = new TerceroRepository();
            _productoRepository = new ProductoRepository();
        }

        public async Task<IEnumerable<Venta>> GetAllAsync()
        {
            List<Venta> ventas = new List<Venta>();
            
            using (var dbContext = new DbContext())
            {
                using var command = new MySqlCommand(
                    "SELECT v.*, " +
                    "tc.nombre as cliente_nombre, tc.apellidos as cliente_apellidos, " +
                    "te.nombre as empleado_nombre, te.apellidos as empleado_apellidos " +
                    "FROM venta v " +
                    "LEFT JOIN tercero tc ON v.tercero_cliente_id = tc.id " +
                    "LEFT JOIN tercero te ON v.tercero_empleado_id = te.id " +
                    "ORDER BY v.fecha DESC",
                    dbContext.Connection);
                
                using var reader = await command.ExecuteReaderAsync();
                
                while (await reader.ReadAsync())
                {
                    var venta = new Venta
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        TerceroClienteId = reader["tercero_cliente_id"].ToString()!,
                        TerceroEmpleadoId = reader["tercero_empleado_id"].ToString()!,
                        Fecha = Convert.ToDateTime(reader["fecha"]),
                        DocVenta = reader["doc_venta"].ToString()!,
                        Total = Convert.ToDecimal(reader["total"]),
                        Ganancia = Convert.ToDecimal(reader["ganancia"]),
                        Detalles = new List<DetalleVenta>()
                    };

                    // Cargar datos del cliente
                    venta.Cliente = new Tercero
                    {
                        Id = venta.TerceroClienteId,
                        Nombre = reader["cliente_nombre"].ToString()!,
                        Apellidos = reader["cliente_apellidos"].ToString()!
                    };

                    // Cargar datos del empleado
                    venta.Empleado = new Tercero
                    {
                        Id = venta.TerceroEmpleadoId,
                        Nombre = reader["empleado_nombre"].ToString()!,
                        Apellidos = reader["empleado_apellidos"].ToString()!
                    };

                    ventas.Add(venta);
                }
            }
            
            // Cargar detalles para cada venta
            foreach (var venta in ventas)
            {
                venta.Detalles = (List<DetalleVenta>)await GetDetallesVentaAsync(venta.Id);
            }
            
            return ventas;
        }

        public async Task<Venta?> GetByIdAsync(object id)
        {
            Venta? venta = null;
            
            using (var dbContext = new DbContext())
            {
                using var command = new MySqlCommand(
                    "SELECT v.*, " +
                    "tc.nombre as cliente_nombre, tc.apellidos as cliente_apellidos, " +
                    "te.nombre as empleado_nombre, te.apellidos as empleado_apellidos " +
                    "FROM venta v " +
                    "LEFT JOIN tercero tc ON v.tercero_cliente_id = tc.id " +
                    "LEFT JOIN tercero te ON v.tercero_empleado_id = te.id " +
                    "WHERE v.id = @Id",
                    dbContext.Connection);
                
                command.Parameters.AddWithValue("@Id", id);
                
                using var reader = await command.ExecuteReaderAsync();
                
                if (await reader.ReadAsync())
                {
                    venta = new Venta
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        TerceroClienteId = reader["tercero_cliente_id"].ToString()!,
                        TerceroEmpleadoId = reader["tercero_empleado_id"].ToString()!,
                        Fecha = Convert.ToDateTime(reader["fecha"]),
                        DocVenta = reader["doc_venta"].ToString()!,
                        Total = Convert.ToDecimal(reader["total"]),
                        Ganancia = Convert.ToDecimal(reader["ganancia"]),
                        Detalles = new List<DetalleVenta>()
                    };

                    // Cargar datos del cliente
                    venta.Cliente = new Tercero
                    {
                        Id = venta.TerceroClienteId,
                        Nombre = reader["cliente_nombre"].ToString()!,
                        Apellidos = reader["cliente_apellidos"].ToString()!
                    };

                    // Cargar datos del empleado
                    venta.Empleado = new Tercero
                    {
                        Id = venta.TerceroEmpleadoId,
                        Nombre = reader["empleado_nombre"].ToString()!,
                        Apellidos = reader["empleado_apellidos"].ToString()!
                    };
                }
            }
            
            // Cargar detalles de la venta
            if (venta != null)
            {
                venta.Detalles = (List<DetalleVenta>)await GetDetallesVentaAsync(venta.Id);
            }
            
            return venta;
        }

        public async Task<IEnumerable<DetalleVenta>> GetDetallesVentaAsync(int ventaId)
        {
            List<DetalleVenta> detalles = new List<DetalleVenta>();
            
            using (var dbContext = new DbContext())
            {
                using var command = new MySqlCommand(
                    "SELECT dv.*, p.nombre as producto_nombre, p.precio as producto_precio " +
                    "FROM detalle_venta dv " +
                    "LEFT JOIN producto p ON dv.producto_id = p.id " +
                    "WHERE dv.venta_id = @VentaId",
                    dbContext.Connection);
                
                command.Parameters.AddWithValue("@VentaId", ventaId);
                
                using var reader = await command.ExecuteReaderAsync();
                
                while (await reader.ReadAsync())
                {
                    var detalle = new DetalleVenta
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        VentaId = ventaId,
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

        public async Task<bool> InsertAsync(Venta venta)
        {
            using (var dbContext = new DbContext())
            {
                // Iniciar transacción
                using var transaction = await dbContext.Connection.BeginTransactionAsync();
                
                try
                {
                    // Calcular total y ganancia de la venta
                    decimal total = 0;
                    decimal ganancia = 0;
                    
                    foreach (var detalle in venta.Detalles)
                    {
                        // Obtener el producto para calcular la ganancia
                        var producto = await _productoRepository.GetByIdAsync(detalle.ProductoId);
                        if (producto != null)
                        {
                            // Calcular ganancia (precio de venta - costo)
                            decimal costoUnitario = producto.Costo ?? producto.Precio * 0.7m; // Si no hay costo, estimamos 70% del precio
                            decimal gananciaUnitaria = detalle.Valor - costoUnitario;
                            ganancia += gananciaUnitaria * detalle.Cantidad;
                        }
                        
                        total += detalle.Cantidad * detalle.Valor;
                    }
                    
                    venta.Total = total;
                    venta.Ganancia = ganancia;

                    // Insertar la venta
                    using (var command = new MySqlCommand(
                        "INSERT INTO venta (tercero_cliente_id, tercero_empleado_id, fecha, doc_venta, total, ganancia) " +
                        "VALUES (@TerceroClienteId, @TerceroEmpleadoId, @Fecha, @DocVenta, @Total, @Ganancia); " +
                        "SELECT LAST_INSERT_ID();",
                        dbContext.Connection))
                    {
                        command.Transaction = transaction;
                        command.Parameters.AddWithValue("@TerceroClienteId", venta.TerceroClienteId);
                        command.Parameters.AddWithValue("@TerceroEmpleadoId", venta.TerceroEmpleadoId);
                        command.Parameters.AddWithValue("@Fecha", venta.Fecha);
                        command.Parameters.AddWithValue("@DocVenta", venta.DocVenta);
                        command.Parameters.AddWithValue("@Total", venta.Total);
                        command.Parameters.AddWithValue("@Ganancia", venta.Ganancia);
                        
                        // Obtener el ID de la venta insertada
                        venta.Id = Convert.ToInt32(await command.ExecuteScalarAsync());
                    }

                    // Insertar los detalles de la venta
                    foreach (var detalle in venta.Detalles)
                    {
                        detalle.VentaId = venta.Id;
                        detalle.Fecha = venta.Fecha;
                        
                        using (var command = new MySqlCommand(
                            "INSERT INTO detalle_venta (venta_id, producto_id, cantidad, valor, fecha) " +
                            "VALUES (@VentaId, @ProductoId, @Cantidad, @Valor, @Fecha)",
                            dbContext.Connection))
                        {
                            command.Transaction = transaction;
                            command.Parameters.AddWithValue("@VentaId", detalle.VentaId);
                            command.Parameters.AddWithValue("@ProductoId", detalle.ProductoId);
                            command.Parameters.AddWithValue("@Cantidad", detalle.Cantidad);
                            command.Parameters.AddWithValue("@Valor", detalle.Valor);
                            command.Parameters.AddWithValue("@Fecha", detalle.Fecha);
                            
                            await command.ExecuteNonQueryAsync();
                        }

                        // Actualizar el stock del producto (restar)
                        await _productoRepository.ActualizarStockAsync(Convert.ToInt32(detalle.ProductoId), -detalle.Cantidad);
                        
                        // Registrar el movimiento de inventario
                        var movimientoRepository = new MovimientoRepository();
                        await movimientoRepository.InsertAsync(new Movimiento
                        {
                            ProductoId = detalle.ProductoId,
                            Fecha = venta.Fecha,
                            TipoMovimiento = "SALIDA",
                            Cantidad = detalle.Cantidad,
                            Referencia = $"Venta #{venta.Id}",
                            Observacion = $"Venta a {venta.Cliente?.NombreCompleto}"
                        });
                    }

                    // Actualizar la fecha de última compra del cliente
                    var clienteRepository = new ClienteRepository();
                    await clienteRepository.ActualizarFechaCompraAsync(Convert.ToInt32(venta.TerceroClienteId), venta.Fecha);

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

        public async Task<bool> UpdateAsync(Venta venta)
        {
            // En este caso, no permitimos actualizar ventas para mantener la integridad
            // de los movimientos de inventario y el stock
            throw new NotImplementedException("La actualización de ventas no está permitida para mantener la integridad del inventario.");
        }

        public async Task<bool> DeleteAsync(object id)
        {
            // En este caso, no permitimos eliminar ventas para mantener la integridad
            // de los movimientos de inventario y el stock
            throw new NotImplementedException("La eliminación de ventas no está permitida para mantener la integridad del inventario.");
        }
    }