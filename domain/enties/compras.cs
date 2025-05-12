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


        public Tercero? Proveedor { get; set; }
        public Tercero? Empleado { get; set; }
        public List<DetalleCompra> Detalles { get; set; } = new List<DetalleCompra>();
        
        public decimal Total => Detalles.Sum(d => d.Valor * d.Cantidad);
        
        public override string ToString()
        {
            return $"ID: {Id}, Fecha: {Fecha:d}, Doc: {DocCompra}, Total: {Total:C}";
        }
   
    }
}