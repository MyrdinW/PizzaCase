using PizzaCase.Pizza;
using System;

namespace PizzaCase.Helpers
{
    public static class OrderHelper
    {
        public static Order CollectOrder()
        {
            Order order = new Order();

            Console.Write("Voer uw naam in: ");
            order.CustomerName = Console.ReadLine();

            Console.Write("Voer uw adres in: ");
            order.Address = Console.ReadLine();

            Console.Write("Voer uw postcode in: ");
            order.Postcode = Console.ReadLine();

            Console.Write("Voer uw woonplaats in: ");
            order.City = Console.ReadLine();

            Console.WriteLine("Beschikbare pizza types: Margherita, Pepperoni, Tonno, Diablo");

            Console.Write("Hoeveel verschillende soorten pizza's wilt u bestellen? ");
            int pizzaTypesCount;
            while (!int.TryParse(Console.ReadLine(), out pizzaTypesCount) || pizzaTypesCount <= 0)
            {
                Console.WriteLine("Ongeldige invoer. Voer een positief geheel getal in.");
                Console.Write("Hoeveel verschillende soorten pizza's wilt u bestellen? ");
            }

            for (int i = 0; i < pizzaTypesCount; i++)
            {
                Console.Write($"Voer de naam in van pizza type {i + 1}: ");
                string pizzaType = Console.ReadLine();

                Console.Write($"Hoeveel {pizzaType} pizza's wilt u bestellen? ");
                int quantity;
                while (!int.TryParse(Console.ReadLine(), out quantity) || quantity <= 0)
                {
                    Console.WriteLine("Ongeldige invoer. Voer een positief geheel getal in.");
                    Console.Write($"Hoeveel {pizzaType} pizza's wilt u bestellen? ");
                }

                for (int q = 0; q < quantity; q++)
                {
                    Pizza.Pizza pizza = CreatePizza(pizzaType);

                    Console.WriteLine($"Beschikbare toppings voor {pizzaType} pizza {q + 1}: Extra Cheese, Mushrooms, Onions, Peppers");
                    Console.Write($"Hoeveel extra toppings wilt u voor {pizzaType} pizza {q + 1}? ");
                    int toppingsCount;
                    while (!int.TryParse(Console.ReadLine(), out toppingsCount) || toppingsCount < 0)
                    {
                        Console.WriteLine("Ongeldige invoer. Voer een geheel getal in groter of gelijk aan 0.");
                        Console.Write($"Hoeveel extra toppings wilt u voor {pizzaType} pizza {q + 1}? ");
                    }

                    for (int j = 0; j < toppingsCount; j++)
                    {
                        Console.Write($"Voer topping {j + 1} in voor {pizzaType} pizza {q + 1}: ");
                        string topping = Console.ReadLine();
                        pizza = AddTopping(pizza, topping);
                    }

                    order.Pizzas.Add(pizza);
                }
            }

            order.OrderDateTime = DateTime.Now;
            return order;
        }

        private static Pizza.Pizza CreatePizza(string pizzaType)
        {
            switch (pizzaType)
            {
                case "Margherita":
                    return new MargheritaPizza();
                case "Pepperoni":
                    return new PepperoniPizza();
                case "Tonno":
                    return new TonnoPizza();
                case "Diablo":
                    return new DiabloPizza();
                default:
                    throw new Exception($"Onbekend pizza type: {pizzaType}");
            }
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
                    return pizza;
            }
        }
    }
}
