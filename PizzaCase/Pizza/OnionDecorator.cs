using System.Collections.Generic;

namespace PizzaCase.Pizza
{
    public class OnionDecorator : ToppingDecorator
    {
        private readonly double toppingPrice = 0.40;

        public OnionDecorator(Pizza pizza) : base(pizza) { }

        public override double Cost => pizza.Cost + toppingPrice;

        public override string Description => pizza.Description + ", Uien";

        protected override string GetToppingName() => "Uien";
    }
}
