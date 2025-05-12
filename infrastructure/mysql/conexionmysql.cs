using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;


namespace sgi_App.infrastructure.mysql

    public class conexionDingleton
    {
        private static ConexionSingleton?_instancia;
        private readonly string _connectionString;
    private MySqlConnection? _conexion;

    private ConexionSingleton(string connectionString)
    {
        _connectionString = connectionString;
    }

    public static ConexionSingleton Instancia(string connectionString)
    {
        _instancia ??= new ConexionSingleton(connectionString);
        return _instancia;
    }

    public MySqlConnection ObtenerConexion()
    {
        _conexion ??= new MySqlConnection(_connectionString);

        if (_conexion.State != System.Data.ConnectionState.Open)
            _conexion.Open();

        return _conexion;
    }
}
    