using System;
using System.Collections.Generic;
using PizzaCase.Pizza;

namespace PizzaCase
{
    public class Order
    {
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string Postcode { get; set; }
        public string City { get; set; }
        public List<Pizza.Pizza> Pizzas { get; set; } = new List<Pizza.Pizza>();
        public DateTime OrderDateTime { get; set; }

        public override string ToString()
        {
            string orderString = $"{CustomerName}\n{Address}\n{Postcode}\n{City}\n{OrderDateTime}";
            foreach (var pizza in Pizzas)
            {
                orderString += $"\n{pizza.Name}\n{pizza.Toppings.Count}";
                foreach (var topping in pizza.Toppings)
                {
                    orderString += $"\n{topping}";
                }
            }
            return orderString;
        }

        public static Order FromString(string orderString)
        {
            var lines = orderString.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            Order order = new Order
            {
                CustomerName = lines[0],
                Address = lines[1],
                Postcode = lines[2],
                City = lines[3],
                OrderDateTime = DateTime.Parse(lines[4])
            };

            int index = 5;
            while (index < lines.Length)
            {
                var pizzaType = lines[index++];
                var quantity = int.Parse(lines[index++]);

                for (int q = 0; q < quantity; q++)
                {
                    var toppingsCount = int.Parse(lines[index++]);
                    Pizza.Pizza pizza = CreatePizzaFromType(pizzaType);

                    for (int i = 0; i < toppingsCount; i++)
                    {
                        var topping = lines[index++];
                        pizza = ToppingFactory.AddTopping(pizza, topping);
                    }

                    order.Pizzas.Add(pizza);
                }
            }

            return order;
        }

        private static Pizza.Pizza CreatePizzaFromType(string pizzaType)
        {
            PizzaFactory factory;
            switch (pizzaType)
            {
                case "Margherita":
                    factory = new MargheritaFactory();
                    break;
                case "Pepperoni":
                    factory = new PepperoniFactory();
                    break;
                case "Tonno":
                    factory = new TonnoFactory();
                    break;
                case "Diablo":
                    factory = new DiabloFactory();
                    break;
                default:
                    throw new Exception($"Onbekend pizza type: {pizzaType}");
            }
            return factory.CreatePizza();
        }
    }
}
