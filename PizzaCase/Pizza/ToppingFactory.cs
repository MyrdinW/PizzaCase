using System;

namespace PizzaCase.Pizza
{
    public static class ToppingFactory
    {
        public static Pizza AddTopping(Pizza pizza, string topping)
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
    }
}
