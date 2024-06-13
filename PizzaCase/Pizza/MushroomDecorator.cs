using System.Collections.Generic;

namespace PizzaCase.Pizza
{
    public class MushroomDecorator : ToppingDecorator
    {
        private readonly double toppingPrice = 0.75;

        public MushroomDecorator(Pizza pizza) : base(pizza) { }

        public override double Cost => pizza.Cost + toppingPrice;

        public override string Description => pizza.Description + ", Mushrooms";

        protected override string GetToppingName() => "Mushrooms";
    }
}
