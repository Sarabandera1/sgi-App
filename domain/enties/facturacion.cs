using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sgi_App.domain.enties
{
    public class Facturacion
    {
          public int Id {get; set;}
    public int FechaResolucion {get; set;}
    public int NumInicio {get; set;}
    public int NumFinal {get; set;}
    public int FactActual {get; set;}
    }
}