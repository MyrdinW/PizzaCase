namespace PizzaCase.Pizza
{
    public class OnionDecorator : ToppingDecorator
    {
        public OnionDecorator(Pizza pizza) : base(pizza) { }

        public override double Cost => pizza.Cost + 0.50;

        public override string Description => pizza.Description + ", Onions";

        protected override string GetToppingName() => "Onions";
    }
}
