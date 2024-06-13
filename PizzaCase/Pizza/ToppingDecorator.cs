using PizzaCase.Pizza;
using System.Collections.Generic;
using System;

namespace PizzaCase.Pizza
{
    public abstract class ToppingDecorator : Pizza
    {
        protected Pizza pizza;

        public ToppingDecorator(Pizza pizza)
        {
            this.pizza = pizza ?? throw new ArgumentNullException(nameof(pizza));
        }

        public Pizza GetBasePizza()
        {
            return this.pizza;
        }

        public override string Name => pizza.Name;

        public override List<string> Toppings
        {
            get
            {
                var toppings = new List<string>(pizza.Toppings);
                if (!toppings.Contains(GetToppingName()))
                {
                    toppings.Add(GetToppingName());
                }
                return toppings;
            }
            set => pizza.Toppings = value;
        }

        protected abstract string GetToppingName();

        public abstract override string Description { get; }
        public abstract override double Cost { get; }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}