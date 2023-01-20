using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace dominio
{
    public class Disco
    {
        /* 
            Select D.Titulo, D.FechaLanzamiento, D.UrlImagenTapa, D.CantidadCanciones, E.Descripcion, T.Descripcion 
            from DISCOS D, ESTILOS E, TIPOSEDICION T
            Where D.IdEstilo = E.Id
            AND
            D.IdEstilo = T.Id
        */
        public int Id { get; set; }
        public string Titulo { get; set; }
        [DisplayName("Publicación")]
        public DateTime FechaLanzamiento { get; set; }
        public string UrlImagen { get; set; }
        [DisplayName("Canciones")]
        public int CantidadCanciones { get; set; }

        // Crear Elementos para estos dos.
        public Genero Genero { get; set; }
        public Edicion Edicion { get; set; }

    }
}
