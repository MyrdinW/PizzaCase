using System.Collections.Generic;

namespace PizzaCase.Pizza
{
    public abstract class Pizza
    {
        public abstract string Name { get; }
        public abstract double Cost { get; }
        public abstract string Description { get; }
        public virtual List<string> Toppings { get; set; } = new List<string>();

        public abstract void Accept(IVisitor visitor);
    }
}
