using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sgi_App.domain.enties
{
    public class Detalle_venta
    {
          public int Id {get; set;}
    public string? Fecha {get; set;}
    public int Productos_id {get; set;}
    public int Cantidad {get; set;}
    public float Valor {get; set;}
    public int Compra_id  {get; set;}
    }
}