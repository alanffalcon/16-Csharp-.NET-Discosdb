using dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace negocio
{
    public class GeneroNegocio
    {
        public List<Genero> listar()
        {
            List<Genero> lista = new List<Genero>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("Select Id, Descripcion From ESTILOS");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Genero aux = new Genero();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Edicion = (string)datos.Lector["Descripcion"];

                    lista.Add(aux);
                }

                return lista;
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
    }
}
