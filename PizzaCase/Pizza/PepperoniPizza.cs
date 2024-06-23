namespace PizzaCase.Pizza
{
    public class PepperoniPizza : Pizza
    {
        public override string Name => "Pepperoni";

        public override double Cost => 7.00;

        public override string Description => "Pepperoni Pizza";

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
