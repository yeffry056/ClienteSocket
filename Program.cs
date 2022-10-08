using Cliente;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

internal class Program
{
	static IPAddress ipAddress1;
	static Socket sender1;
    static List<Mesa> LisMesa = new List<Mesa>();
   static Mesa mesa = new Mesa();
    private static void Main(string[] args)
    {

        conexion();
    }
    public static void conexion()
    {
        
        IPHostEntry host = Dns.GetHostEntry("localhost");
        IPAddress ipAddress = host.AddressList[0];
        //ipAddress = host.AddressList[0];
        IPEndPoint remoteEp = new IPEndPoint(ipAddress, 11200);// 11200
        int opc = 0;
        try
        {
            Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            sender.Connect(remoteEp);

            Console.WriteLine("Conectado con el servidor");
            byte[] bytes = new byte[1024];


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
           
            Listar();
            // string text = Encoding.ASCII.GetString(bytes, 0, byteRec);
            // Console.WriteLine("servidor: " + text);

            Console.WriteLine("Ingrese un texto: ");
            string texto = Console.ReadLine();

            int byteRec = sender.Receive(bytes);
            mesa = (Mesa)serialization.Deserializate(bytes);

            byte[] msg = Encoding.ASCII.GetBytes(texto + "<EOF>");//el texto se convierte en arreglo de byte

            int byteSent = sender.Send(msg);//enviado msj al servidor 


            //recibiendo msj del servidor 
           
            sender.Shutdown(SocketShutdown.Both);
            sender.Close();
        }
        catch (Exception e)
        {

            Console.WriteLine(e.ToString());
        }
    }
    public static void Listar()
    {
        int i = 1;
        Console.WriteLine("==================================");
        Console.WriteLine("         Listado de Mesas         ");
        Console.WriteLine("==================================\n");
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
        Console.ReadKey();

    }
    public static void Agregar()
    {
        int cant;
        Console.WriteLine("====================================");
        Console.WriteLine("        Agregando nuevas mesas         ");
        Console.WriteLine("====================================\n");
        Console.Write("Cuantas mesa desea agragar: ");
        cant = int.Parse(Console.ReadLine());

        for (int i = 0; i < cant; i++)
        {
            Console.Write("\nMesaId: ");
            mesa.MesaId = int.Parse(Console.ReadLine());
            Console.Write("\nUbicacion: ");
            mesa.Ubicacion = Console.ReadLine();
            Console.Write("\nCapacidad: ");
            mesa.Capacidad = int.Parse(Console.ReadLine());
            Console.Write("\nForma: ");
            mesa.Forma = Console.ReadLine();
            Console.Write("\nPrecio: ");
            mesa.Precio = double.Parse(Console.ReadLine());


            LisMesa.Add(mesa);
            Console.Write("\n\n");

        }
        Console.Write("\nGuardado. ");
        Thread.Sleep(2000);
    }
    public void enviar(Object obj)
	{
		sender1.Send(serialization.Serializate(obj));

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
           // Console.WriteLine("servidor: " + text);*/