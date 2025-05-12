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

    public Producto? Producto { get; set; }
        
    public decimal Subtotal => Valor * Cantidad;
        
    public override string ToString()
    {
        return $"ID: {Id}, Producto: {ProductoId}, Cantidad: {Cantidad}, Valor: {Valor:C}, Subtotal: {Subtotal:C}";
    }
    }
}