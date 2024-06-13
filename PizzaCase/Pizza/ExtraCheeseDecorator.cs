using System.Collections.Generic;

namespace PizzaCase.Pizza
{
    public class ExtraCheeseDecorator : ToppingDecorator
    {
        private readonly double toppingPrice = 1.25;

        public ExtraCheeseDecorator(Pizza pizza) : base(pizza) { }

        public override double Cost => pizza.Cost + toppingPrice;

        public override string Description => pizza.Description + ", Extra Cheese";

        protected override string GetToppingName() => "Extra Cheese";
    }
}
