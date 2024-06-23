using PizzaCase.Pizza;
using PizzaCase.Security;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace PizzaCase.Networking
{
    public class UDPServer
    {
        private static UDPServer _instance;
        private readonly int port;

        private UDPServer(int port)
        {
            this.port = port;
        }

        public static UDPServer Instance(int port)
        {
            if (_instance == null)
            {
                _instance = new UDPServer(port);
            }
            return _instance;
        }

        public void Start()
        {
            UdpClient listener = new UdpClient(port);
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, port);
            Console.WriteLine("UDP Server gestart.");

            try
            {
                while (true)
                {
                    byte[] bytes = listener.Receive(ref groupEP);
                    string encryptedMessage = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
                    string message = Encryption.Decrypt(encryptedMessage);

                    string response = ProcessOrder(message);
                    string encryptedResponse = Encryption.Encrypt(response);
                    byte[] responseData = Encoding.UTF8.GetBytes(encryptedResponse);
                    listener.Send(responseData, responseData.Length, groupEP);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Er trad een fout op in de server: " + ex.Message);
            }
            finally
            {
                listener.Close();
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
