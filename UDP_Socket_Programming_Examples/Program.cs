using System;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Reflection.Metadata;
using System.Security.Principal;
using System.Text;

namespace UDP_Socket_Programming_Examples
{
 class Program
    {
        public const int SIO_UDP_CONNRESET =-1744830452;
        private const int BufferSize = 1024;
        static void Main(string[] args)
        {
            Console.WriteLine("uygulamayı server olarak baslatmak icin S,Client olarak baslatmak icin C giriniz");
            string input = Console.ReadLine();
            if(input.Equals("S"))
                {
                Console.WriteLine("server başlatıldı");
                Server();
                }

            else if (input.Equals("C"))
            {
                Console.WriteLine("client başlatıldı");
                Client();
            }
            else
            {
                
                Console.WriteLine("bilinmedik komut girdiniz");
            }
            Console.ReadKey();

        }
        private static void Server()
        {
            // soket oluştur server için soket
            Socket serverSocket = new Socket(SocketType.Dgram, ProtocolType.Udp);
            serverSocket.IOControl((IOControlCode)SIO_UDP_CONNRESET, new byte[] { 0, 0, 0, 0 }, null);
            IPAddress serverIpAddress = IPAddress.Parse("127.0.0.1"); // localhost 127.0.0.1
            int serverPortNum = 50000;
            IPEndPoint serverEndPoint = new IPEndPoint(serverIpAddress, serverPortNum);
            serverSocket.Bind(serverEndPoint);

            // mesajı dinle 
            byte[] receivedBytes = new byte[BufferSize];
            EndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
            serverSocket.ReceiveFrom(receivedBytes, ref clientEndPoint);
            // mesajı gönderen endpointe bize gelen mesajwıw tekrar gönder
            string receivedMessage = Encoding.ASCII.GetString(receivedBytes);
            Console.WriteLine(receivedMessage);
            // gelen veriyi yaz
            serverSocket.SendTo(receivedBytes, clientEndPoint);
            // soketi kapat
            serverSocket.Close();
        }
        private static void Client()
        {
            // soket oluştur
            Socket clientSocket = new Socket(SocketType.Dgram, ProtocolType.Udp);
            clientSocket.IOControl((IOControlCode)SIO_UDP_CONNRESET, new byte[] { 0, 0, 0, 0 }, null);
            IPAddress clientIpAddress = IPAddress.Parse("127.0.0.1"); // localhost 127.0.0.1
            int clientPortNum = 60000;
            IPEndPoint clientEndPoint = new IPEndPoint(clientIpAddress, clientPortNum);
            clientSocket.Bind(clientEndPoint);
            // servere mesaj gönder
            IPAddress serverIpAddress = IPAddress.Parse("127.0.0.1");
            int serverPortNum = 50000;
            IPEndPoint serverEndPoint = new IPEndPoint(serverIpAddress, serverPortNum);
            string MessageToSend = "merhaba ben ali can yucel";
            byte[] bytseToSend = Encoding.ASCII.GetBytes(MessageToSend);
            clientSocket.SendTo(bytseToSend, serverEndPoint);
            // soketi dinle
            byte[] receivedBytes = new byte[BufferSize];
            clientSocket.Receive(receivedBytes);
            // log message received to User
            string receivedMessage = Encoding.ASCII.GetString(receivedBytes);
            Console.WriteLine(receivedMessage);
        }

    }
}
