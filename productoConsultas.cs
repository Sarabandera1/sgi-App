using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sgi_App.infrastructure.mysql;

namespace sgi_App
{
    public class ProductoConsultas
    {
       private Conexionmysql ConexionMysql;
       public ProductoConsultas()
       {
        ConexionMysql = new Conexionmysql();
       } 
    }
}