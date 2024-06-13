namespace PizzaCase.Pizza
{
    public class PriceVisitor : IVisitor
    {
        public double TotalPrice { get; private set; }

        public void Visit(Pizza pizza)
        {
            TotalPrice += pizza.Cost;
        }

        public void Visit(ToppingDecorator toppingDecorator)
        {
            TotalPrice += toppingDecorator.Cost;
        }
    }
}
