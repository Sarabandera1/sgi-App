using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sgi-App.infrastructure.repositorios
    public class Repoterceros : IRepository<Tercero>
    {
        public async Task<IEnumerable<Tercero>> GetAllAsync()
        {
            List<Tercero> terceros = new List<Tercero>();
            
            using (var dbContext = new DbContext())
            {
                using var command = new MySqlCommand("SELECT * FROM tercero", dbContext.Connection);
                using var reader = await command.ExecuteReaderAsync();
                
                while (await reader.ReadAsync())
                {
                    var tercero = new Tercero
                    {
                        Id = reader["id"].ToString()!,
                        TipoDocumento = reader["tipo_documento"].ToString()!,
                        Nombre = reader["nombre"].ToString()!,
                        Apellidos = reader["apellidos"].ToString()!,
                        Direccion = reader["direccion"].ToString(),
                        Telefono = reader["telefono"].ToString(),
                        Email = reader["email"].ToString(),
                        FechaRegistro = Convert.ToDateTime(reader["fecha_registro"])
                    };

                    terceros.Add(tercero);
                }
            }
            
            return terceros;
        }

        public async Task<Tercero?> GetByIdAsync(object id)
        {
            Tercero? tercero = null;
            
            using (var dbContext = new DbContext())
            {
                using var command = new MySqlCommand("SELECT * FROM tercero WHERE id = @Id", dbContext.Connection);
                command.Parameters.AddWithValue("@Id", id);
                
                using var reader = await command.ExecuteReaderAsync();
                
                if (await reader.ReadAsync())
                {
                    tercero = new Tercero
                    {
                        Id = reader["id"].ToString()!,
                        TipoDocumento = reader["tipo_documento"].ToString()!,
                        Nombre = reader["nombre"].ToString()!,
                        Apellidos = reader["apellidos"].ToString()!,
                        Direccion = reader["direccion"].ToString(),
                        Telefono = reader["telefono"].ToString(),
                        Email = reader["email"].ToString(),
                        FechaRegistro = Convert.ToDateTime(reader["fecha_registro"])
                    };
                }
            }
            
            return tercero;
        }

        public async Task<bool> InsertAsync(Tercero tercero)
        {
            using (var dbContext = new DbContext())
            {
                using var command = new MySqlCommand(
                    "INSERT INTO tercero (id, tipo_documento, nombre, apellidos, direccion, telefono, email, fecha_registro) " +
                    "VALUES (@Id, @TipoDocumento, @Nombre, @Apellidos, @Direccion, @Telefono, @Email, @FechaRegistro)",
                    dbContext.Connection);
                
                command.Parameters.AddWithValue("@Id", tercero.Id);
                command.Parameters.AddWithValue("@TipoDocumento", tercero.TipoDocumento);
                command.Parameters.AddWithValue("@Nombre", tercero.Nombre);
                command.Parameters.AddWithValue("@Apellidos", tercero.Apellidos);
                command.Parameters.AddWithValue("@Direccion", tercero.Direccion ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Telefono", tercero.Telefono ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Email", tercero.Email ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@FechaRegistro", tercero.FechaRegistro);
                
                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<bool> UpdateAsync(Tercero tercero)
        {
            using (var dbContext = new DbContext())
            {
                using var command = new MySqlCommand(
                    "UPDATE tercero SET tipo_documento = @TipoDocumento, nombre = @Nombre, " +
                    "apellidos = @Apellidos, direccion = @Direccion, telefono = @Telefono, " +
                    "email = @Email WHERE id = @Id",
                    dbContext.Connection);
                
                command.Parameters.AddWithValue("@Id", tercero.Id);
                command.Parameters.AddWithValue("@TipoDocumento", tercero.TipoDocumento);
                command.Parameters.AddWithValue("@Nombre", tercero.Nombre);
                command.Parameters.AddWithValue("@Apellidos", tercero.Apellidos);
                command.Parameters.AddWithValue("@Direccion", tercero.Direccion ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Telefono", tercero.Telefono ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Email", tercero.Email ?? (object)DBNull.Value);
                
                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<bool> DeleteAsync(object id)
        {
            using (var dbContext = new DbContext())
            {
                using var command = new MySqlCommand("DELETE FROM tercero WHERE id = @Id", dbContext.Connection);
                command.Parameters.AddWithValue("@Id", id);
                
                return await command.ExecuteNonQueryAsync() > 0;
            }
        }
    }