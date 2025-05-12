using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sgi_App.domain.enties
{
    public class Planes
    {
        public int Id {get; set;}
    public string? Nombre {get; set;}
    public int FechaInicio {get; set;}
    public int FechaFin {get; set;}
    public int Documento {get; set;} 

    public List<Producto> Productos { get; set; } = new List<Producto>();

    public bool Vigente()
    {
        DateTime hoy = DateTime.Today;
        return hoy >= FechaInicio && hoy <= FechaFin;
    }

    public override string ToString()
        {
            return $"ID: {Id}, Nombre: {Nombre}, Descuento: {Descuento:P}, Vigencia: {FechaInicio:d} - {FechaFin:d}";
        }
    }
}