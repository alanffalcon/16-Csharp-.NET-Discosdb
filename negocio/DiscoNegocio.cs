using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dominio;
// INCLUIMOS LIBRERIA
//using System.Data.SqlClient;

namespace negocio
{
    public class DiscoNegocio
    {
        public List<Disco> listar()
        {
            List<Disco> lista = new List<Disco>();
            // Conexion sql:
            SqlConnection conexion = new SqlConnection();
            SqlCommand comando = new SqlCommand();
            SqlDataReader lector;

            try
            {
                conexion.ConnectionString = "server=.\\SQLEXPRESS; database=DISCOS_DB; integrated security=true";
                comando.CommandType = System.Data.CommandType.Text;
                comando.CommandText = "Select D.Titulo, D.FechaLanzamiento, D.UrlImagenTapa, D.CantidadCanciones, E.Descripcion as Genero, T.Descripcion as Edicion, D.IdEstilo, D.IdTipoEdicion, D.Id from DISCOS D, ESTILOS E, TIPOSEDICION T Where D.IdEstilo = E.Id AND D.IdTipoEdicion = T.Id\r\n";
                comando.Connection = conexion;

                // Abro conexion
                conexion.Open();
                // guardamos la lectura:
                lector = comando.ExecuteReader();

                // Miramos las filas mientas haya algo que leer:
                while (lector.Read())
                {
                    // Los guardamos en un disco aux:
                    Disco aux = new Disco();

                    // Guardamos los datos
                    aux.Id = (int)lector["Id"];

                    aux.Titulo = (string)lector["Titulo"];
                    //klojnnklnl
                    aux.Genero = new Genero();
                    aux.Genero.Id = (int)lector["IdEstilo"];
                    aux.Genero.Edicion = (string)lector["Genero"];
                    aux.Edicion = new Edicion();
                    aux.Edicion.Id = (int)lector["IdTipoEdicion"];
                    aux.Edicion.Formato = (string)lector["Edicion"];
                    aux.FechaLanzamiento = (DateTime)lector["FechaLanzamiento"];
                    //aux.UrlImagen = (string)lector["UrlImagenTapa"];
                    aux.CantidadCanciones = (int)lector["CantidadCanciones"];

                    if (!(lector["UrlImagenTapa"] is DBNull))
                    {
                        aux.UrlImagen = (string)lector["UrlImagenTapa"];
                    }

                    // Agregamos el disco a la lista:
                    lista.Add(aux);
                }

                // Cerramos conexion.
                conexion.Close();

                return lista;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void eliminarLogico(int id)
        {
            try
            {
                AccesoDatos datos = new AccesoDatos();
                datos.setearConsulta("update DISCOS set Activo Where id = @id");
                datos.setearParametro("@id", id);
                datos.ejecutarAccion();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void eliminar(int id)
        {
            try
            {
                AccesoDatos datos = new AccesoDatos();
                datos.setearConsulta("delete from discos Where id = @id");
                datos.setearParametro("@id", id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<Disco> filtrar(string campo, string criterio, string filtro)
        {
            List<Disco> lista = new List<Disco>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                string consulta = "select D.CantidadCanciones as Canciones, D.Titulo, D.FechaLanzamiento, D.UrlImagenTapa as UrlImagen, E.Descripcion as Estilo, T.Descripcion as Edicion, E.Id as IdEstilo, T.Id as IdEdicion,  D.Id from DISCOS D, ESTILOS E, TIPOSEDICION T Where D.Id = E.Id And D.Id = T.Id And ";
                if (campo == "Canciones")
                {
                    switch (criterio)
                    {
                        case "Mayor a":
                            consulta += "D.CantidadCanciones > " + filtro;
                            break;
                        case "Menor a":
                            consulta += "D.CantidadCanciones < " + filtro;
                            break;
                        default:
                            consulta += "D.CantidadCanciones = " + filtro;
                            break;
                    }
                }
                else if (campo == "Titulo")
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "Titulo like '" + filtro + "%' ";
                            break;
                        case "Termina con":
                            consulta += "Titulo like '%" + filtro + "'";
                            break;
                        default:
                            consulta += "Titulo like '%" + filtro + "%'";
                            break;
                    }
                }
                else
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "E.Descripcion like '" + filtro + "%' ";
                            break;
                        case "Termina con":
                            consulta += "E.Descripcion like '%" + filtro + "'";
                            break;
                        default:
                            consulta += "E.Descripcion like '%" + filtro + "%'";
                            break;
                    }
                }

                datos.setearConsulta(consulta);
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    Disco aux = new Disco();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Titulo = (string)datos.Lector["Titulo"];
                    aux.FechaLanzamiento = (DateTime)datos.Lector["FechaLanzamiento"];
                    aux.CantidadCanciones = (int)datos.Lector["Canciones"];
                    if (!(datos.Lector["UrlImagen"] is DBNull))
                    {
                        aux.UrlImagen = (string)datos.Lector["UrlImagen"];
                    }

                    aux.Genero = new Genero();
                    aux.Genero.Id = (int)datos.Lector["IdEstilo"];
                    aux.Genero.Edicion = (string)datos.Lector["Estilo"];
                    aux.Edicion = new Edicion();
                    aux.Edicion.Id = (int)datos.Lector["IdEdicion"];
                    aux.Edicion.Formato = (string)datos.Lector["Edicion"];

                    lista.Add(aux);
                }

                return lista;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void agregar(Disco nuevo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("Insert into DISCOS (Titulo, FechaLanzamiento, CantidadCanciones, IdEstilo, IdTipoEdicion, UrlImagenTapa)values('"+ nuevo.Titulo + "', '" + nuevo.FechaLanzamiento + "', " + nuevo.CantidadCanciones + ", @idEstilo, @idTipoEdicion, @urlImagen)");
                datos.setearParametro("@idEstilo", nuevo.Genero.Id);
                datos.setearParametro("@idTipoEdicion", nuevo.Edicion.Id);
                datos.setearParametro("@urlImagen", nuevo.UrlImagen);
                datos.ejecutarAccion();
            
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        public void modificar(Disco disc)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("update DISCOS set Titulo = @titulo, FechaLanzamiento = @fechalanzamiento, CantidadCanciones = @canciones, UrlImagenTapa = @img, IdEstilo = @idEstilo, IdTipoEdicion = @idTipoEdicion Where Id = @id\r\n");
                datos.setearParametro("@titulo", disc.Titulo);
                datos.setearParametro("@fechalanzamiento", disc.FechaLanzamiento);
                datos.setearParametro("@canciones", disc.CantidadCanciones);
                datos.setearParametro("@img", disc.UrlImagen);
                datos.setearParametro("@idEstilo", disc.Genero.Id);
                datos.setearParametro("@idTipoEdicion", disc.Edicion.Id);
                datos.setearParametro("@id", disc.Id);

                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }






        // PRUEBA 


    }
}
