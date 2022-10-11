using Cliente;
using Cliente.entidades;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

public class Program
{
	
    static List<Mesa> LisMesa = new List<Mesa>();
    static Mesa mesa;

    static Persona persona;
    static List<Persona> personaList = new List<Persona>();

    static Reservacion reservacion;
    static List<Reservacion> reservacionList = new List<Reservacion>();
   

    public static void conexion()
    {
        
        IPHostEntry host = Dns.GetHostEntry("localhost");
        IPAddress ipAddress = host.AddressList[0];
     
        IPEndPoint remoteEp = new IPEndPoint(ipAddress, 11200);
        int opc = 0;
        try
        {
            Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            sender.Connect(remoteEp);

            Console.WriteLine("Conectado con el servidor");
            byte[] bytes;

           

            do
            {
                

                bytes = new byte[1024];
             
                Console.Clear();
                Console.Write("1.Reservacion\n2.Registrarme\n3.Salir\nElija una opcion: ");
                 opc = int.Parse(Console.ReadLine());
                byte[] msg = Encoding.ASCII.GetBytes(Convert.ToString(opc) + "<EOF>");
                int byteSent = sender.Send(msg);
                
                   

                switch (opc)
                {
                    case 1:
                        Console.Clear();
                        LisMesa = new List<Mesa>();
                        int contM = 0, vuelta = 0;
                        mesa = new Mesa();
                        while (true)
                        {
                            string dataM = null;
                            byte[] bytesM = null;

                            while (true)
                            {
                                bytesM = new byte[1024];
                                int byteRec = sender.Receive(bytesM);
                                dataM = Encoding.ASCII.GetString(bytesM, 0, byteRec);

                                if (dataM.IndexOf("<EOF>") > -1)
                                    break;
                            }
                          
                            if (contM == 0)
                                vuelta = Convert.ToInt16(dataM.Replace("<EOF>", ""));

                            if (contM == 1)
                                mesa.MesaId = Convert.ToInt32(dataM.Replace("<EOF>", ""));

                            if (contM == 2)
                                mesa.Ubicacion = dataM.Replace("<EOF>", "");

                            if (contM == 3)
                                mesa.Capacidad = Convert.ToInt32(dataM.Replace("<EOF>", ""));

                            if (contM == 4)
                                mesa.Forma = dataM.Replace("<EOF>", "");
                                
                            if(contM == 5)
                                mesa.Precio = Convert.ToDouble(dataM.Replace("<EOF>", ""));

                            if(contM == 6)
                            {
                                mesa.Dsiponibilidad = Convert.ToBoolean(dataM.Replace("<EOF>", ""));
                                LisMesa.Add(mesa);
                                mesa = new Mesa();
                                contM = 0;
                                vuelta--;

                                if(vuelta == 0)
                                     break;
                            }
     

                            contM++;
                        }

                        
                        while (true)
                        {
                            bool aux= false;
                            Console.Clear();
                            Listar();
                            Console.Write("\nDesea hacer una reservasion:\n1.Si\n2.No\nElija una opcion:");
                            int op = int.Parse(Console.ReadLine());

                            if(op == 1)
                            {
                                byte[] res = Encoding.ASCII.GetBytes(Convert.ToString(op) + "<EOF>");
                                int byteSentRes = sender.Send(res);
                                
                                reservacion = new Reservacion();
                                Console.Write("\nReservacionId: ");
                                reservacion.reservacionId = int.Parse(Console.ReadLine());
                                byte[] msgRes = Encoding.ASCII.GetBytes(Convert.ToString(reservacion.reservacionId) + "<EOF>");
                                sender.Send(msgRes);

                                Console.Write("\nPersonaId: ");
                                reservacion.personaId = int.Parse(Console.ReadLine());
                                byte[] msgRes1 = Encoding.ASCII.GetBytes(Convert.ToString(reservacion.personaId) + "<EOF>");
                                sender.Send(msgRes1);
                                
                                Console.Write("\nMesaId: ");
                                reservacion.mesaId = int.Parse(Console.ReadLine());
                                foreach (var m in LisMesa)
                                {
                                    if(m.MesaId == reservacion.mesaId)
                                    {
                                        if (m.Dsiponibilidad)
                                        {
                                            byte[] msgRes2 = Encoding.ASCII.GetBytes(Convert.ToString(reservacion.mesaId) + "<EOF>");
                                            sender.Send(msgRes2);
                                            reservacionList.Add(reservacion);
                                            aux = true;
                                            
                                        }
                                        else
                                        {
                                            byte[] msgRes2 = Encoding.ASCII.GetBytes(Convert.ToString("0"));
                                            sender.Send(msgRes2);
                                            Console.WriteLine("\nMesa no disponible. ");
                                            Thread.Sleep(1000);
                                        }
                                    }
                                }
                               


                                
                            }
                            

                            if(op == 2)
                            {
                                byte[] res = Encoding.ASCII.GetBytes("2" + "<EOF>");
                                int byteSentRes = sender.Send(res);
                                break;
                            }


                            if (aux)
                                break;

                        }
                       

                        break;

                       
                    case 2:
                        Console.Clear();
                        Console.WriteLine("========================");
                        Console.WriteLine("  Registro de cliente");
                        Console.WriteLine("========================");
                        persona = new Persona();
                        Console.Write("\nPersonaId: ");
                        persona.Id = int.Parse(Console.ReadLine());
                        byte[] msgCli = Encoding.ASCII.GetBytes(Convert.ToString(persona.Id) + "<EOF>");
                        int byteSentCl = sender.Send(msgCli);

                        Console.Write("\nNombres: ");
                        persona.nombres = Console.ReadLine();
                        byte[] msgCli1 = Encoding.ASCII.GetBytes(Convert.ToString(persona.nombres) + "<EOF>");
                        int byteSentCl1 = sender.Send(msgCli1);

                        Console.Write("\nTelefono: ");
                        persona.Telefono = Console.ReadLine();
                        byte[] msgCli2 = Encoding.ASCII.GetBytes(Convert.ToString(persona.Telefono) + "<EOF>");
                        int byteSentCl2 = sender.Send(msgCli2);

                        Console.Write("\nEmail: ");
                        persona.Email = Console.ReadLine();
                        byte[] msgCli3 = Encoding.ASCII.GetBytes(Convert.ToString(persona.Email) + "<EOF>");
                        int byteSentCl3 = sender.Send(msgCli3);




                        personaList.Add(persona);
                        Console.Write("\n\n");

                        Console.Write("\nGuardado. ");
                        Thread.Sleep(2000);

                        break;
                    case 3:
                        Console.Clear();
                        Console.WriteLine("\n\nBye...");
                        break;
                    
                    default:
                        Console.WriteLine("\n\nOpcion invalida...");
                        break;
                }

               
               
            } while (opc != 3);
        }
        catch (Exception e)
        {

            Console.WriteLine(e.ToString());
        }
    }
    public static void Listar()
    {
       
        Console.WriteLine("==================================");
        Console.WriteLine("         Listado de Mesas         ");
        Console.WriteLine("==================================\n");

        if(LisMesa == null)
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
   

    private static void Main(string[] args)
    {
     
        conexion();
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