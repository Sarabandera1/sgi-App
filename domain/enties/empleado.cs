using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sgi_App.domain.enties
{
    public class Empleado
    {
              public int Id {get; set;}
    public int Tercero_id {get; set;}
    public int FechaIngreso {get; set;}
    public float SalarioBase {get; set;}
    public int Eps_id {get; set;}
    public int Compra_id  {get; set;}

    public Tercero? Tercero { get; set; }
        
    public override string ToString()
    {
        return $"ID: {Id}, TerceroID: {TerceroId}, Fecha ingreso: {FechaIngreso:d}, Salario: {SalarioBase:C}";
    }
    }
}