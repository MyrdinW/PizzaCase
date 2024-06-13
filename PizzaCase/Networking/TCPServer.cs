using PizzaCase.Security;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using PizzaCase.Pizza;
using PizzaCase.Serialization;
using System.Collections.Generic;

namespace PizzaCase.Networking
{
    public class TCPServer
    {
        private static TCPServer instance;
        private static readonly object lockObj = new object();
        private readonly int port;

        private TCPServer(int port)
        {
            this.port = port;
        }

        public static TCPServer Instance(int port)
        {
            if (instance == null)
            {
                lock (lockObj)
                {
                    if (instance == null)
                    {
                        instance = new TCPServer(port);
                    }
                }
            }
            return instance;
        }

        public void Start()
        {
            TcpListener server = null;
            try
            {
                server = new TcpListener(IPAddress.Any, port);
                server.Start();

                Console.WriteLine("TCP Server gestart.");
                SecureLogger.Log("TCP Server gestart.");

                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Client verbonden.");
                    SecureLogger.Log("Client verbonden.");

                    NetworkStream stream = client.GetStream();

                    byte[] dataLength = new byte[4];
                    stream.Read(dataLength, 0, 4);
                    int length = BitConverter.ToInt32(dataLength, 0);

                    byte[] data = new byte[length];
                    stream.Read(data, 0, length);

                    string encryptedMessage = Encoding.UTF8.GetString(data);
                    string message = Encryption.Decrypt(encryptedMessage);

                    JsonSerializerSettings settings = new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All,
                        Converters = new List<JsonConverter> { new CustomPizzaConverter() },
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    };

                    Order order = JsonConvert.DeserializeObject<Order>(message, settings);
                    ProcessOrder(order);

                    // Verzend een bevestiging naar de client
                    string confirmationMessage = "Bevestiging van server: Bestelling ontvangen en verwerkt.";
                    byte[] confirmationData = Encoding.UTF8.GetBytes(confirmationMessage);
                    stream.Write(confirmationData, 0, confirmationData.Length);
                    SecureLogger.Log("Bevestiging naar client verzonden.");

                    client.Close();
                }
            }
            catch (Exception ex)
            {
                SecureLogger.LogError("Er trad een fout op in de server.", ex);
            }
            finally
            {
                server?.Stop();
            }
        }

        private void ProcessOrder(Order order)
        {
            // Verwerk de bestelling
            Console.WriteLine($"Bestelling ontvangen: {order.CustomerName}, {order.Pizzas.Count} pizza(s)");
            Console.WriteLine($"Adres: {order.Address}");
            Console.WriteLine($"Postcode + Woonplaats: {order.Postcode} {order.City}");
            Console.WriteLine($"Bestelling geplaatst op: {order.OrderDateTime}");
            SecureLogger.Log($"Bestelling ontvangen: {order.CustomerName}, {order.Pizzas.Count} pizza(s)");
            PriceVisitor visitor = new PriceVisitor();
            foreach (var pizza in order.Pizzas)
            {
                pizza.Accept(visitor);
                Console.WriteLine($"Pizza: {pizza.Name}, Toppings: {string.Join(", ", pizza.Toppings)}, Prijs: {pizza.Cost.ToString("0.00")}");
                SecureLogger.Log($"Pizza: {pizza.Name}, Toppings: {string.Join(", ", pizza.Toppings)}, Prijs: {pizza.Cost.ToString("0.00")}");
            }
            Console.WriteLine($"Totale Prijs: {visitor.TotalPrice.ToString("0.00")}");
            SecureLogger.Log($"Totale Prijs: {visitor.TotalPrice.ToString("0.00")}");
        }
    }
}
