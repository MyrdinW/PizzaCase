using System;
using System.Collections.Generic;

namespace PizzaCase.Pizza
{
    public class Order
    {
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string Postcode { get; set; }
        public string City { get; set; }
        public List<Pizza> Pizzas { get; set; } = new List<Pizza>();
        public DateTime OrderDateTime { get; set; }
    }
}
