using System;
using System.Linq;
using System.Threading.Tasks;
using sgi-App.Models;
using sgi-App.Repositorios;

namespace sgi-App.UI
{
    public class MenuCaja
    {
        private readonly Repomovimientos _repomovimientos;
        
        public MenuCaja()
        {
            _repomovimientos = new Repomovimientos();
        }
        
        public void MostrarMenu()
        {
            bool regresar = false;
            
            while (!regresar)
            {
                Console.Clear();
                MenuPrincipal.MostrarEncabezado("GESTIÓN DE CAJA");
                
                Console.Clear();
                Console.WriteLine("╔════════════════════════════════════════╗");
                Console.WriteLine("║           MENÚ DE CAJA                 ║");
                Console.WriteLine("╠════════════════════════════════════════╣");
                Console.WriteLine("║  1. Iniciar Caja                       ║");
                Console.WriteLine("║  2. Cierre de Caja                     ║");
                Console.WriteLine("║  3. Registrar Movimiento               ║");
                Console.WriteLine("║  4. Ver Movimientos por Fecha          ║");
                Console.WriteLine("║  5. Ver Saldo de Caja                  ║");
                Console.WriteLine("║  0. Regresar al Menú Principal         ║");
                Console.WriteLine("╚════════════════════════════════════════╝");
                Console.Write("Seleccione una opción: ");

                
                Console.Write("\nSeleccione una opción: ");
                string opcion = Console.ReadLine() ?? "";
                
                switch (opcion)
                {
                    case "1":
                        InicioCaja().Wait();
                        break;
                    case "2":
                        CierreCaja().Wait();
                        break;
                    case "3":
                        RegistrarMovimiento().Wait();
                        break;
                    case "4":
                        VerMovimientosPorFecha().Wait();
                        break;
                    case "5":
                        VerSaldoCaja().Wait();
                        break;
                    case "0":
                        regresar = true;
                        break;
                    default:
                        MenuPrincipal.MostrarMensaje("Opción no válida, intentelo de nuevo.", ConsoleColor.Yellow);
                        Console.ReadKey();
                        break;
                }
            }
        }
        
        private async Task InicioCaja()
        {
            Console.Clear();
            MenuPrincipal.MostrarEncabezado("Inicio de caja");
            
            try
            {
                DateTime fechaActual = DateTime.Today;
                
                // Verificar si se habilito la caja
                var movimientosDia = await _repositorioMovimientos.GetMovimientosByFechaAsync(fechaActual);
                bool yaAbierto = movimientosDia.Any(m => m.Concepto.Contains("Inicio de caja"));
                
                if (yaAbierto)
                {
                    MenuPrincipal.MostrarMensaje("\nLa caja ya fue habilitada para la jornada de hoy.", ConsoleColor.Yellow);
                }
                else
                {
                    decimal montoInicial = MenuPrincipal.LeerDecimalPositivo("\nSaldo inicial de la jornada ");
                    string terceroId = MenuPrincipal.LeerEntrada("ID del Tercero responsable: ");
                    
                    var movimiento = new MovimientoCaja
                    {
                        Fecha = fechaActual,
                        TipoMovimientoId = 1,
                        Valor = montoInicial,
                        Concepto = "Habilitar caja",
                        TerceroId = terceroId
                    };
                    
                    bool resultado = await _repositorioMovimientos.InsertAsync(movimiento);
                    
                    if (resultado)
                    {
                        MenuPrincipal.MostrarMensaje("\n Se llevo a cabo el inicio de caja con exito", ConsoleColor.Green);
                    }
                    else
                    {
                        MenuPrincipal.MostrarMensaje("\nNo se registro el registro de inicio de la caja.", ConsoleColor.Red);
                    }
                }
            }
            catch (Exception ex)
            {
                MenuPrincipal.MostrarMensaje($"\nError al registrar el inicio de caja: {ex.Message}", ConsoleColor.Red);
            }
            
            Console.Write("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
        
        private async Task CierreCaja()
        {
            Console.Clear();
            MenuPrincipal.MostrarEncabezado("CIERRE DE CAJA");
            
            try
            {
                DateTime fechaActual = DateTime.Today;
                
                // Verificar si ya hay cierre para hoy
                var movimientosDia = await _repositorioMovimientos.GetMovimientosByFechaAsync(fechaActual);
                bool yaCerrado = movimientosDia.Any(m => m.Concepto.Contains("Cierre de caja"));
                
                if (yaCerrado)
                {
                    MenuPrincipal.MostrarMensaje("\nYa se ha realizado el cierre de caja para hoy.", ConsoleColor.Yellow);
                }
                else
                {
                    // Verificar si se puede habilitar la caja
                    bool Habilitar = movimientosDia.Any(m => m.Concepto.Contains("Inicio de caja"));
                    
                    if (!Habilitar)
                    {
                        MenuPrincipal.MostrarMensaje("\n No se ha registrado nada, habilite la caja.", ConsoleColor.Yellow);
                    }
                    else
                    {
                        // Obtener saldo de la caja
                        decimal saldoCaja = await _repositorioMovimientos.GetSaldoCajaAsync(fechaActual);
                        
                        Console.WriteLine($"\nSaldo actual de caja: {saldoCaja:C}");
                        
                        // Mostrar movimientos del día
                        await MostrarMovimientosDia(fechaActual);
                        
                        decimal montoContado = MenuPrincipal.LeerDecimalPositivo("\nMonto contado físicamente: ");
                        
                        decimal diferencia = montoContado - saldoCaja;
                        if (diferencia != 0)
                        {
                            Console.WriteLine($"\nDiferencia detectada: {Math.Abs(diferencia):C} {(diferencia > 0 ? "sobrante" : "faltante")}");
                        }
                        
                        string terceroId = MenuPrincipal.LeerEntrada("ID del Tercero responsable: ");
                        
                        var movimiento = new MovimientoCaja
                        {
                            Fecha = fechaActual,
                            TipoMovimientoId = 2, // Asumir que el tipo 2 es Cierre de Caja
                            Valor = montoContado,
                            Concepto = $"Cierre de caja. Saldo sistema: {saldoCaja:C}, Diferencia: {diferencia:C}",
                            TerceroId = terceroId
                        };
                        
                        bool resultado = await _repositorioMovimientos.InsertAsync(movimiento);
                        
                        if (resultado)
                        {
                            MenuPrincipal.MostrarMensaje("\nCierre de caja registrado correctamente.", ConsoleColor.Green);
                        }
                        else
                        {
                            MenuPrincipal.MostrarMensaje("\nNo se pudo registrar el cierre de caja.", ConsoleColor.Red);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MenuPrincipal.MostrarMensaje($"\nError al registrar cierre de caja: {ex.Message}", ConsoleColor.Red);
            }
            
            Console.Write("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
        
        private async Task RegistrarMovimiento()
        {
            Console.Clear();
            MenuPrincipal.MostrarEncabezado("REGISTRAR MOVIMIENTO DE CAJA");
            
            try
            {
                Console.WriteLine("\n╔══════════════════════════════╗");
                Console.WriteLine("  ║     TIPOS DE MOVIMIENTO      ║");
                Console.WriteLine("  ╠══════════════════════════════╣");
                Console.WriteLine("  ║ 1. Entrada de efectivo       ║");
                Console.WriteLine("  ║ 2. Salida de efectivo        ║");
                Console.WriteLine("  ╚══════════════════════════════╝");
                Console.Write("Seleccione una opción: ");

                
                int tipoMovimientoId = MenuPrincipal.LeerEnteroPositivo("\nSeleccione el tipo de movimiento: ");
                
                if (tipoMovimientoId != 1 && tipoMovimientoId != 2)
                {
                    MenuPrincipal.MostrarMensaje("\nTipo de movimiento no válido.", ConsoleColor.Yellow);
                    Console.ReadKey();
                    return;
                }
                
                string concepto = MenuPrincipal.LeerEntrada("Concepto del movimiento: ");
                decimal valor = MenuPrincipal.LeerDecimalPositivo("Valor: ");
                string terceroId = MenuPrincipal.LeerEntrada("ID del Tercero: ");
                
                var movimiento = new MovimientoCaja
                {
                    Fecha = DateTime.Now,
                    TipoMovimientoId = tipoMovimientoId,
                    Valor = valor,
                    Concepto = concepto,
                    TerceroId = terceroId
                };
                
                bool resultado = await _repositorioMovimientos.InsertAsync(movimiento);
                
                if (resultado)
                {
                    MenuPrincipal.MostrarMensaje("\nMovimiento registrado correctamente.", ConsoleColor.Green);
                }
                else
                {
                    MenuPrincipal.MostrarMensaje("\nNo se pudo registrar el movimiento.", ConsoleColor.Red);
                }
            }
            catch (Exception ex)
            {
                MenuPrincipal.MostrarMensaje($"\nError al registrar movimiento: {ex.Message}", ConsoleColor.Red);
            }
            
            Console.Write("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
        
        private async Task VerMovimientosPorFecha()
        {
            Console.Clear();
            MenuPrincipal.MostrarEncabezado("MOVIMIENTOS POR FECHA");
            
            try
            {
                DateTime fecha = MenuPrincipal.LeerFecha("\nIngrese la fecha (DD/MM/AAAA): ");
                
                await MostrarMovimientosDia(fecha);
                
                // Mostrar saldo
                decimal saldo = await _repositorioMovimientos.GetSaldoCajaAsync(fecha);
                Console.WriteLine($"\nSaldo de caja para la fecha {fecha:dd/MM/yyyy}: {saldo:C}");
            }
            catch (Exception ex)
            {
                MenuPrincipal.MostrarMensaje($"\nError al obtener movimientos: {ex.Message}", ConsoleColor.Red);
            }
            
            Console.Write("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
        
        private async Task VerSaldoCaja()
        {
            Console.Clear();
            MenuPrincipal.MostrarEncabezado("SALDO DE CAJA");
            
            try
            {
                DateTime fecha = MenuPrincipal.LeerFecha("\nIngrese la fecha (DD/MM/AAAA): ");
                
                decimal saldo = await _repositorioMovimientos.GetSaldoCajaAsync(fecha);
                
                Console.WriteLine($"\nSaldo de caja para la fecha {fecha:dd/MM/yyyy}: {saldo:C}");
            }
            catch (Exception ex)
            {
                MenuPrincipal.MostrarMensaje($"\nError al obtener saldo: {ex.Message}", ConsoleColor.Red);
            }
            
            Console.Write("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
        
        private async Task MostrarMovimientosDia(DateTime fecha)
        {
            var movimientos = await _repositorioMovimientos.GetMovimientosByFechaAsync(fecha);
            
            if (!movimientos.Any())
            {
                MenuPrincipal.MostrarMensaje("\nNo hay movimientos registrados para esta fecha.", ConsoleColor.Yellow);
            }
            else
            {
                Console.WriteLine($"\nMovimientos para la fecha {fecha:dd/MM/yyyy}:");
                Console.WriteLine("\n{0,-5} {1,-12} {2,-20} {3,-10} {4,-15} {5,-20}", 
                    "ID", "Hora", "Tipo", "Mov.", "Valor", "Tercero");
                Console.WriteLine(new string('-', 90));
                
                foreach (var movimiento in movimientos)
                {
                    ConsoleColor color = movimiento.TipoMovimiento == "Entrada" ? ConsoleColor.Green : ConsoleColor.Red;
                    
                    Console.ForegroundColor = color;
                    Console.WriteLine("{0,-5} {1,-12} {2,-20} {3,-10} {4,-15} {5,-20}", 
                        movimiento.Id, 
                        movimiento.Fecha.ToString("HH:mm:ss"),
                        movimiento.TipoMovimientoNombre,
                        movimiento.TipoMovimiento,
                        movimiento.TipoMovimiento == "Entrada" ? movimiento.Valor.ToString("C") : $"-{movimiento.Valor:C}",
                        movimiento.Tercero?.NombreCompleto.Length > 17 
                            ? movimiento.Tercero.NombreCompleto.Substring(0, 17) + "..." 
                            : movimiento.Tercero?.NombreCompleto);
                    Console.ResetColor();
                }
                
                Console.WriteLine(new string('-', 90));
            }
        }
    }
}