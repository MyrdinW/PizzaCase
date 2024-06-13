using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using PizzaCase.Security;
using PizzaCase.Pizza;
using PizzaCase.Serialization;
using System.Collections.Generic;

namespace PizzaCase.Networking
{
    public class NetworkClient
    {
        private readonly string serverAddress;
        private readonly int serverPort;
        private readonly string protocol; // "TCP" of "UDP"

        public NetworkClient(string serverAddress, int serverPort, string protocol)
        {
            this.serverAddress = serverAddress;
            this.serverPort = serverPort;
            this.protocol = protocol;
        }

        public void SendOrder(Order order)
        {
            if (protocol == "TCP")
            {
                SendOrderTCP(order);
            }
            else if (protocol == "UDP")
            {
                SendOrderUDP(order);
            }
        }

        private void SendOrderTCP(Order order)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                Converters = new List<JsonConverter> { new CustomPizzaConverter() },
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            string message = JsonConvert.SerializeObject(order, settings);
            string encryptedMessage = Encryption.Encrypt(message);
            byte[] data = Encoding.UTF8.GetBytes(encryptedMessage);

            using (TcpClient client = new TcpClient(serverAddress, serverPort))
            using (NetworkStream stream = client.GetStream())
            {
                // Verstuur eerst de lengte van het bericht
                byte[] dataLength = BitConverter.GetBytes(data.Length);
                stream.Write(dataLength, 0, dataLength.Length);

                // Verstuur daarna het bericht
                stream.Write(data, 0, data.Length);
                Console.WriteLine("Bestelling naar server verstuurd.");

                // Wacht op bevestiging van de server
                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string confirmationMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Bevestiging van server: {confirmationMessage}");
            }
        }

        private void SendOrderUDP(Order order)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                Converters = new List<JsonConverter> { new CustomPizzaConverter() },
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            string message = JsonConvert.SerializeObject(order, settings);
            string encryptedMessage = Encryption.Encrypt(message);
            byte[] data = Encoding.UTF8.GetBytes(encryptedMessage);

            using (UdpClient client = new UdpClient())
            {
                client.Send(data, data.Length, serverAddress, serverPort);
                Console.WriteLine("Bestelling via UDP naar server verstuurd.");

                // Wacht op bevestiging van de server
                IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, serverPort);
                byte[] buffer = client.Receive(ref remoteEP);
                string confirmationMessage = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                Console.WriteLine($"Bevestiging van server: {confirmationMessage}");
            }
        }
    }
}
