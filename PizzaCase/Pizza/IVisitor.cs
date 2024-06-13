namespace PizzaCase.Pizza
{
    public interface IVisitor
    {
        void Visit(Pizza pizza);
        void Visit(ToppingDecorator toppingDecorator);
    }
}
