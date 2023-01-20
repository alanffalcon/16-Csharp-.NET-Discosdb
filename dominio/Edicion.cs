using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class Edicion 
    {
        public int Id { get; set; }
        public string Formato { get; set; }

        public override string ToString()
        {
            return Formato;
        }

    }
}
