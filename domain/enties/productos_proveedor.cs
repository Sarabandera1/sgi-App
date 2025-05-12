using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sgi_App.domain.enties
{
    public class Productos_proveedor
    {
        public int Id {get; set;}
    public int Tercero_id {get; set;}
    public int Productos_id {get; set;}
    public override string ToString()
        {
            return $"ID: {Id}, Nombre: {Nombre}, Stock: {Stock}, Código: {CodigoBarra}";
        }
    } 
}