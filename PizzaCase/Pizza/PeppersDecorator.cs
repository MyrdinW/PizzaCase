using System.Collections.Generic;

namespace PizzaCase.Pizza
{
    public class PeppersDecorator : ToppingDecorator
    {
        private readonly double toppingPrice = 1.15;

        public PeppersDecorator(Pizza pizza) : base(pizza) { }

        public override double Cost => pizza.Cost + toppingPrice;

        public override string Description => pizza.Description + ", Pepers";

        protected override string GetToppingName() => "Pepers";
    }
}
