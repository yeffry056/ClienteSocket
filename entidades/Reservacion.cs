using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cliente.entidades
{
    public class Reservacion
    {
        public int reservacionId { get; set; }
        public int personaId { get; set; }
        public int mesaId { get; set; }
        //public string fecha { get; set; }
        //public string hora { get; set; }
    }
}
