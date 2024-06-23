namespace PizzaCase.Pizza
{
    public class MushroomDecorator : ToppingDecorator
    {
        public MushroomDecorator(Pizza pizza) : base(pizza) { }

        public override double Cost => pizza.Cost + 0.75;

        public override string Description => pizza.Description + ", Mushrooms";

        protected override string GetToppingName() => "Mushrooms";
    }
}
