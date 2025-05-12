using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sgi-App.infrastructure.repositorios
 public class repoplanes : IRepository<Plan>
    {
        private readonly Repoproductos _repoproductos;

        public PlanRepository()
        {
            _repoproductos = new Repoproductos();
        }

        public async Task<IEnumerable<Plan>> GetAllAsync()
        {
            List<Plan> planes = new List<Plan>();
            
            using (var dbContext = new DbContext())
            {
                using var command = new MySqlCommand(
                    "SELECT * FROM plan ORDER BY fecha_inicio DESC",
                    dbContext.Connection);
                
                using var reader = await command.ExecuteReaderAsync();
                
                while (await reader.ReadAsync())
                {
                    var plan = new Plan
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        Nombre = reader["nombre"].ToString()!,
                        FechaInicio = Convert.ToDateTime(reader["fecha_inicio"]),
                        FechaFin = Convert.ToDateTime(reader["fecha_fin"]),
                        Descuento = Convert.ToDecimal(reader["descuento"]),
                        Productos = new List<Producto>()
                    };

                    planes.Add(plan);
                }
            }
            
            // Cargar productos para cada plan
            foreach (var plan in planes)
            {
                plan.Productos = (List<Producto>)await GetProductosPlanAsync(plan.Id);
            }
            
            return planes;
        }

        public async Task<IEnumerable<Plan>> GetPlanesVigentesAsync()
        {
            List<Plan> planes = new List<Plan>();
            var fechaActual = DateTime.Now.Date;
            
            using (var dbContext = new DbContext())
            {
                using var command = new MySqlCommand(
                    "SELECT * FROM plan " +
                    "WHERE fecha_inicio <= @FechaActual AND fecha_fin >= @FechaActual " +
                    "ORDER BY fecha_inicio DESC",
                    dbContext.Connection);
                
                command.Parameters.AddWithValue("@FechaActual", fechaActual);
                
                using var reader = await command.ExecuteReaderAsync();
                
                while (await reader.ReadAsync())
                {
                    var plan = new Plan
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        Nombre = reader["nombre"].ToString()!,
                        FechaInicio = Convert.ToDateTime(reader["fecha_inicio"]),
                        FechaFin = Convert.ToDateTime(reader["fecha_fin"]),
                        Descuento = Convert.ToDecimal(reader["descuento"]),
                        Productos = new List<Producto>()
                    };

                    planes.Add(plan);
                }
            }
            
            // Cargar productos para cada plan
            foreach (var plan in planes)
            {
                plan.Productos = (List<Producto>)await GetProductosPlanAsync(plan.Id);
            }
            
            return planes;
        }

        public async Task<Plan?> GetByIdAsync(object id)
        {
            Plan? plan = null;
            
            using (var dbContext = new DbContext())
            {
                using var command = new MySqlCommand(
                    "SELECT * FROM plan WHERE id = @Id",
                    dbContext.Connection);
                
                command.Parameters.AddWithValue("@Id", id);
                
                using var reader = await command.ExecuteReaderAsync();
                
                if (await reader.ReadAsync())
                {
                    plan = new Plan
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        Nombre = reader["nombre"].ToString()!,
                        FechaInicio = Convert.ToDateTime(reader["fecha_inicio"]),
                        FechaFin = Convert.ToDateTime(reader["fecha_fin"]),
                        Descuento = Convert.ToDecimal(reader["descuento"]),
                        Productos = new List<Producto>()
                    };
                }
            }
            
            // Cargar productos del plan
            if (plan != null)
            {
                plan.Productos = (List<Producto>)await GetProductosPlanAsync(plan.Id);
            }
            
            return plan;
        }

        public async Task<IEnumerable<Producto>> GetProductosPlanAsync(int planId)
        {
            List<Producto> productos = new List<Producto>();
            
            using (var dbContext = new DbContext())
            {
                using var command = new MySqlCommand(
                    "SELECT p.* FROM producto p " +
                    "INNER JOIN plan_producto pp ON p.id = pp.producto_id " +
                    "WHERE pp.plan_id = @PlanId",
                    dbContext.Connection);
                
                command.Parameters.AddWithValue("@PlanId", planId);
                
                using var reader = await command.ExecuteReaderAsync();
                
                while (await reader.ReadAsync())
                {
                    var producto = new Producto
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        Nombre = reader["nombre"].ToString()!,
                        Descripcion = reader["descripcion"].ToString(),
                        Precio = Convert.ToDecimal(reader["precio"]),
                        Stock = Convert.ToInt32(reader["stock"]),
                        Imagen = reader["imagen"] != DBNull.Value ? (byte[])reader["imagen"] : null
                    };

                    productos.Add(producto);
                }
            }
            
            return productos;
        }

        public async Task<bool> InsertAsync(Plan plan)
        {
            using (var dbContext = new DbContext())
            {
                // Iniciar transacción
                using var transaction = await dbContext.Connection.BeginTransactionAsync();
                
                try
                {
                    // Insertar el plan
                    using (var command = new MySqlCommand(
                        "INSERT INTO plan (nombre, fecha_inicio, fecha_fin, descuento) " +
                        "VALUES (@Nombre, @FechaInicio, @FechaFin, @Descuento); " +
                        "SELECT LAST_INSERT_ID();",
                        dbContext.Connection))
                    {
                        command.Transaction = transaction;
                        command.Parameters.AddWithValue("@Nombre", plan.Nombre);
                        command.Parameters.AddWithValue("@FechaInicio", plan.FechaInicio);
                        command.Parameters.AddWithValue("@FechaFin", plan.FechaFin);
                        command.Parameters.AddWithValue("@Descuento", plan.Descuento);
                        
                        // Obtener el ID del plan insertado
                        plan.Id = Convert.ToInt32(await command.ExecuteScalarAsync());
                    }

                    // Insertar las relaciones plan-producto
                    foreach (var producto in plan.Productos)
                    {
                        using (var command = new MySqlCommand(
                            "INSERT INTO plan_producto (plan_id, producto_id) " +
                            "VALUES (@PlanId, @ProductoId)",
                            dbContext.Connection))
                        {
                            command.Transaction = transaction;
                            command.Parameters.AddWithValue("@PlanId", plan.Id);
                            command.Parameters.AddWithValue("@ProductoId", producto.Id);
                            
                            await command.ExecuteNonQueryAsync();
                        }
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

        public async Task<bool> UpdateAsync(Plan plan)
        {
            using (var dbContext = new DbContext())
            {
                // Iniciar transacción
                using var transaction = await dbContext.Connection.BeginTransactionAsync();
                
                try
                {
                    // Actualizar el plan
                    using (var command = new MySqlCommand(
                        "UPDATE plan SET nombre = @Nombre, fecha_inicio = @FechaInicio, " +
                        "fecha_fin = @FechaFin, descuento = @Descuento WHERE id = @Id",
                        dbContext.Connection))
                    {
                        command.Transaction = transaction;
                        command.Parameters.AddWithValue("@Id", plan.Id);
                        command.Parameters.AddWithValue("@Nombre", plan.Nombre);
                        command.Parameters.AddWithValue("@FechaInicio", plan.FechaInicio);
                        command.Parameters.AddWithValue("@FechaFin", plan.FechaFin);
                        command.Parameters.AddWithValue("@Descuento", plan.Descuento);
                        
                        await command.ExecuteNonQueryAsync();
                    }

                    // Eliminar las relaciones plan-producto existentes
                    using (var command = new MySqlCommand(
                        "DELETE FROM plan_producto WHERE plan_id = @PlanId",
                        dbContext.Connection))
                    {
                        command.Transaction = transaction;
                        command.Parameters.AddWithValue("@PlanId", plan.Id);
                        
                        await command.ExecuteNonQueryAsync();
                    }

                    // Insertar las nuevas relaciones plan-producto
                    foreach (var producto in plan.Productos)
                    {
                        using (var command = new MySqlCommand(
                            "INSERT INTO plan_producto (plan_id, producto_id) " +
                            "VALUES (@PlanId, @ProductoId)",
                            dbContext.Connection))
                        {
                            command.Transaction = transaction;
                            command.Parameters.AddWithValue("@PlanId", plan.Id);
                            command.Parameters.AddWithValue("@ProductoId", producto.Id);
                            
                            await command.ExecuteNonQueryAsync();
                        }
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

        public async Task<bool> DeleteAsync(object id)
        {
            using (var dbContext = new DbContext())
            {
                using var transaction = await dbContext.Connection.BeginTransactionAsync();
                
                try
                {
                    // Eliminar relaciones plan-producto
                    using (var command = new MySqlCommand(
                        "DELETE FROM plan_producto WHERE plan_id = @PlanId",
                        dbContext.Connection))
                    {
                        command.Transaction = transaction;
                        command.Parameters.AddWithValue("@PlanId", id);
                        
                        await command.ExecuteNonQueryAsync();
                    }

                    // Eliminar el plan
                    using (var command = new MySqlCommand(
                        "DELETE FROM plan WHERE id = @Id",
                        dbContext.Connection))
                    {
                        command.Transaction = transaction;
                        command.Parameters.AddWithValue("@Id", id);
                        
                        await command.ExecuteNonQueryAsync();
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
    }