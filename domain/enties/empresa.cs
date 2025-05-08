using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sgi_App.domain.enties
{
    public class Empresa
    {
          public int Id {get; set;}
    public string? Nombre {get; set;}
    public int Ciudad_id {get; set;}
    public int Fecha_reg {get; set;}
    }
}