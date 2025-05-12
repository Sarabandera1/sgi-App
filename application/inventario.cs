namespace Inventario.Application.Config
{
    public static class AppSettings
    {
        public static string ConnectionString = "Server=localhost;Database=gestionInventario;Uid=campus2023;Pwd=campus2023;";


        public static int NumPaginacion = 10;
        public static bool RegistrosEliminados = false;
        public static int TiempoEspera = 30; // segundos
    }
}