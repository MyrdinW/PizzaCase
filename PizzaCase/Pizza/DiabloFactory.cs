namespace PizzaCase.Pizza
{
    public class DiabloFactory : PizzaFactory
    {
        public override Pizza CreatePizza()
        {
            return new DiabloPizza();
        }
    }
}
