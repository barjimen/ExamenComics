using System.Data;
using ExamenComics.Models;
using Microsoft.Data.SqlClient;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace ExamenComics.Repositories
{
    public class RepositoryComics
    {
        private SqlCommand com;
        private SqlConnection con;
        private DataTable tablaComics;

        public RepositoryComics()
        {
            string connectionString = @"Data Source=LOCALHOST\SQLEXPRESS;Initial Catalog=COMICS;Persist Security Info=True;User ID=sa;Encrypt=True;Trust Server Certificate=True";
            this.tablaComics = new DataTable();
            string sql = "select * from COMICS";
            SqlDataAdapter adap = new SqlDataAdapter(sql, connectionString);
            adap.Fill(this.tablaComics);

            this.con = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = this.con;
        }

        public List<Comics> GetComics()
        {
            var consulta = from datos in this.tablaComics.AsEnumerable()
                           select datos;
            List<Comics> comics = new List<Comics>();
            foreach(var con in consulta)
            {
                Comics comic = new Comics
                {
                    idComic = con.Field<int>("IDCOMIC"),
                    Nombre = con.Field<string>("NOMBRE"),
                    Imagen = con.Field<string>("IMAGEN"),
                    Descripcion = con.Field<string>("DESCRIPCION")
                };
                comics.Add(comic);
            }
            return comics;
        }
        //Insert

        public async Task InsertComic(string nombre, string imagen, string descripcion)
        {
            int maxId = this.tablaComics.AsEnumerable().Select(row => row.Field<int>("IDCOMIC")).Max();
            int idNuevo = maxId + 1;
            string sql = "INSERT INTO COMICS (IDCOMIC, NOMBRE, IMAGEN, DESCRIPCION)" +
                "VALUES (@idComic, @nombre, @imagen, @descripcion)";
            this.com.Parameters.AddWithValue("@idComic", idNuevo);
            this.com.Parameters.AddWithValue("@nombre", nombre);
            this.com.Parameters.AddWithValue("@imagen", imagen);
            this.com.Parameters.AddWithValue("@descripcion", descripcion);
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            await this.con.OpenAsync();
            await this.com.ExecuteNonQueryAsync();
            await this.con.CloseAsync();
            this.com.Parameters.Clear();
        }
        //Detalles + buscador
        public List<string> GetNombres()
        {
            var consulta = (from datos in this.tablaComics.AsEnumerable()
                            select datos.Field<string>("NOMBRE")).Distinct();
            return consulta.ToList();
        }

        public List<Comics>DetallesComics(string nombre)
        {
            var consulta = from datos in this.tablaComics.AsEnumerable()
                           where datos.Field<string>("NOMBRE") == nombre
                           select datos;
            List<Comics> comics = new List<Comics>();
            foreach( var comic in consulta)
            {
                Comics comi = new Comics
                {
                    idComic = comic.Field<int>("IDCOMIC"),
                    Nombre = comic.Field<string>("NOMBRE"),
                    Imagen = comic.Field<string>("IMAGEN"),
                    Descripcion = comic.Field<string>("DESCRIPCION")
                };
                comics.Add(comi);
            }
            return comics;
        }
    }
}
