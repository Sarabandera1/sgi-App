using System;
using sgi-App.Domain.Entities;
using sgi-App.Domain.Ports;
using sgi-App.Infrastructure.Mysql;
using sgi-Appl.Data.MySqlClient;

namespace sgi_App.Infrastructure.Repositorios;

public class lmpClienteRepository : IGenericRepository<Cliente>, IClienteRepository
{
    private readonly Conexionmysql _conexion;

    public lmpClienteRepository(string connectionString)
    {
        _conexion = Conexionmysql.Instancia(connectionString);
    }

    public List<Cliente> ObtenerTodos()
    {
        var clientes = new List<Cliente>();
        var connection = _conexion.ObtenerConexion();

        string query = "SELECT id, nombre FROM clientes";
        using var cmd = new MySqlCommand(query, connection);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            clientes.Add(new Cliente
            {
                Id = reader.GetInt32("id"),
                Nombre = reader.GetString("nombre")
            });
        }

        return clientes;
    }

    public void Crear(Cliente cliente)
    {
        var connection = _conexion.ObtenerConexion();
        string query = "INSERT INTO clientes (nombre) VALUES (@nombre)";
        using var cmd = new MySqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@nombre", cliente.Nombre);
        cmd.ExecuteNonQuery();
    }

    public void Actualizar(Cliente cliente)
    {
        var connection = _conexion.ObtenerConexion();
        string query = "UPDATE clientes SET nombre = @nombre WHERE id = @id";
        using var cmd = new MySqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@nombre", cliente.Nombre);
        cmd.Parameters.AddWithValue("@id", cliente.Id);
        cmd.ExecuteNonQuery();
    }

    public void Eliminar(int id)
    {
        var connection = _conexion.ObtenerConexion();
        string query = "DELETE FROM clientes WHERE id = @id";
        using var cmd = new MySqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.ExecuteNonQuery();
    }
}