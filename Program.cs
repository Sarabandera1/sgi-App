<<<<<<< HEAD
﻿// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
=======
﻿using System;
using sgi_App.infrastructure.mysql;
using sgi_App.UI; 

namespace sgi_App
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine(" Probando conexión a la base de datos MySQL...");
                Conexionmysql conexion = new Conexionmysql();
                bool resultado = conexion.ProbarConexion();

                Console.WriteLine(resultado ? " Todo correcto." :  "Falló la prueba.");

                // Si la conexión fue exitosa, mostrar el menú
                if (resultado)
                {
                    MenuPrincipal menu = new MenuPrincipal();
                    menu.MostrarMenu();
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($" Error fatal en la aplicación: {ex.Message}");
                Console.ResetColor();
            }
        }
    }
}
>>>>>>> 2aa8f24 (feat: :sparkles:)
