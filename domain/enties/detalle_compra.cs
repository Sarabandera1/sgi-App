using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sgi_App.domain.enties
{
    public class Detalle_compra
    {
         public int Id {get; set;}
    public string? Producto_id {get; set;}
    public int Fecha {get; set;}
    public int Cantidad {get; set;}
    public float Valor {get; set;}
    public int Compra_id {get; set;}
    }
}