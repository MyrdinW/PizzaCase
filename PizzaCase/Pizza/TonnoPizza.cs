using System.Collections.Generic;

namespace PizzaCase.Pizza
{
    public class TonnoPizza : Pizza
    {
        public override string Name => "Tonno";
        public override double Cost => 7.50; // Basisprijs
        public override string Description => "Tonno Pizza";

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
