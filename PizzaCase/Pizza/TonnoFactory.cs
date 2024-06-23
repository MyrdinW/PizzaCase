namespace PizzaCase.Pizza
{
    public class TonnoFactory : PizzaFactory
    {
        public override Pizza CreatePizza()
        {
            return new TonnoPizza();
        }
    }
}
