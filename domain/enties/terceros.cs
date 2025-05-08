using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sgi_App.domain.enties
{
    public class Terceros
    {
         public int Id {get; set;}
    public string? Nombre {get; set;}
    public string? Apellido {get; set;}
    public string? Email {get; set;}
    public int Tipodoc_id {get; set;}
    public int TipoTercero_id {get; set;}
    public int Ciudad_id {get; set;}
    }
}
