using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sgi_App.domain.enties
{
    public class MovCasa
    {
          public int Id {get; set;}
    public int Fecha {get; set;}
    public int TipoMov_id {get; set;}
    public float Valor {get; set;}
    public string? Concepto {get; set;} 
    public int Tercero_id {get; set;}
    }
}