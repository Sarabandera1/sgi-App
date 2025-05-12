using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sgi_App.domain.enties
{
    public class Venta
    {
       public int Id {get; set;}
    public int  fact_id {get; set;}
    public int fecha {get; set;}
    public int terceroEM_id {get; set;}
    public  int TerceroCli_id  {get; set;}

    public Tercero? Empleado { get; set; }
    public Tercero? Cliente { get; set; }
    public List<DetalleVenta> Detalles { get; set; } = new List<DetalleVenta>();
        
    public decimal Total => Detalles.Sum(d => d.Valor * d.Cantidad);
        
    public override string ToString()
    {
        return $"Factura: {FacturaId}, Fecha: {Fecha:d}, Total: {Total:C}";
    }
    }
}