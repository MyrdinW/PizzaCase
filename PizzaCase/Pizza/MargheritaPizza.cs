﻿namespace PizzaCase.Pizza
{
    public class MargheritaPizza : Pizza
    {
        public override string Name => "Margherita";

        public override double Cost => 5.00;

        public override string Description => "Margherita Pizza";

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
