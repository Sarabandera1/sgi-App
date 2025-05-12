using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sgi-App.Models;
using sgi-App.Repositories;

namespace sgi-App.UI
{
    public class MenuPlanes
    {
        private readonly PlanRepository _almacenPlanes;
        private readonly ProductoRepository _almacenProductos;
        
        public MenuPlanes()
        {
            _almacenPlanes = new PlanRepository();
            _almacenProductos = new ProductoRepository();
        }
        
        public void MostrarMenu()
        {
            bool salir = false;
            
            while (!salir)
            {
                Console.Clear();
                MenuPrincipal.MostrarEncabezado("GESTIÓN DE PLANES PROMOCIONALES");
                
                Console.Clear();
                Console.WriteLine("╔════════════════════════════════════════╗");
                Console.WriteLine("║           MENÚ DE PLANES               ║");
                Console.WriteLine("╠════════════════════════════════════════╣");
                Console.WriteLine("║ 1. 📋 Listar Planes                    ║");
                Console.WriteLine("║ 2. 🔍 Ver Detalle de Plan              ║");
                Console.WriteLine("║ 3. ➕ Crear Nuevo Plan                 ║");
                Console.WriteLine("║ 4. ✏️  Modificar Plan                  ║");
                Console.WriteLine("║ 5. ❌ Eliminar Plan                    ║");
                Console.WriteLine("║ 6. ✅ Ver Planes Vigentes              ║");
                Console.WriteLine("║ 0. 🔙 Regresar al Menú Principal       ║");
                Console.WriteLine("╚════════════════════════════════════════╝");
                Console.Write("\nSeleccione una opción: ");

                
                Console.Write("\nSeleccione una opción: ");
                string opcionSeleccionada = Console.ReadLine() ?? "";
                
                switch (opcionSeleccionada)
                {
                    case "1":
                        VerTodosLosPlanes().Wait();
                        break;
                    case "2":
                        VerDetalleDePlan().Wait();
                        break;
                    case "3":
                        CrearNuevoPlan().Wait();
                        break;
                    case "4":
                        ActualizarPlanExistente().Wait();
                        break;
                    case "5":
                        EliminarPlanExistente().Wait();
                        break;
                    case "6":
                        VerPlanesActivos().Wait();
                        break;
                    case "0":
                        salir = true;
                        break;
                    default:
                        MenuPrincipal.MostrarMensaje("Opción no válida, vuelva a intentarlo", ConsoleColor.Yellow);
                        Console.ReadKey();
                        break;
                }
            }
        }
        
        private async Task VerTodosLosPlanes()
        {
            Console.Clear();
            MenuPrincipal.MostrarEncabezado("LISTA DE PLANES PROMOCIONALES");
            
            try
            {
                var listaPlanes = await _almacenPlanes.GetAllAsync();
                
                if (!listaPlanes.Any())
                {
                    MenuPrincipal.MostrarMensaje("\nNo tiene planes registrados.", ConsoleColor.Yellow);
                }
                else
                {
                    Console.WriteLine("\n{0,-5} {1,-20} {2,-12} {3,-12} {4,-15} {5,-10}", 
                        "ID", "Nombre", "Inicio", "Fin", "Descuento", "Estado");
                    Console.WriteLine(new string('-', 80));
                    
                    foreach (var planActual in listaPlanes)
                    {
                        bool estaActivo = planActual.EstaVigente();
                        ConsoleColor colorTexto = estaActivo ? ConsoleColor.Green : ConsoleColor.Gray;
                        
                        Console.ForegroundColor = colorTexto;
                        Console.WriteLine("{0,-5} {1,-20} {2,-12} {3,-12} {4,-15} {5,-10}", 
                            planActual.Id, 
                            planActual.Nombre.Length > 17 ? planActual.Nombre.Substring(0, 17) + "..." : planActual.Nombre,
                            planActual.FechaInicio.ToString("dd/MM/yyyy"),
                            planActual.FechaFin.ToString("dd/MM/yyyy"),
                            planActual.Descuento.ToString("P"),
                            estaActivo ? "VIGENTE" : "INACTIVO");
                        Console.ResetColor();
                    }
                }
            }
            catch (Exception ex)
            {
                MenuPrincipal.MostrarMensaje($"\nError al listar planes: {ex.Message}", ConsoleColor.Red);
            }
            
            Console.Write("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
        
        private async Task VerDetalleDePlan()
        {
            Console.Clear();
            MenuPrincipal.MostrarEncabezado("DETALLE DE PLAN PROMOCIONAL");
            
            try
            {
                int idPlan = MenuPrincipal.LeerEnteroPositivo("\nIngrese el ID del plan: ");
                
                var planBuscado = await _almacenPlanes.GetByIdAsync(idPlan);
                
                if (planBuscado == null)
                {
                    MenuPrincipal.MostrarMensaje("\nEl plan no existe.", ConsoleColor.Yellow);
                }
                else
                {
                    bool estaActivo = planBuscado.EstaVigente();
                    
                    Console.WriteLine("\nINFORMACIÓN DEL PLAN:");
                    Console.WriteLine($"ID: {planBuscado.Id}");
                    Console.WriteLine($"Nombre: {planBuscado.Nombre}");
                    Console.WriteLine($"Fecha Inicio: {planBuscado.FechaInicio:dd/MM/yyyy}");
                    Console.WriteLine($"Fecha Fin: {planBuscado.FechaFin:dd/MM/yyyy}");
                    Console.WriteLine($"Descuento: {planBuscado.Descuento:P}");
                    Console.WriteLine($"Estado: {(estaActivo ? "VIGENTE" : "INACTIVO")}");
                    
                    Console.WriteLine("\nPRODUCTOS EN PROMOCIÓN:");
                    
                    if (!planBuscado.Productos.Any())
                    {
                        MenuPrincipal.MostrarMensaje("  No hay productos asignados a este plan.", ConsoleColor.Yellow);
                    }
                    else
                    {
                        Console.WriteLine("{0,-10} {1,-30} {2,-10}", 
                            "ID", "Nombre", "Stock");
                        Console.WriteLine(new string('-', 60));
                        
                        foreach (var articulo in planBuscado.Productos)
                        {
                            Console.WriteLine("{0,-10} {1,-30} {2,-10}", 
                                articulo.Id, 
                                articulo.Nombre.Length > 27 ? articulo.Nombre.Substring(0, 27) + "..." : articulo.Nombre,
                                articulo.Stock);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MenuPrincipal.MostrarMensaje($"\nError al obtener detalle del plan: {ex.Message}", ConsoleColor.Red);
            }
            
            Console.Write("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
        
        private async Task CrearNuevoPlan()
        {
            Console.Clear();
            MenuPrincipal.MostrarEncabezado("CREAR NUEVO PLAN PROMOCIONAL");
            
            try
            {
                string nombrePlan = MenuPrincipal.LeerEntrada("\nNombre del plan: ");
                DateTime fechaInicio = MenuPrincipal.LeerFecha("Fecha de inicio (DD/MM/AAAA): ");
                DateTime fechaFin = MenuPrincipal.LeerFecha("Fecha de fin (DD/MM/AAAA): ");
                
                // Validar que la fecha de fin sea posterior a la de inicio
                if (fechaFin < fechaInicio)
                {
                    MenuPrincipal.MostrarMensaje("\nError: La fecha de fin debe ser posterior a la fecha de inicio.", ConsoleColor.Red);
                    Console.ReadKey();
                    return;
                }
                
                decimal porcentajeDescuento = 0;
                while (true)
                {
                    Console.Write("Porcentaje de descuento (0-100): ");
                    if (decimal.TryParse(Console.ReadLine(), out porcentajeDescuento) && porcentajeDescuento >= 0 && porcentajeDescuento <= 100)
                    {
                        // Convertir de porcentaje a decimal (ej: 10% -> 0.1)
                        porcentajeDescuento = porcentajeDescuento / 100;
                        break;
                    }
                    
                    MenuPrincipal.MostrarMensaje("Error: Debe ingresar un número entre 0 y 100.", ConsoleColor.Red);
                }
                
                var planNuevo = new Plan
                {
                    Nombre = nombrePlan,
                    FechaInicio = fechaInicio,
                    FechaFin = fechaFin,
                    Descuento = porcentajeDescuento,
                    Productos = new List<Producto>()
                };
                
                // Agregar productos al plan
                await AsignarProductosAlPlan(planNuevo);
                
                // Si no se agregaron productos, confirmar si se desea guardar el plan
                if (!planNuevo.Productos.Any())
                {
                    string confirmarGuardar = MenuPrincipal.LeerEntrada("\nNo se agregaron productos al plan. ¿Desea guardarlo de todas formas? (S/N): ");
                    
                    if (confirmarGuardar.ToUpper() != "S")
                    {
                        MenuPrincipal.MostrarMensaje("\nOperación cancelada.", ConsoleColor.Yellow);
                        Console.ReadKey();
                        return;
                    }
                }
                
                bool resultadoOperacion = await _almacenPlanes.InsertAsync(planNuevo);
                
                if (resultadoOperacion)
                {
                    MenuPrincipal.MostrarMensaje("\nPlan promocional creado correctamente.", ConsoleColor.Green);
                }
                else
                {
                    MenuPrincipal.MostrarMensaje("\nNo se pudo crear el plan promocional.", ConsoleColor.Red);
                }
            }
            catch (Exception ex)
            {
                MenuPrincipal.MostrarMensaje($"\nError al crear el plan: {ex.Message}", ConsoleColor.Red);
            }
            
            Console.Write("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
        
        private async Task ActualizarPlanExistente()
        {
            Console.Clear();
            MenuPrincipal.MostrarEncabezado("MODIFICAR PLAN PROMOCIONAL");
            
            try
            {
                int idPlan = MenuPrincipal.LeerEnteroPositivo("\nIngrese el ID del plan a modificar: ");
                
                var planObjetivo = await _almacenPlanes.GetByIdAsync(idPlan);
                
                if (planObjetivo == null)
                {
                    MenuPrincipal.MostrarMensaje("\nEl plan no existe.", ConsoleColor.Yellow);
                }
                else
                {
                    Console.WriteLine($"\nPlan actual: {planObjetivo.Nombre}");
                    
                    string nuevoNombre = MenuPrincipal.LeerEntrada($"Ingrese el nuevo nombre ({planObjetivo.Nombre}): ");
                    if (!string.IsNullOrWhiteSpace(nuevoNombre))
                    {
                        planObjetivo.Nombre = nuevoNombre;
                    }
                    
                    Console.Write($"Ingrese la nueva fecha de inicio ({planObjetivo.FechaInicio:dd/MM/yyyy}): ");
                    string textoFechaInicio = Console.ReadLine() ?? "";
                    if (!string.IsNullOrWhiteSpace(textoFechaInicio) && DateTime.TryParse(textoFechaInicio, out DateTime nuevaFechaInicio))
                    {
                        planObjetivo.FechaInicio = nuevaFechaInicio;
                    }
                    
                    Console.Write($"Ingrese la nueva fecha de fin ({planObjetivo.FechaFin:dd/MM/yyyy}): ");
                    string textoFechaFin = Console.ReadLine() ?? "";
                    if (!string.IsNullOrWhiteSpace(textoFechaFin) && DateTime.TryParse(textoFechaFin, out DateTime nuevaFechaFin))
                    {
                        planObjetivo.FechaFin = nuevaFechaFin;
                    }
                    
                    // Validar que la fecha de fin sea posterior a la de inicio
                    if (planObjetivo.FechaFin < planObjetivo.FechaInicio)
                    {
                        MenuPrincipal.MostrarMensaje("\nError: La fecha de fin debe ser posterior a la fecha de inicio.", ConsoleColor.Red);
                        Console.ReadKey();
                        return;
                    }
                    
                    Console.Write($"Ingrese el nuevo porcentaje de descuento ({planObjetivo.Descuento:P0}): ");
                    string textoDescuento = Console.ReadLine() ?? "";
                    if (!string.IsNullOrWhiteSpace(textoDescuento) && decimal.TryParse(textoDescuento, out decimal nuevoDescuento) && nuevoDescuento >= 0 && nuevoDescuento <= 100)
                    {
                        planObjetivo.Descuento = nuevoDescuento / 100; // Convertir de porcentaje a decimal
                    }
                    
                    // Preguntar si desea modificar los productos
                    string actualizarProductos = MenuPrincipal.LeerEntrada("\n¿Desea modificar los productos del plan? (S/N): ");
                    
                    if (actualizarProductos.ToUpper() == "S")
                    {
                        // Limpiar la lista de productos y agregar los nuevos
                        planObjetivo.Productos.Clear();
                        await AsignarProductosAlPlan(planObjetivo);
                    }
                    
                    bool resultadoOperacion = await _almacenPlanes.UpdateAsync(planObjetivo);
                    
                    if (resultadoOperacion)
                    {
                        MenuPrincipal.MostrarMensaje("\nPlan promocional actualizado correctamente.", ConsoleColor.Green);
                    }
                    else
                    {
                        MenuPrincipal.MostrarMensaje("\nNo se pudo actualizar el plan promocional.", ConsoleColor.Red);
                    }
                }
            }
            catch (Exception ex)
            {
                MenuPrincipal.MostrarMensaje($"\nError al modificar el plan: {ex.Message}", ConsoleColor.Red);
            }
            
            Console.Write("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
        
        private async Task EliminarPlanExistente()
        {
            Console.Clear();
            MenuPrincipal.MostrarEncabezado("ELIMINAR PLAN PROMOCIONAL");
            
            try
            {
                int idPlan = MenuPrincipal.LeerEnteroPositivo("\nIngrese el ID del plan a eliminar: ");
                
                var planObjetivo = await _almacenPlanes.GetByIdAsync(idPlan);
                
                if (planObjetivo == null)
                {
                    MenuPrincipal.MostrarMensaje("\nEl plan no existe.", ConsoleColor.Yellow);
                }
                else
                {
                    Console.WriteLine($"\nPlan a eliminar: {planObjetivo.Nombre}");
                    Console.WriteLine($"Vigencia: {planObjetivo.FechaInicio:dd/MM/yyyy} - {planObjetivo.FechaFin:dd/MM/yyyy}");
                    Console.WriteLine($"Descuento: {planObjetivo.Descuento:P}");
                    Console.WriteLine($"Productos en promoción: {planObjetivo.Productos.Count}");
                    
                    string confirmarEliminacion = MenuPrincipal.LeerEntrada("\n¿Está seguro de eliminar este plan promocional? (S/N): ");
                    
                    if (confirmarEliminacion.ToUpper() == "S")
                    {
                        bool resultadoOperacion = await _almacenPlanes.DeleteAsync(idPlan);
                        
                        if (resultadoOperacion)
                        {
                            MenuPrincipal.MostrarMensaje("\nPlan promocional eliminado correctamente.", ConsoleColor.Green);
                        }
                        else
                        {
                            MenuPrincipal.MostrarMensaje("\nNo se pudo eliminar el plan promocional.", ConsoleColor.Red);
                        }
                    }
                    else
                    {
                        MenuPrincipal.MostrarMensaje("\nOperación cancelada.", ConsoleColor.Yellow);
                    }
                }
            }
            catch (Exception ex)
            {
                MenuPrincipal.MostrarMensaje($"\nError al eliminar el plan: {ex.Message}", ConsoleColor.Red);
            }
            
            Console.Write("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
        
        private async Task VerPlanesActivos()
        {
            Console.Clear();
            MenuPrincipal.MostrarEncabezado("PLANES PROMOCIONALES VIGENTES");
            
            try
            {
                var planesActivos = await _almacenPlanes.GetPlanesVigentesAsync();
                
                if (!planesActivos.Any())
                {
                    MenuPrincipal.MostrarMensaje("\nNo hay planes promocionales vigentes actualmente.", ConsoleColor.Yellow);
                }
                else
                {
                    MenuPrincipal.MostrarMensaje($"\nSe encontraron {planesActivos.Count()} planes promocionales vigentes.", ConsoleColor.Green);
                    
                    Console.WriteLine("\n{0,-5} {1,-20} {2,-12} {3,-12} {4,-15}", 
                        "ID", "Nombre", "Inicio", "Fin", "Descuento");
                    Console.WriteLine(new string('-', 70));
                    
                    foreach (var planActual in planesActivos)
                    {
                        Console.WriteLine("{0,-5} {1,-20} {2,-12} {3,-12} {4,-15}", 
                            planActual.Id, 
                            planActual.Nombre.Length > 17 ? planActual.Nombre.Substring(0, 17) + "..." : planActual.Nombre,
                            planActual.FechaInicio.ToString("dd/MM/yyyy"),
                            planActual.FechaFin.ToString("dd/MM/yyyy"),
                            planActual.Descuento.ToString("P"));
                        
                        // Mostrar productos del plan
                        Console.WriteLine("  Productos en promoción:");
                        if (!planActual.Productos.Any())
                        {
                            Console.WriteLine("    No hay productos asignados a este plan.");
                        }
                        else
                        {
                            foreach (var articulo in planActual.Productos)
                            {
                                Console.WriteLine($"    - {articulo.Id}: {articulo.Nombre}");
                            }
                        }
                        
                        Console.WriteLine();
                    }
                }
            }
            catch (Exception ex)
            {
                MenuPrincipal.MostrarMensaje($"\nError al obtener planes vigentes: {ex.Message}", ConsoleColor.Red);
            }
            
            Console.Write("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
        
        private async Task AsignarProductosAlPlan(Plan planObjetivo)
        {
            bool seguirAgregando = true;
            
            // Primero obtener todos los productos para mostrar una lista
            var productosDisponibles = await _almacenProductos.GetAllAsync();
            
            if (!productosDisponibles.Any())
            {
                MenuPrincipal.MostrarMensaje("\nNo hay productos disponibles para agregar al plan.", ConsoleColor.Yellow);
                return;
            }
            
            while (seguirAgregando)
            {
                Console.Clear();
                MenuPrincipal.MostrarEncabezado("AGREGAR PRODUCTOS AL PLAN");
                
                Console.WriteLine("\nProductos disponibles:");
                Console.WriteLine("{0,-10} {1,-30} {2,-8}", "ID", "Nombre", "Stock");
                Console.WriteLine(new string('-', 50));
                
                foreach (var articulo in productosDisponibles)
                {
                    // No mostrar productos que ya están en el plan
                    if (!planObjetivo.Productos.Any(p => p.Id == articulo.Id))
                    {
                        Console.WriteLine("{0,-10} {1,-30} {2,-8}", 
                            articulo.Id, 
                            articulo.Nombre.Length > 27 ? articulo.Nombre.Substring(0, 27) + "..." : articulo.Nombre, 
                            articulo.Stock);
                    }
                }
                
                // Mostrar productos ya agregados
                if (planObjetivo.Productos.Any())
                {
                    Console.WriteLine("\nProductos ya agregados al plan:");
                    foreach (var articulo in planObjetivo.Productos)
                    {
                        Console.WriteLine($"- {articulo.Id}: {articulo.Nombre}");
                    }
                }
                
                string textoIdProducto = MenuPrincipal.LeerEntrada("\nIngrese el ID del producto a agregar (0 para terminar): ");
                
                if (textoIdProducto == "0")
                {
                    seguirAgregando = false;
                }
                else
                {
                    // Verificar si el producto ya está en el plan
                    if (planObjetivo.Productos.Any(p => p.Id == textoIdProducto))
                    {
                        MenuPrincipal.MostrarMensaje("\nEl producto ya está agregado al plan.", ConsoleColor.Yellow);
                        Console.ReadKey();
                        continue;
                    }
                    
                    // Verificar si el producto existe
                    var productoSeleccionado = productosDisponibles.FirstOrDefault(p => p.Id == textoIdProducto);
                    if (productoSeleccionado == null)
                    {
                        MenuPrincipal.MostrarMensaje("\nEl producto no existe.", ConsoleColor.Yellow);
                        Console.ReadKey();
                        continue;
                    }
                    
                    // Agregar el producto al plan
                    planObjetivo.Productos.Add(productoSeleccionado);
                    MenuPrincipal.MostrarMensaje($"\nProducto '{productoSeleccionado.Nombre}' agregado al plan.", ConsoleColor.Green);
                    
                    string agregarMas = MenuPrincipal.LeerEntrada("\n¿Desea agregar otro producto? (S/N): ");
                    seguirAgregando = agregarMas.ToUpper() == "S";
                }
            }
        }
    }
}