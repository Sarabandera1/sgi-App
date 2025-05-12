using MiAppHexagonal.Application.Services;
using MiAppHexagonal.Application.UI.Clientes;
using MiAppHexagonal.Domain.Entities;
using MiAppHexagonal.Domain.Factory;
using MiAppHexagonal.Infrastructure.Mysql;

internal class Program
{
    private static void Main(string[] args)
    {
        string connStr = "server=localhost;database=introcsharp;user=root;password=123456;";
        IDbFactory factory = new MySqlDbFactory(connStr);
        var servicio = new ClienteService(factory.CrearClienteRepository());
        var servicioProducto = new ProductoService(factory.CrearProductoRepository());
                while (true)
        {
            Console.WriteLine("\n--- MENÚ CLIENTES ---");
            Console.WriteLine("1. Mostrar todos");
            Console.WriteLine("2. Crear nuevo");
            Console.WriteLine("3. Actualizar");
            Console.WriteLine("4. Eliminar");
            Console.WriteLine("0. Salir");
            Console.Write("Opción: ");
            var opcion = Console.ReadLine();
            switch (opcion)
            {
                case "1":
                    servicio.MostrarTodos();
                    UICliente Uc = new UICliente(factory);
                    
                    break;
                case "2":
                    Producto producto = new Producto();
                    Console.Write("Nombre: ");
                    producto.Nombre = Console.ReadLine();
                    Console.WriteLine("Stock: ");
                    producto.Stock = int.Parse(Console.ReadLine());
                    servicioProducto.CrearProducto(producto);
                    //servicio.CrearCliente(Console.ReadLine()!);
                    break;
                case "3":
                    Console.Write("ID a actualizar: ");
                    int idA = int.Parse(Console.ReadLine()!);
                    Console.Write("Nuevo nombre: ");
                    servicio.ActualizarCliente(idA, Console.ReadLine()!);
                    break;
                case "4":
                    Console.Write("ID a eliminar: ");
                    int idE = int.Parse(Console.ReadLine()!);
                    servicio.EliminarCliente(idE);
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("❌ Opción inválida.");
                    break;
            }
        }
    }
}