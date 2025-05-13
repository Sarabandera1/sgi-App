using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sgi_App.domain.enties;
using sgi_App.infrastructure.mysql;

namespace sgi_App
{
    // hola
    public class ProductoConsultas
    {
       private Conexionmysql ConexionMysql;
       public ProductoConsultas()
       {
        ConexionMysql = new Conexionmysql();
        mproductos = new List<Productos>
       } 

    public List<ProductoConsultas> getProductos(string filtro)
    {
        string QUERY = "SELECT * FROM producto ";
        MySqlDataReader mReader  null; 
        try
        {
            if (filtro != "")
            {
                QUERY += " WHERE " +
                    "id LIKE '%" + filtro + "%' OR " +
                    "nombre LIKE '%" + filtro + "%' OR " +
                    "precio LIKE '%" + filtro + "%' OR " +
                    "cantidad LIKE '%" + filtro + "%';";
            }

            MySqlCommand mComando = new MysqlCommand(QUERY);
            mComando.Connection = ConexionMysql.GetConnection();
            mReader = mComando.ExecuteREader();

            ProductoConsultas mProducto = null;
            while(mReader.Read())
            {
                mProducto = new Producto();
                mProducto.id = mReader.GetInt16("id");
                mProducto.nombre = mReader.GetString("nombre");
                mProducto.precio = mReader.GetInt16("Precio");
                mProducto.cantidad = mReader.GetInt16("Cantidad");
                mProducto.imagen = (byte[]) mReader.GetValue(4);
                mProductos.Add(mProducto);
            }
            mReader.Close();
        }
        catch (Exception)
        {
            
            throw;
        }
        return mProductos;
    }

    internal bool agregarProductos(ProductoConsultas mproducto)
    {
        string INSERT = "INSERT INTO producto (nombre, precio, cantidad, imagen)"
            values (@nombre,@precio,@cantidad,@imagen);";

            MysqlCommand mCommand = new MysqlCommand(INSERT, ConexionMysql.GetConnection());

            mCommand.parameters.Add(new MysqlParameter("@nombre", mproducto.nombre));
            mCommand.parameters.Add(new MysqlParameter("@precio", mproducto.precio));
            mCommand.parameters.Add(new MysqlParameter("@cantidad", mproducto.cantidad));
            mCommand.parameters.Add(new MysqlParameter("@imagen", mproducto.imagen));

            return mCommand.ExecuteNonQuery() > 0;
    }

    internal bool modificarProducto(ProductoConsultas mproducto)
    {
        string UPDATE = "UPDATE INTO producto SET" +
            "nombre=@nombre, " +
            "precio=@precio, " +
            "cantidad=@cantidad, " +
            "imagen=@imagen " +
            "WHERE id=@id;";
            
            MysqlCommand mCommand = new MysqlCommand(INSERT, ConexionMysql.GetConnection());

            mCommand.parameters.Add(new MysqlParameter("@nombre", mproducto.nombre));
            mCommand.parameters.Add(new MysqlParameter("@precio", mproducto.precio));
            mCommand.parameters.Add(new MysqlParameter("@cantidad", mproducto.cantidad));
            mCommand.parameters.Add(new MysqlParameter("@imagen", mproducto.imagen));
            mCommand.parameters.Add(new MysqlParameter("@id", mproducto.id));
            

            return mCommand.ExecuteNonQuery() > 0;
    }

     internal bool modificarProducto(ProductoConsultas mproducto)
    {
        string DELETE FROM = "UPDATE INTO producto  WHERE id=@id;";
            

            mCommand.parameters.Add(new MysqlParameter("@id", mproducto.id));
            return mCommand.ExecuteNonQuery() > 0;
    }
}
}