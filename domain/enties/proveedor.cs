using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sgi_App.domain.enties
{
    public class Proveedor
    {
       public int Id {get; set;}
    public int Tercero_id {get; set;}
    public int Dcto_double {get; set;}
    public int DiaPago {get; set;}
    public Tercero? Tercero { get; set; }
        
    public Tercero? Tercero { get; set; }
        
    public override string ToString()
    {
        return $"ID: {Id}, TerceroID: {TerceroId}, Descuento: {Descuento:P}, Día pago: {DiaPago}";
    }
    }
}