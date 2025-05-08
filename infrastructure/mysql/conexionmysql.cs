using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sgi_App.infrastructure.mysql
{
    internal class Conexionmysql : conexion
    {
       private MysqlConnection connection; 
       private string cadenaConexion;
       public Conexionmysql() 
       {
        cadenaConexion = "Database=" + Database +
        "; DataSource=" server +
        "; User Id= " + user +
        "; Password=" + Password;

        connection = new MysqlConnection(cadenaConexion);

       }
       public MySqlConnection GetConnection()
       {
            try
            {
                if (connection.State != System.Data.ConnectionState.Open)
                {
                    connection.Open();
                }
            catch (Exception e)
            {
                MessageBox.Show(e.Tostring());
            }
            return connection;
        }
       }
    }
}