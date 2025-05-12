using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sgi-App.infrastructure.repositorios

    public class Repoproductos: IRepository<Producto>
    {
        public async Task<IEnumerable<Producto>> GetAllAsync()
        {
            List<Producto> productos = new List<Producto>();
            
            using (var dbContext = new DbContext())
            {
                using var command = new MySqlCommand(
                    "SELECT * FROM producto",
                    dbContext.Connection);
                
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

        public async Task<IEnumerable<Producto>> GetByNameAsync(string nombre)
        {
            List<Producto> productos = new List<Producto>();
            
            using (var dbContext = new DbContext())
            {
                using var command = new MySqlCommand(
                    "SELECT * FROM producto WHERE nombre LIKE @Nombre",
                    dbContext.Connection);
                
                command.Parameters.AddWithValue("@Nombre", $"%{nombre}%");
                
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

        public async Task<Producto?> GetByIdAsync(object id)
        {
            Producto? producto = null;
            
            using (var dbContext = new DbContext())
            {
                using var command = new MySqlCommand(
                    "SELECT * FROM producto WHERE id = @Id",
                    dbContext.Connection);
                
                command.Parameters.AddWithValue("@Id", id);
                
                using var reader = await command.ExecuteReaderAsync();
                
                if (await reader.ReadAsync())
                {
                    producto = new Producto
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        Nombre = reader["nombre"].ToString()!,
                        Descripcion = reader["descripcion"].ToString(),
                        Precio = Convert.ToDecimal(reader["precio"]),
                        Stock = Convert.ToInt32(reader["stock"]),
                        Imagen = reader["imagen"] != DBNull.Value ? (byte[])reader["imagen"] : null
                    };
                }
            }
            
            return producto;
        }

        public async Task<bool> InsertAsync(Producto producto)
        {
            using (var dbContext = new DbContext())
            {
                using var command = new MySqlCommand(
                    "INSERT INTO producto (nombre, descripcion, precio, stock, imagen) " +
                    "VALUES (@Nombre, @Descripcion, @Precio, @Stock, @Imagen)",
                    dbContext.Connection);
                
                command.Parameters.AddWithValue("@Nombre", producto.Nombre);
                command.Parameters.AddWithValue("@Descripcion", producto.Descripcion ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Precio", producto.Precio);
                command.Parameters.AddWithValue("@Stock", producto.Stock);
                command.Parameters.AddWithValue("@Imagen", producto.Imagen ?? (object)DBNull.Value);
                
                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<bool> UpdateAsync(Producto producto)
        {
            using (var dbContext = new DbContext())
            {
                using var command = new MySqlCommand(
                    "UPDATE producto SET nombre = @Nombre, descripcion = @Descripcion, " +
                    "precio = @Precio, stock = @Stock, imagen = @Imagen WHERE id = @Id",
                    dbContext.Connection);
                
                command.Parameters.AddWithValue("@Id", producto.Id);
                command.Parameters.AddWithValue("@Nombre", producto.Nombre);
                command.Parameters.AddWithValue("@Descripcion", producto.Descripcion ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Precio", producto.Precio);
                command.Parameters.AddWithValue("@Stock", producto.Stock);
                command.Parameters.AddWithValue("@Imagen", producto.Imagen ?? (object)DBNull.Value);
                
                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<bool> DeleteAsync(object id)
        {
            using (var dbContext = new DbContext())
            {
                using var command = new MySqlCommand("DELETE FROM producto WHERE id = @Id", dbContext.Connection);
                command.Parameters.AddWithValue("@Id", id);
                
                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<bool> ActualizarStockAsync(int productoId, int cantidad)
        {
            using (var dbContext = new DbContext())
            {
                using var command = new MySqlCommand(
                    "UPDATE producto SET stock = stock + @Cantidad WHERE id = @Id",
                    dbContext.Connection);
                
                command.Parameters.AddWithValue("@Id", productoId);
                command.Parameters.AddWithValue("@Cantidad", cantidad);
                
                return await command.ExecuteNonQueryAsync() > 0;
            }
        }
    }