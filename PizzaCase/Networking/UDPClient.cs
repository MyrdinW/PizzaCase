using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using PizzaCase.Security;
using PizzaCase.Pizza;
using PizzaCase.Helpers;
using System.Collections.Generic;
using PizzaCase.Serialization;

namespace PizzaCase.Networking
{
    public class UDPClient
    {
        private readonly string serverAddress;
        private readonly int serverPort;

        public UDPClient(string serverAddress, int serverPort)
        {
            this.serverAddress = serverAddress;
            this.serverPort = serverPort;
        }

        public void SendOrder(Order order)
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
                IPEndPoint serverEP = new IPEndPoint(IPAddress.Any, 0);
                byte[] responseBuffer = client.Receive(ref serverEP);
                string responseMessage = Encoding.UTF8.GetString(responseBuffer);
                Console.WriteLine(responseMessage);
            }
        }

        public void CollectAndSendOrder()
        {
            Order order = OrderHelper.CollectOrder();
            SendOrder(order);
        }
    }
}
