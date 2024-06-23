namespace PizzaCase.Pizza
{
    public class PepperoniFactory : PizzaFactory
    {
        public override Pizza CreatePizza()
        {
            return new PepperoniPizza();
        }
    }
}
