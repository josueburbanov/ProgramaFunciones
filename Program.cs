using System.Data.SqlClient;

namespace ProgramaFunciones
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Bienvenido");
            Console.WriteLine("Este programa permite hacer consultas a la BDD");
            Console.WriteLine("Escriba 1 si quiere obtener todos los productos de la Base");
            Console.WriteLine("Escriba 2 si quiere obtener un producto dado su nombre");
            string opcion = Console.ReadLine();
            
            //El switch funciona con un if pero para varias opciones
            switch (opcion)
            {
                //Si la opcion seleccionada es 1
                case "1":
                    //Aqui deberia escribir el codigo que se conecte a la base de datos
                    //Si escribo todo el codigo aqui va a ser dificil de leer
                    //Por eso utilizo una función
                    //Esta funcion esta escrita debajo del metodo main
                    //El codigo se ve mas ordenado
                    List<Producto> productos = ObtenerProductos();

                    //utilizo un foreach para imprimir
                    foreach (var item in productos)
                    {
                        Console.WriteLine(item.Descripciones);
                    }

                    break;

                //Si la opcion seleccionada es 2
                case "2":
                    //Aqui tambien voy a utilizar una funcion para que el codigo se mantenga legible
                    //A diferencia de la anterior funcion, esta funcion va a estar en otro archivo
                    //El archivo donde esta esta funcion se llama ManejadorProducto
                    //Primero pido el id del producto que quiero buscar
                    Console.WriteLine("Ingrese el nombre del producto que desea obtener");
                    string nombreProducto = Convert.ToString(Console.ReadLine());
                    //Utilizo la clase ManejadorProducto y el metodo ObtenerProducto directamente porque
                    //es una clase estatica es decir la clase y el metodo tienen la palabra static en su defincion
                    Producto productoEncontrado = ManejadorProducto.ObtenerProducto(nombreProducto);
                    Console.WriteLine("Se ha encontrado el producto "+productoEncontrado.Descripciones);
                    break;
                default:
                    break;
            }

        }

        public static string cadenaConexion = "Data Source=DESKTOP-B4790FP\\SQLEXPRESS01;Initial Catalog=SistemaGestion;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public static List<Producto> ObtenerProductos()
        {
            List<Producto> productos = new List<Producto>();
            using (SqlConnection conn = new SqlConnection(cadenaConexion))
            {
                SqlCommand comando = new SqlCommand("SELECT * FROM Producto", conn);
                conn.Open();

                using (SqlDataReader reader = comando.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Producto productoTemporal = new Producto();
                            productoTemporal.Id = reader.GetInt64(0);
                            productoTemporal.Descripciones = reader.GetString(1);
                            productoTemporal.Costo = reader.GetDecimal(2);
                            productoTemporal.PrecioVenta = reader.GetDecimal(3);
                            productoTemporal.Stock = reader.GetInt32(4);
                            productoTemporal.IdUsuario = reader.GetInt64(5);

                            productos.Add(productoTemporal);
                        }
                    }
                }
                return productos;


            }

        }
    }
}