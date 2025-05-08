using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sgi_App.domain.enties
{
    public class Compras
    {
           public int Id {get; set;}
    public string? Terceroprov_id {get; set;}
    public int Fecha {get; set;}
    public int TerceroEmp_id {get; set;}
    public string? DocCompra {get; set;}
   
    }
}