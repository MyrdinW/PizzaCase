using System;
using System.Net.Sockets;
using System.Text;
using PizzaCase.Security;
using PizzaCase.Helpers;

namespace PizzaCase.Networking
{
    public class TCPClient
    {
        private readonly string serverAddress;
        private readonly int serverPort;

        public TCPClient(string serverAddress, int serverPort)
        {
            this.serverAddress = serverAddress;
            this.serverPort = serverPort;
        }

        public void SendOrder(string order)
        {
            Console.WriteLine("Te versturen bericht:");
            Console.WriteLine(order);
            string encryptedMessage = Encryption.Encrypt(order);
            Console.WriteLine(encryptedMessage);
            byte[] data = Encoding.UTF8.GetBytes(encryptedMessage);

            using (TcpClient client = new TcpClient(serverAddress, serverPort))
            {
                NetworkStream stream = client.GetStream();
                stream.Write(data, 0, data.Length);
                Console.WriteLine("Bestelling naar server verstuurd.");

                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string encryptedResponse = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                string response = Encryption.Decrypt(encryptedResponse);

                Console.WriteLine("Ontvangen bericht:");
                Console.WriteLine(response);
            }
        }

        public void CollectAndSendOrder()
        {
            string customerName = OrderHelper.ReadValidatedInput("Voer uw naam in: ", OrderHelper.ValidateName);
            string address = OrderHelper.ReadValidatedInput("Voer uw adres in: ", OrderHelper.ValidateAddress);
            string postcode = OrderHelper.ReadValidatedInput("Voer uw postcode in: ", OrderHelper.ValidatePostcode);
            string city = OrderHelper.ReadValidatedInput("Voer uw woonplaats in: ", OrderHelper.ValidateCity);

            string orderString = $"{customerName}\n{address}\n{postcode}\n{city}\n{DateTime.Now}";

            Console.WriteLine("Beschikbare pizza types: Margherita, Pepperoni, Tonno, Diablo");

            string pizzaTypesCountInput = OrderHelper.ReadValidatedInput("Hoeveel verschillende soorten pizza's wilt u bestellen? ", OrderHelper.ValidatePositiveInteger);
            int pizzaTypesCount = int.Parse(pizzaTypesCountInput);

            for (int i = 0; i < pizzaTypesCount; i++)
            {
                string pizzaType = OrderHelper.ReadValidatedInput($"Voer de naam in van pizza type {i + 1}: ", OrderHelper.ValidatePizzaType);

                string quantityInput = OrderHelper.ReadValidatedInput($"Hoeveel {pizzaType} pizza's wilt u bestellen? ", OrderHelper.ValidatePositiveInteger);
                int quantity = int.Parse(quantityInput);

                for (int q = 0; q < quantity; q++)
                {
                    orderString += $"\n{pizzaType}\n{quantity}";

                    Console.WriteLine($"Beschikbare toppings voor {pizzaType} pizza {q + 1}: Extra Cheese, Mushrooms, Onions, Peppers");
                    string toppingsCountInput = OrderHelper.ReadValidatedInput($"Hoeveel extra toppings wilt u voor {pizzaType} pizza {q + 1}? ", OrderHelper.ValidateNonNegativeInteger);
                    int toppingsCount = int.Parse(toppingsCountInput);

                    orderString += $"\n{toppingsCount}";
                    for (int j = 0; j < toppingsCount; j++)
                    {
                        string topping = OrderHelper.ReadValidatedInput($"Voer topping {j + 1} in voor {pizzaType} pizza {q + 1}: ", OrderHelper.ValidateTopping);
                        orderString += $"\n{topping}";
                    }
                }
            }

            SendOrder(orderString);
        }
    }
}
