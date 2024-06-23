using PizzaCase.Pizza;
using PizzaCase.Security;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace PizzaCase.Networking
{
    public class TCPServer
    {
        private static TCPServer _instance;
        private readonly int port;

        private TCPServer(int port)
        {
            this.port = port;
        }

        public static TCPServer Instance(int port)
        {
            if (_instance == null)
            {
                _instance = new TCPServer(port);
            }
            return _instance;
        }

        public void Start()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            Console.WriteLine("TCP Server gestart.");

            try
            {
                while (true)
                {
                    using (TcpClient client = listener.AcceptTcpClient())
                    {
                        NetworkStream stream = client.GetStream();

                        byte[] buffer = new byte[1024];
                        int bytesRead = stream.Read(buffer, 0, buffer.Length);
                        string encryptedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        string message = Encryption.Decrypt(encryptedMessage);

                        string response = ProcessOrder(message);
                        string encryptedResponse = Encryption.Encrypt(response);
                        byte[] responseData = Encoding.UTF8.GetBytes(encryptedResponse);
                        stream.Write(responseData, 0, responseData.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Er trad een fout op in de server: " + ex.Message);
            }
            finally
            {
                listener.Stop();
            }
        }

        private string ProcessOrder(string order)
        {
            var lines = order.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            string customerName = lines[0];
            string address = lines[1];
            string postcode = lines[2];
            string city = lines[3];
            DateTime orderDateTime = DateTime.Parse(lines[4]);

            string orderOverview = $"Bestelling ontvangen: {customerName}\nAdres: {address}\nPostcode + Woonplaats: {postcode} {city}\nBestelling geplaatst op: {orderDateTime}\n";
            double totalPrice = 0;

            int index = 5;
            while (index < lines.Length)
            {
                string pizzaType = lines[index++];
                int quantity = int.Parse(lines[index++]);
                int toppingsCount = int.Parse(lines[index++]);

                PizzaFactory factory = CreateFactory(pizzaType);
                for (int i = 0; i < quantity; i++)
                {
                    Pizza.Pizza pizza = factory.CreatePizza();
                    string[] toppings = new string[toppingsCount];
                    for (int j = 0; j < toppingsCount; j++)
                    {
                        toppings[j] = lines[index++];
                        pizza = AddTopping(pizza, toppings[j]);
                    }

                    PriceVisitor visitor = new PriceVisitor();
                    pizza.Accept(visitor);
                    orderOverview += $"Pizza: {pizza.Name}, Toppings: {string.Join(", ", toppings)}, Prijs: {pizza.Cost}\n";
                    totalPrice += pizza.Cost;
                }
            }

            orderOverview += $"Totale Prijs: {totalPrice}";
            return orderOverview;
        }

        private Pizza.Pizza AddTopping(Pizza.Pizza pizza, string topping)
        {
            switch (topping)
            {
                case "Extra Cheese":
                    return new ExtraCheeseDecorator(pizza);
                case "Mushrooms":
                    return new MushroomDecorator(pizza);
                case "Onions":
                    return new OnionDecorator(pizza);
                case "Peppers":
                    return new PeppersDecorator(pizza);
                default:
                    throw new Exception($"Onbekende topping: {topping}");
            }
        }

        private PizzaFactory CreateFactory(string pizzaType)
        {
            switch (pizzaType)
            {
                case "Margherita":
                    return new MargheritaFactory();
                case "Pepperoni":
                    return new PepperoniFactory();
                case "Tonno":
                    return new TonnoFactory();
                case "Diablo":
                    return new DiabloFactory();
                default:
                    throw new Exception($"Onbekend pizza type: {pizzaType}");
            }
        }
    }
}
