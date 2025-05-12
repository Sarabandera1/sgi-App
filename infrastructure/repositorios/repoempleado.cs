using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sgi-App.infrastructure.repositorios
  public class EmpleadoRepository : IRepository<Empleado>
    {
        private readonly TerceroRepository _terceroRepository;

        public EmpleadoRepository()
        {
            _terceroRepository = new TerceroRepository();
        }

        public async Task<IEnumerable<Empleado>> GetAllAsync()
        {
            List<Empleado> empleados = new List<Empleado>();
            
            using (var dbContext = new DbContext())
            {
                using var command = new MySqlCommand(
                    "SELECT e.*, t.nombre, t.apellidos " +
                    "FROM empleado e " +
                    "LEFT JOIN tercero t ON e.tercero_id = t.id",
                    dbContext.Connection);
                
                using var reader = await command.ExecuteReaderAsync();
                
                while (await reader.ReadAsync())
                {
                    var empleado = new Empleado
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        TerceroId = reader["tercero_id"].ToString()!,
                        Cargo = reader["cargo"].ToString()!,
                        FechaContratacion = Convert.ToDateTime(reader["fecha_contratacion"]),
                        Salario = Convert.ToDecimal(reader["salario"])
                    };

                    // Cargar datos del tercero
                    empleado.Tercero = new Tercero
                    {
                        Id = empleado.TerceroId,
                        Nombre = reader["nombre"].ToString()!,
                        Apellidos = reader["apellidos"].ToString()!
                    };

                    empleados.Add(empleado);
                }
            }
            
            return empleados;
        }

        public async Task<Empleado?> GetByIdAsync(object id)
        {
            Empleado? empleado = null;
            
            using (var dbContext = new DbContext())
            {
                using var command = new MySqlCommand(
                    "SELECT e.*, t.nombre, t.apellidos " +
                    "FROM empleado e " +
                    "LEFT JOIN tercero t ON e.tercero_id = t.id " +
                    "WHERE e.id = @Id",
                    dbContext.Connection);
                
                command.Parameters.AddWithValue("@Id", id);
                
                using var reader = await command.ExecuteReaderAsync();
                
                if (await reader.ReadAsync())
                {
                    empleado = new Empleado
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        TerceroId = reader["tercero_id"].ToString()!,
                        Cargo = reader["cargo"].ToString()!,
                        FechaContratacion = Convert.ToDateTime(reader["fecha_contratacion"]),
                        Salario = Convert.ToDecimal(reader["salario"])
                    };

                    // Cargar datos del tercero
                    empleado.Tercero = new Tercero
                    {
                        Id = empleado.TerceroId,
                        Nombre = reader["nombre"].ToString()!,
                        Apellidos = reader["apellidos"].ToString()!
                    };
                }
            }
            
            return empleado;
        }

        public async Task<bool> InsertAsync(Empleado empleado)
        {
            using (var dbContext = new DbContext())
            {
                using var command = new MySqlCommand(
                    "INSERT INTO empleado (tercero_id, cargo, fecha_contratacion, salario) " +
                    "VALUES (@TerceroId, @Cargo, @FechaContratacion, @Salario)",
                    dbContext.Connection);
                
                command.Parameters.AddWithValue("@TerceroId", empleado.TerceroId);
                command.Parameters.AddWithValue("@Cargo", empleado.Cargo);
                command.Parameters.AddWithValue("@FechaContratacion", empleado.FechaContratacion);
                command.Parameters.AddWithValue("@Salario", empleado.Salario);
                
                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<bool> UpdateAsync(Empleado empleado)
        {
            using (var dbContext = new DbContext())
            {
                using var command = new MySqlCommand(
                    "UPDATE empleado SET tercero_id = @TerceroId, cargo = @Cargo, " +
                    "fecha_contratacion = @FechaContratacion, salario = @Salario WHERE id = @Id",
                    dbContext.Connection);
                
                command.Parameters.AddWithValue("@Id", empleado.Id);
                command.Parameters.AddWithValue("@TerceroId", empleado.TerceroId);
                command.Parameters.AddWithValue("@Cargo", empleado.Cargo);
                command.Parameters.AddWithValue("@FechaContratacion", empleado.FechaContratacion);
                command.Parameters.AddWithValue("@Salario", empleado.Salario);
                
                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<bool> DeleteAsync(object id)
        {
            using (var dbContext = new DbContext())
            {
                using var command = new MySqlCommand("DELETE FROM empleado WHERE id = @Id", dbContext.Connection);
                command.Parameters.AddWithValue("@Id", id);
                
                return await command.ExecuteNonQueryAsync() > 0;
            }
        }
    }
