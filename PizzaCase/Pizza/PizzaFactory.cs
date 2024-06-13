using System;
using System.Collections.Generic;

namespace PizzaCase.Pizza
{
    public class PizzaFactory
    {
        private static PizzaFactory _instance;

        private PizzaFactory() { }

        public static PizzaFactory Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PizzaFactory();
                }
                return _instance;
            }
        }

        public Pizza CreatePizza(string type)
        {
            switch (type.ToLower())
            {
                case "margherita":
                    return new MargheritaPizza();
                case "pepperoni":
                    return new PepperoniPizza();
                // Voeg andere soorten pizza's toe volgens hetzelfde patroon
                default:
                    throw new ArgumentException($"Onbekend pizzatype: {type}");
            }
        }

        public List<string> GetAvailablePizzaTypes()
        {
            // Voeg hier alle beschikbare pizzatypes toe
            return new List<string> { "Margherita", "Pepperoni" };
        }
    }
}