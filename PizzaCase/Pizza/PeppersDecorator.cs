namespace PizzaCase.Pizza
{
    public class PeppersDecorator : ToppingDecorator
    {
        public PeppersDecorator(Pizza pizza) : base(pizza) { }

        public override double Cost => pizza.Cost + 1.15;

        public override string Description => pizza.Description + ", Peppers";

        protected override string GetToppingName() => "Peppers";
    }
}
