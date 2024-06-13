using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PizzaCase.Pizza;

namespace PizzaCase.Serialization
{
    public class CustomPizzaConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(Pizza.Pizza).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jsonObject = JObject.Load(reader);
            string typeName = jsonObject["Type"]?.ToString();
            Pizza.Pizza pizza;

            switch (typeName)
            {
                case "MargheritaPizza":
                    pizza = new MargheritaPizza();
                    break;
                case "PepperoniPizza":
                    pizza = new PepperoniPizza();
                    break;
                case "TonnoPizza":
                    pizza = new TonnoPizza();
                    break;
                case "DiabloPizza":
                    pizza = new DiabloPizza();
                    break;
                case "ExtraCheeseDecorator":
                    var basePizzaCheese = jsonObject["BasePizza"].ToObject<Pizza.Pizza>(serializer);
                    pizza = new ExtraCheeseDecorator(basePizzaCheese);
                    break;
                case "MushroomDecorator":
                    var basePizzaMushroom = jsonObject["BasePizza"].ToObject<Pizza.Pizza>(serializer);
                    pizza = new MushroomDecorator(basePizzaMushroom);
                    break;
                case "OnionDecorator":
                    var basePizzaOnion = jsonObject["BasePizza"].ToObject<Pizza.Pizza>(serializer);
                    pizza = new OnionDecorator(basePizzaOnion);
                    break;
                case "PeppersDecorator":
                    var basePizzaPeppers = jsonObject["BasePizza"].ToObject<Pizza.Pizza>(serializer);
                    pizza = new PeppersDecorator(basePizzaPeppers);
                    break;
                default:
                    throw new Exception($"Unknown pizza type: {typeName}");
            }

            serializer.Populate(jsonObject.CreateReader(), pizza);
            return pizza;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var originalReferenceLoopHandling = serializer.ReferenceLoopHandling;
            serializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            JObject jsonObject = new JObject
            {
                { "Type", value.GetType().Name }
            };

            if (value is Pizza.Pizza pizza)
            {
                if (pizza is ToppingDecorator decorator)
                {
                    jsonObject.Add("BasePizza", JToken.FromObject(decorator.GetBasePizza(), serializer));
                }
                foreach (var prop in pizza.GetType().GetProperties())
                {
                    if (prop.CanRead && prop.GetValue(pizza) != null)
                    {
                        jsonObject[prop.Name] = JToken.FromObject(prop.GetValue(pizza), serializer);
                    }
                }
            }

            jsonObject.WriteTo(writer);

            serializer.ReferenceLoopHandling = originalReferenceLoopHandling;
        }
    }
}
