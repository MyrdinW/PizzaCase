namespace PizzaCase.Pizza
{
    public class MargheritaFactory : PizzaFactory
    {
        public override Pizza CreatePizza()
        {
            return new MargheritaPizza();
        }
    }
}
