using Cliente.entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Cliente
{
    public class Conexion
    {

        static List<Mesa> LisMesa = new List<Mesa>();
        static Mesa mesa;

        static Clientes cliente;
        static List<Clientes> clienteList = new List<Clientes>();

        static Reservacion reservacion;
        static List<Reservacion> reservacionList = new List<Reservacion>();


        static IPHostEntry host = Dns.GetHostEntry("localhost");
        static IPAddress ipAddress = host.AddressList[0];

        static IPEndPoint remoteEp = new IPEndPoint(ipAddress, 11200);

        static Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        
       
        public static void conexion()
        {


            int opc = 0;
            try
            {
                
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
                            CargarListaMesa();
                            RegistrarReservacion();
                            break;

                        case 2:
                            RegistroCli();
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
        public static void CargarListaMesa()
        {
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

                if (contM == 5)
                    mesa.Precio = Convert.ToDouble(dataM.Replace("<EOF>", ""));

                if (contM == 6)
                {
                    mesa.Dsiponibilidad = Convert.ToBoolean(dataM.Replace("<EOF>", ""));
                    LisMesa.Add(mesa);
                    mesa = new Mesa();
                    contM = 0;
                    vuelta--;

                    if (vuelta == 0)
                        break;
                }


                contM++;
            }
        }
        public static void RegistrarReservacion()
        {
            while (true)
            {
                bool aux = false;
                Console.Clear();
                ListaMesa.Listar(LisMesa);
                Console.Write("\nDesea hacer una reservasion:\n1.Si\n2.No\nElija una opcion:");
                int op = int.Parse(Console.ReadLine());

                if (op == 1)
                {
                    byte[] res = Encoding.ASCII.GetBytes(Convert.ToString(op) + "<EOF>");
                    int byteSentRes = sender.Send(res);

                    reservacion = new Reservacion();
                    Console.Write("\nReservacionId: ");
                    reservacion.reservacionId = int.Parse(Console.ReadLine());
                    byte[] msgRes = Encoding.ASCII.GetBytes(Convert.ToString(reservacion.reservacionId) + "<EOF>");
                    sender.Send(msgRes);

                    Console.Write("\nClienteId: ");
                    reservacion.clienteId = int.Parse(Console.ReadLine());
                    byte[] msgRes1 = Encoding.ASCII.GetBytes(Convert.ToString(reservacion.clienteId) + "<EOF>");
                    sender.Send(msgRes1);

                    Console.Write("\nMesaId: ");
                    reservacion.mesaId = int.Parse(Console.ReadLine());
                    //porne validar 
                    foreach (var m in LisMesa)
                    {
                        if (m.MesaId == reservacion.mesaId)
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


                if (op == 2)
                {
                    byte[] res = Encoding.ASCII.GetBytes("2" + "<EOF>");
                    int byteSentRes = sender.Send(res);
                    break;
                }


                if (aux)
                    break;

            }
        }
        public static void RegistroCli()
        {
            Console.Clear();
            Console.WriteLine("========================");
            Console.WriteLine("  Registro de cliente");
            Console.WriteLine("========================");
            cliente = new Clientes();
            Console.Write("\nClienteId: ");
            cliente.ClienteId = int.Parse(Console.ReadLine());
            byte[] msgCli = Encoding.ASCII.GetBytes(Convert.ToString(cliente.ClienteId) + "<EOF>");
            int byteSentCl = sender.Send(msgCli);

            Console.Write("\nNombres: ");
            cliente.nombres = Console.ReadLine();
            byte[] msgCli1 = Encoding.ASCII.GetBytes(Convert.ToString(cliente.nombres) + "<EOF>");
            int byteSentCl1 = sender.Send(msgCli1);

            Console.Write("\nTelefono: ");
            cliente.Telefono = Console.ReadLine();
            byte[] msgCli2 = Encoding.ASCII.GetBytes(Convert.ToString(cliente.Telefono) + "<EOF>");
            int byteSentCl2 = sender.Send(msgCli2);

            Console.Write("\nEmail: ");
            cliente.Email = Console.ReadLine();
            byte[] msgCli3 = Encoding.ASCII.GetBytes(Convert.ToString(cliente.Email) + "<EOF>");
            int byteSentCl3 = sender.Send(msgCli3);


            //poner validar



            clienteList.Add(cliente);
            Console.Write("\n\n");

            Console.Write("\nGuardado. ");
            Thread.Sleep(2000);
        }
        public static bool validarCliente(Clientes cliente)
        {
            bool esValido = true;
            if (cliente.nombres.Length == 0)
            {
                esValido = false;
                Console.WriteLine("nombre vacia");
                return esValido;
            }
            if (cliente.Telefono.Length == 0)
            {
                esValido = false;
                Console.WriteLine("telefono vacio");
                return esValido;
            }
            if (cliente.Email.Length == 0)
            {
                esValido = false;
                Console.WriteLine("email vacia");
                return esValido;
            }
            
            return esValido;
        }
    }
}
