using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sgi-App.infrastructure.repositorios
 public class Repomovimientos : IRepository<Movimiento>
    {
        private readonly Repoproductos _repoproductos;

        public Repomovimientos()
        {
            _repoproductos = new Repoproductos();
        }

        public async Task<IEnumerable<Movimiento>> GetAllAsync()
        {
            List<Movimiento> movimientos = new List<Movimiento>();
            
            using (var dbContext = new DbContext())
            {
                using var command = new MySqlCommand(
                    "SELECT m.*, p.nombre as producto_nombre " +
                    "FROM movimiento m " +
                    "LEFT JOIN producto p ON m.producto_id = p.id " +
                    "ORDER BY m.fecha DESC",
                    dbContext.Connection);
                
                using var reader = await command.ExecuteReaderAsync();
                
                while (await reader.ReadAsync())
                {
                    var movimiento = new Movimiento
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        ProductoId = reader["producto_id"].ToString()!,
                        Fecha = Convert.ToDateTime(reader["fecha"]),
                        TipoMovimiento = reader["tipo_movimiento"].ToString()!,
                        Cantidad = Convert.ToInt32(reader["cantidad"]),
                        Referencia = reader["referencia"].ToString(),
                        Observacion = reader["observacion"].ToString()
                    };

                    // Cargar datos del producto
                    movimiento.Producto = new Producto
                    {
                        Id = Convert.ToInt32(movimiento.ProductoId),
                        Nombre = reader["producto_nombre"].ToString()!
                    };

                    movimientos.Add(movimiento);
                }
            }
            
            return movimientos;
        }

        public async Task<IEnumerable<Movimiento>> GetByProductoIdAsync(int productoId)
        {
            List<Movimiento> movimientos = new List<Movimiento>();
            
            using (var dbContext = new DbContext())
            {
                using var command = new MySqlCommand(
                    "SELECT m.*, p.nombre as producto_nombre " +
                    "FROM movimiento m " +
                    "LEFT JOIN producto p ON m.producto_id = p.id " +
                    "WHERE m.producto_id = @ProductoId " +
                    "ORDER BY m.fecha DESC",
                    dbContext.Connection);
                
                command.Parameters.AddWithValue("@ProductoId", productoId);
                
                using var reader = await command.ExecuteReaderAsync();
                
                while (await reader.ReadAsync())
                {
                    var movimiento = new Movimiento
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        ProductoId = reader["producto_id"].ToString()!,
                        Fecha = Convert.ToDateTime(reader["fecha"]),
                        TipoMovimiento = reader["tipo_movimiento"].ToString()!,
                        Cantidad = Convert.ToInt32(reader["cantidad"]),
                        Referencia = reader["referencia"].ToString(),
                        Observacion = reader["observacion"].ToString()
                    };

                    // Cargar datos del producto
                    movimiento.Producto = new Producto
                    {
                        Id = Convert.ToInt32(movimiento.ProductoId),
                        Nombre = reader["producto_nombre"].ToString()!
                    };

                    movimientos.Add(movimiento);
                }
            }
            
            return movimientos;
        }

        public async Task<Movimiento?> GetByIdAsync(object id)
        {
            Movimiento? movimiento = null;
            
            using (var dbContext = new DbContext())
            {
                using var command = new MySqlCommand(
                    "SELECT m.*, p.nombre as producto_nombre " +
                    "FROM movimiento m " +
                    "LEFT JOIN producto p ON m.producto_id = p.id " +
                    "WHERE m.id = @Id",
                    dbContext.Connection);
                
                command.Parameters.AddWithValue("@Id", id);
                
                using var reader = await command.ExecuteReaderAsync();
                
                if (await reader.ReadAsync())
                {
                    movimiento = new Movimiento
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        ProductoId = reader["producto_id"].ToString()!,
                        Fecha = Convert.ToDateTime(reader["fecha"]),
                        TipoMovimiento = reader["tipo_movimiento"].ToString()!,
                        Cantidad = Convert.ToInt32(reader["cantidad"]),
                        Referencia = reader["referencia"].ToString(),
                        Observacion = reader["observacion"].ToString()
                    };

                    // Cargar datos del producto
                    movimiento.Producto = new Producto
                    {
                        Id = Convert.ToInt32(movimiento.ProductoId),
                        Nombre = reader["producto_nombre"].ToString()!
                    };
                }
            }
            
            return movimiento;
        }

        public async Task<bool> InsertAsync(Movimiento movimiento)
        {
            using (var dbContext = new DbContext())
            {
                using var command = new MySqlCommand(
                    "INSERT INTO movimiento (producto_id, fecha, tipo_movimiento, cantidad, referencia, observacion) " +
                    "VALUES (@ProductoId, @Fecha, @TipoMovimiento, @Cantidad, @Referencia, @Observacion)",
                    dbContext.Connection);
                
                command.Parameters.AddWithValue("@ProductoId", movimiento.ProductoId);
                command.Parameters.AddWithValue("@Fecha", movimiento.Fecha);
                command.Parameters.AddWithValue("@TipoMovimiento", movimiento.TipoMovimiento);
                command.Parameters.AddWithValue("@Cantidad", movimiento.Cantidad);
                command.Parameters.AddWithValue("@Referencia", movimiento.Referencia ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Observacion", movimiento.Observacion ?? (object)DBNull.Value);
                
                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<bool> UpdateAsync(Movimiento movimiento)
        {
            // En este caso, no permitimos actualizar movimientos para mantener la integridad del inventario
            throw new NotImplementedException("La actualización de movimientos no está permitida para mantener la integridad del inventario.");
        }

        public async Task<bool> DeleteAsync(object id)
        {
            // En este caso, no permitimos eliminar movimientos para mantener la integridad del inventario
            throw new NotImplementedException("La eliminación de movimientos no está permitida para mantener la integridad del inventario.");
        }
    }