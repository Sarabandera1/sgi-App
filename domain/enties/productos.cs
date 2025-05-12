using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sgi_App.domain.enties
{
    public class Productos
    {
        public int Id {get; set;}
    public string? nombre {get; set;}
    public int idstock {get; set;}
    public int idstockMin {get; set;}
    public int idstockMas {get; set;}
    public  string? barcodde  {get; set;}

    public override string ToString()
    {
        return $"ID: {Id}, Nombre: {Nombre}, Stock: {Stock}, Código: {CodigoBarra}";
    }
    }
}