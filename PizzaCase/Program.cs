using System;
using System.Threading;
using PizzaCase.Networking;

namespace PizzaCase
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Selecteer communicatieprotocol (TCP/UDP): ");
            string protocol = Console.ReadLine().ToUpper();

            if (protocol != "TCP" && protocol != "UDP")
            {
                Console.WriteLine("Ongeldig protocol. Standaard naar TCP.");
                protocol = "TCP";
            }

            // Start de server in een aparte thread
            var serverThread = new Thread(() =>
            {
                if (protocol == "TCP")
                {
                    var server = TCPServer.Instance(5000);
                    server.Start();
                }
                else if (protocol == "UDP")
                {
                    var server = UDPServer.Instance(5000);
                    server.Start();
                }
            });
            serverThread.IsBackground = true;
            serverThread.Start();

            // Wacht even om zeker te zijn dat de server is gestart
            Thread.Sleep(500);

            // Start de client
            if (protocol == "TCP")
            {
                var client = new TCPClient("127.0.0.1", 5000);
                client.CollectAndSendOrder();
            }
            else if (protocol == "UDP")
            {
                var client = new UDPClient("127.0.0.1", 5000);
                client.CollectAndSendOrder();
            }

            // Wacht tot de serverthread klaar is
            Console.WriteLine("Druk op een toets om af te sluiten...");
            Console.ReadKey();
        }
    }
}
