using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sgi_App.domain.enties
{
    public class Cliente
    {
            public int Id {get; set;}
    public string? Tercero_id {get; set;}
    public int Fecha_id {get; set;}
    public int FechaCompra {get; set;}
        
        public override string ToString()
        {
            return $"ID: {Id}, TerceroID: {TerceroId}, Última compra: {FechaCompra?.ToString("d") ?? "Sin compras"}";
        }
    }
}