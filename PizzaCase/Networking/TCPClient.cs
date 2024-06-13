using System;
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
    public class TCPClient
    {
        private readonly string serverAddress;
        private readonly int serverPort;
        private readonly string protocol;

        public TCPClient(string serverAddress, int serverPort, string protocol)
        {
            this.serverAddress = serverAddress;
            this.serverPort = serverPort;
            this.protocol = protocol;
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

            using (TcpClient client = new TcpClient(serverAddress, serverPort))
            {
                NetworkStream stream = client.GetStream();
                byte[] dataLength = BitConverter.GetBytes(data.Length);
                stream.Write(dataLength, 0, dataLength.Length);
                stream.Write(data, 0, data.Length);
                Console.WriteLine("Bestelling naar server verstuurd.");

                // Wacht op bevestiging van de server
                byte[] responseBuffer = new byte[1024];
                int bytesRead = stream.Read(responseBuffer, 0, responseBuffer.Length);
                string responseMessage = Encoding.UTF8.GetString(responseBuffer, 0, bytesRead);
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
