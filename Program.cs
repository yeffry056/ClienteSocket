using Cliente;
using Cliente.entidades;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

public class Program
{

    private static void Main(string[] args)
    {
        Conexion.conexion();
    }
}



















































/*
    //recibiendo msj del servidor 
            byte[] bytes = new byte[1024];
            int byteRec = sender.Receive(bytes);
            mesa = (Mesa)serialization.Deserializate(bytes);
            LisMesa.Add(mesa);
            Listar();
           // string text = Encoding.ASCII.GetString(bytes, 0, byteRec);
           // Console.WriteLine("servidor: " + text);

 
 
 
 
 
 
 
 
 
  do
            {
                Console.Clear();
                Console.Write("1.Listar Mesa\n2.Agregar Mesa\n3.Salir\nElija una opcion: ");
                opc = int.Parse(Console.ReadLine());

                switch (opc)
                {
                    case 1:
                        Console.Clear();
                        Listar();
                        break;
                    case 2:
                        Console.Clear();
                        Agregar();
                        sender.Send(serialization.Serializate(mesa));
                        //LisMesa.Add(mesa);
                        
                        break;
                    case 3:
                        Console.Clear();
                        break;
                    default:
                        Console.WriteLine("Opcion invalida...");
                        break;


                }


            } while (opc != 3);
 
 
 
 
 
 
 
 
 
 
 */