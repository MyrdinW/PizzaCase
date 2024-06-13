using System;
using PizzaCase.Pizza;

namespace PizzaCase.Security
{
    public class SecurePizzaFactory
    {
        public static Pizza.Pizza CreatePizza(string type)
        {
            Pizza.Pizza pizza = null;
            switch (type)
            {
                case "Margherita":
                    pizza = new MargheritaPizza();
                    break;
                case "Pepperoni":
                    pizza = new PepperoniPizza();
                    break;
                default:
                    throw new ArgumentException("Onbekende pizza type");
            }
            return pizza;
        }
    }
}
