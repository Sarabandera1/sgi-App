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
    }
}