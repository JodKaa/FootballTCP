using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Markup;
using FootballRestService.Managers;

namespace FootballTCP
{
    public class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("FootballPlayer Server Ready");
            TcpListener listener = new TcpListener(IPAddress.Any, 2121);
            listener.Start();

            while (true)
            {
                TcpClient socket = listener.AcceptTcpClient();
                Console.WriteLine("[ New Client ]");

                Task.Run(() => { HandleClient(socket); });
            }
        }

        private static void HandleClient(TcpClient socket)
        {
            NetworkStream ns = socket.GetStream();
            StreamReader reader = new StreamReader(ns);
            StreamWriter writer = new StreamWriter(ns);

            Console.WriteLine("================================================");
            Console.WriteLine("Enter 1, to get all footballplayers.");
            Console.WriteLine("Enter 2, to get a specific player.");
            Console.WriteLine("Enter 3, to create a footballplayer.");
            Console.WriteLine("Type 'disconnect' to disconnect from the server.");
            Console.WriteLine("================================================");

            string message = reader.ReadLine();

            switch (message)
            {
                case "1":
                    JsonSerializer.Serialize(PlayerManager.GetAllPlayers());
                    writer.WriteLine(PlayerManager.Players);
                    writer.Flush();
                    break;

                case "2":
                    writer.WriteLine("Enter the ID, for the specific player.");
                    writer.Flush();
                    int id = Convert.ToInt32(message);
                    JsonSerializer.Serialize(PlayerManager.GetById(id));
                    writer.WriteLine(PlayerManager.GetById(id));
                    writer.Flush();
                    break;

                //case "3":
                //    writer.WriteLine("Enter a value");
                //    writer.Flush();
                //    string value = reader.ReadLine();
                //    JsonSerializer.Deserialize<FootballPlayer>(value);
                //    PlayerManager.AddNewPlayer();
                //    writer.Flush();
                //    break;

                case "disconnect":
                    writer.WriteLine("Client disconnected.");
                    writer.Flush();
                    socket.Close();
                    break;
            }
        }
    }
}
