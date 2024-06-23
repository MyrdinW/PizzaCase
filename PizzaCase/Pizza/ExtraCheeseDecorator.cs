namespace PizzaCase.Pizza
{
    public class ExtraCheeseDecorator : ToppingDecorator
    {
        public ExtraCheeseDecorator(Pizza pizza) : base(pizza) { }

        public override double Cost => pizza.Cost + 1.00;

        public override string Description => pizza.Description + ", Extra Cheese";

        protected override string GetToppingName() => "Extra Cheese";
    }
}
