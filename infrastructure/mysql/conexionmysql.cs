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
{
    internal class Conexionmysql : conexion
    {
        private MySqlConnection connection; 
        private string cadenaConexion;

        public Conexionmysql() 
        {
            cadenaConexion = $"Server={server};Database={database};User Id={user};Password={password};";
            connection = new MySqlConnection(cadenaConexion);
        }

        public MySqlConnection GetConnection()
        {
            try
            {
                if (connection.State != System.Data.ConnectionState.Open)
                {
                    connection.Open();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

            return connection;
        }
    }
}

public bool ProbarConexion()
{
    try
    {
        using (var conn = GetConnection())
        {
            if (conn.State == System.Data.ConnectionState.Open)
            {
                MessageBox.Show("✅ Conexión exitosa a la base de datos.");
                return true;
            }
            else
            {
                MessageBox.Show("❌ No se pudo abrir la conexión.");
                return false;
            }
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show("❌ Error de conexión: " + ex.Message);
        return false;
    }
