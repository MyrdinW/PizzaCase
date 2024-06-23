using PizzaCase.Pizza;
using System;
using System.Text.RegularExpressions;

namespace PizzaCase.Helpers
{
    public static class OrderHelper
    {
        public static string CollectOrder()
        {
            string customerName = ReadValidatedInput("Voer uw naam in: ", ValidateName);
            string address = ReadValidatedInput("Voer uw adres in: ", ValidateAddress);
            string postcode = ReadValidatedInput("Voer uw postcode in: ", ValidatePostcode);
            string city = ReadValidatedInput("Voer uw woonplaats in: ", ValidateCity);

            string orderString = $"{customerName}\n{address}\n{postcode}\n{city}\n{DateTime.Now}";

            Console.WriteLine("Beschikbare pizza types: Margherita, Pepperoni, Tonno, Diablo");

            string pizzaTypesCountInput = ReadValidatedInput("Hoeveel verschillende soorten pizza's wilt u bestellen? ", ValidatePositiveInteger);
            int pizzaTypesCount = int.Parse(pizzaTypesCountInput);

            for (int i = 0; i < pizzaTypesCount; i++)
            {
                string pizzaType = ReadValidatedInput($"Voer de naam in van pizza type {i + 1}: ", ValidatePizzaType);

                string quantityInput = ReadValidatedInput($"Hoeveel {pizzaType} pizza's wilt u bestellen? ", ValidatePositiveInteger);
                int quantity = int.Parse(quantityInput);

                PizzaFactory factory = CreateFactory(pizzaType);

                for (int q = 0; q < quantity; q++)
                {
                    Pizza.Pizza pizza = factory.CreatePizza();
                    orderString += $"\n{pizzaType}\n{quantity}";

                    Console.WriteLine($"Beschikbare toppings voor {pizzaType} pizza {q + 1}: Extra Cheese, Mushrooms, Onions, Peppers");
                    string toppingsCountInput = ReadValidatedInput($"Hoeveel extra toppings wilt u voor {pizzaType} pizza {q + 1}? ", ValidateNonNegativeInteger);
                    int toppingsCount = int.Parse(toppingsCountInput);

                    orderString += $"\n{toppingsCount}";
                    for (int j = 0; j < toppingsCount; j++)
                    {
                        string topping = ReadValidatedInput($"Voer topping {j + 1} in voor {pizzaType} pizza {q + 1}: ", ValidateTopping);
                        pizza = AddTopping(pizza, topping);
                        orderString += $"\n{topping}";
                    }
                }
            }

            return orderString;
        }

        private static Pizza.Pizza AddTopping(Pizza.Pizza pizza, string topping)
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

        private static PizzaFactory CreateFactory(string pizzaType)
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

        public static string ReadValidatedInput(string prompt, Func<string, bool> validate)
        {
            string input;
            do
            {
                Console.Write(prompt);
                input = Console.ReadLine();
                if (!validate(input))
                {
                    Console.WriteLine("Ongeldige invoer. Probeer het opnieuw.");
                }
            } while (!validate(input));
            return input;
        }

        public static bool ValidateName(string name) => !string.IsNullOrWhiteSpace(name) && Regex.IsMatch(name, @"^[a-zA-Z\s]+$");

        public static bool ValidateAddress(string address) => !string.IsNullOrWhiteSpace(address) && Regex.IsMatch(address, @"^[a-zA-Z0-9\s]+$");

        public static bool ValidatePostcode(string postcode) => Regex.IsMatch(postcode, @"^\d{4}\s?[A-Z]{2}$", RegexOptions.IgnoreCase);

        public static bool ValidateCity(string city) => !string.IsNullOrWhiteSpace(city) && Regex.IsMatch(city, @"^[a-zA-Z\s]+$");

        public static bool ValidatePositiveInteger(string input) => int.TryParse(input, out int value) && value > 0;

        public static bool ValidateNonNegativeInteger(string input) => int.TryParse(input, out int value) && value >= 0;

        public static bool ValidatePizzaType(string pizzaType) => pizzaType == "Margherita" || pizzaType == "Pepperoni" || pizzaType == "Tonno" || pizzaType == "Diablo";

        public static bool ValidateTopping(string topping) => topping == "Extra Cheese" || topping == "Mushrooms" || topping == "Onions" || topping == "Peppers";
    }
}
