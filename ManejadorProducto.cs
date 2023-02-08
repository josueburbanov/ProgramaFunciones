using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramaFunciones
{
    //Clase estatica
    internal static class ManejadorProducto
    {
        public static string cadenaConexion = "Data Source=DESKTOP-B4790FP\\SQLEXPRESS01;Initial Catalog=SistemaGestion;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        //Funcion o metodo estatico para poder utilizarlo sin instanciar
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

        public static Producto ObtenerProducto(string descripciones)
        {
            Producto producto = new Producto();

            using (SqlConnection conn = new SqlConnection(cadenaConexion))
            {
                //Forma 1 para crear un comando con parametros
                SqlCommand comando = new SqlCommand($"SELECT * FROM Producto WHERE Descripciones='{descripciones}' ", conn);

                //Forma 2 para crear un comando con parametros
                SqlCommand comando2 = new SqlCommand("SELECT * FROM Producto WHERE Descripciones=@descripciones", conn);

                //Creo mi parametro descripciones
                //SqlParameter descripParametro = new SqlParameter();
                //descripParametro.Value = descripciones;
                //descripParametro.SqlDbType = SqlDbType.VarChar;
                //descripParametro.ParameterName = "descripciones";

                //comando2.Parameters.Add(descripParametro);


                //Tercera opción
                comando2.Parameters.AddWithValue("@descripciones", descripciones);

                conn.Open();

                SqlDataReader reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    producto.Id = reader.GetInt64(0);
                    producto.Descripciones = reader.GetString(1);
                    producto.Costo = reader.GetDecimal(2);
                    producto.PrecioVenta = reader.GetDecimal(3);
                    producto.Stock = reader.GetInt32(4);
                    producto.IdUsuario = reader.GetInt64(5);

                }
            }
            return producto;


        }

        public static Producto ObtenerProducto(long id)
        {
            Producto producto = new Producto();

            using (SqlConnection conn = new SqlConnection(cadenaConexion))
            {
                SqlCommand comando2 = new SqlCommand("SELECT * FROM Producto WHERE Id=@id", conn);

                comando2.Parameters.AddWithValue("@id", id);

                conn.Open();

                SqlDataReader reader = comando2.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    producto.Id = reader.GetInt64(0);
                    producto.Descripciones = reader.GetString(1);
                    producto.Costo = reader.GetDecimal(2);
                    producto.PrecioVenta = reader.GetDecimal(3);
                    producto.Stock = reader.GetInt32(4);
                    producto.IdUsuario = reader.GetInt64(5);

                }
            }
            return producto;
        }

        public static List<Producto> ObtenerProductosVendidos(long idUsuario)
        {
            List<long> ListaIdProductos = new List<long>();
            using (SqlConnection conn = new SqlConnection(cadenaConexion))
            {
                SqlCommand comando2 = new SqlCommand("SELECT IdProducto FROM Venta" +
                    " INNER JOIN ProductoVendido" +
                    " ON Venta.Id = ProductoVendido.IdVenta" +
                    " WHERE IdUsuario = @idUsuario", conn);

                comando2.Parameters.AddWithValue("@idUsuario", idUsuario);

                conn.Open();

                SqlDataReader reader = comando2.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ListaIdProductos.Add(reader.GetInt64(0));
                    }

                }
            }
            List<Producto> productos = new List<Producto>();
            foreach (var id in ListaIdProductos)
            {
                Producto prodTemp = ObtenerProducto(id);
                productos.Add(prodTemp);
            }

            return productos;

        }

        public static int InsertarProducto(Producto producto)
        {
            using (SqlConnection conn = new SqlConnection(cadenaConexion))
            {
                SqlCommand comando = new SqlCommand("INSERT INTO Producto(Descripciones, Costo, PrecioVenta, Stock, IdUsuario)" +
                    "VALUES(@descripciones, @costo, @precioVenta, @stock, @idUsuario)", conn);
                comando.Parameters.AddWithValue("@descripciones", producto.Descripciones);
                comando.Parameters.AddWithValue("@costo", producto.Costo);
                comando.Parameters.AddWithValue("@precioVenta", producto.PrecioVenta);
                comando.Parameters.AddWithValue("@stock", producto.Stock);
                comando.Parameters.AddWithValue("@idUsuario", producto.IdUsuario);

                conn.Open();
                return comando.ExecuteNonQuery();

            }
        }

        public static int DeleteProducto(long id)
        {
            using (SqlConnection conn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand comando = new SqlCommand("DELETE FROM Producto" +
                        " WHERE id=@id", conn);
                    comando.Parameters.AddWithValue("@id", id);
                    conn.Open();
                    return comando.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("" + ex.Message);
                    return -1;
                }
            }
        }

        public static int UpdateProducto(Producto producto, long id)
        {
            using (SqlConnection conn = new SqlConnection(cadenaConexion))
            {
                SqlCommand comando = new SqlCommand("UPDATE Producto " +
                    "SET Descripciones=@descripciones," +
                    "Costo=@costo, PrecioVenta=@precioVenta," +
                    "Stock=@stock, IdUsuario=@idUsuario " +
                    "WHERE Id=@id", conn);
                comando.Parameters.AddWithValue("@id", id);
                comando.Parameters.AddWithValue("@descripciones", producto.Descripciones);
                comando.Parameters.AddWithValue("@costo", producto.Costo);
                comando.Parameters.AddWithValue("@precioVenta", producto.PrecioVenta);
                comando.Parameters.AddWithValue("@stock", producto.Stock);
                comando.Parameters.AddWithValue("@idUsuario", producto.IdUsuario);
                conn.Open();
                return comando.ExecuteNonQuery();
            }
        }

    }
}
