﻿namespace PizzaCase.Pizza
{
    public class DiabloPizza : Pizza
    {
        public override string Name => "Diablo";

        public override double Cost => 8.00;

        public override string Description => "Diablo Pizza";

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
