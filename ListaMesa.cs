using Cliente.entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cliente
{
    public class ListaMesa
    {
        public static void Listar(List<Mesa> LisMesa)
        {

            Console.WriteLine("==================================");
            Console.WriteLine("         Listado de Mesas         ");
            Console.WriteLine("==================================\n");

            if (LisMesa == null)
            {
                Console.WriteLine("Lista vacia");
                Thread.Sleep(1500);
            }
            else
            {
                foreach (var item in LisMesa)
                {

                    Console.WriteLine("MesaId: " + item.MesaId);
                    Console.WriteLine("Ubicacion: " + item.Ubicacion);
                    Console.WriteLine("Capacidad: " + item.Capacidad);
                    Console.WriteLine("Forma: " + item.Forma);
                    Console.WriteLine("Precio: " + item.Precio);
                    Console.WriteLine("Disponibilidad: " + item.Dsiponibilidad);
                    Console.WriteLine("\n\n");
                }

            }


        }
    }
}
