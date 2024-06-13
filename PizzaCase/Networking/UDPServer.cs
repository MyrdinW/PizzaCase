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
    public class UDPServer
    {
        private static UDPServer instance;
        private static readonly object lockObj = new object();
        private readonly int port;

        private UDPServer(int port)
        {
            this.port = port;
        }

        public static UDPServer Instance(int port)
        {
            if (instance == null)
            {
                lock (lockObj)
                {
                    if (instance == null)
                    {
                        instance = new UDPServer(port);
                    }
                }
            }
            return instance;
        }

        public void Start()
        {
            UdpClient listener = new UdpClient(port);
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, port);
            Console.WriteLine("UDP Server gestart.");
            SecureLogger.Log("UDP Server gestart.");

            try
            {
                while (true)
                {
                    byte[] buffer = listener.Receive(ref groupEP);
                    Console.WriteLine($"Ontvangen {buffer.Length} bytes.");
                    SecureLogger.Log($"Ontvangen {buffer.Length} bytes.");

                    string encryptedMessage = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
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
                    listener.Send(confirmationData, confirmationData.Length, groupEP);
                    SecureLogger.Log("Bevestiging naar client verzonden.");
                }
            }
            catch (Exception ex)
            {
                SecureLogger.LogError("Er trad een fout op in de server.", ex);
            }
            finally
            {
                listener.Close();
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
