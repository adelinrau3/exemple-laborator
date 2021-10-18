using LanguageExt;
using System;
using L01.Fake;
using System.Linq;
using static L01.Domain.AppDomain;
using static L01.Domain.CartState;
using L01.Extensions;
using L01.Domain;

namespace L01
{
    class Program
    {
        static void Main(string[] args)
        {
            var cart = (CreateCart() as EmptyCart).Cart;
            while (true)
            {
                var (product, quantity) = RequestItem();
                var result = AddItemToCart(cart, product, quantity);
                var message = string.Empty; // Mixed declarations and expressions in destruction is currently in Preview
                (cart, message) = result switch
                {
                    ValidCart a => (a.Cart, ""),
                    InvalidCart a => (a.Cart, a.Message + (Environment.NewLine * (StringMultiplication)2))
                };
                Displayitems(cart);

                Console.Write(message);
                if (RequestPayment())
                {
                    PayCart(cart);
                    Console.WriteLine($"Thanks");
                    break;
                }
            }
        }

        public static Unit Displayitems(Cart cart)
        {
            Console.WriteLine("Your items:");
            Console.Write(cart.Items
                .OrderBy(a => a.Product.Id)
                .Select(a => $"\t{a.Product.Id} / {a.Product.Name} / {a.Product.Price} / {a.Quantity}" + Environment.NewLine)
                .Aggregate((a, b) => a + b));
            Console.Write("Total to pay: ");
            Console.WriteLine(cart.Items.Select(a => a.Product.Price * a.Quantity).Aggregate((a, b) => a + b));
            Console.WriteLine();
            return Unit.Default;
        }

        public static string RequestCredentials()
        {
            Console.Write("Enter password: ");
            return Console.ReadLine();
        }

        public static (Option<Product>, int) RequestItem()
        {
            var products = DataBase.LoadProducts();
            products.Iter(a => Console.WriteLine($"Id: {a.Id}, Price: {a.Price}, Name: {a.Name}"));
            Console.WriteLine("Select an item Id and then input the quantity");
            Console.Write("Product Id: ");
            var productId = Console.ReadLine().Trim();
            Console.Write("Quantity: ");
            var quantity = int.Parse(Console.ReadLine().Trim());
            var product = products.FirstOrDefault(a => a.Id.ToString() == productId);
            return (product, quantity);
        }

        public static bool RequestPayment()
        {
            Console.WriteLine("Please insert \"y\" if you want to pay:");
            return Console.ReadLine().Trim().ToLower() switch
            {
                "y" => true,
                "Y" => true,
                _ => false
            };
        }
    }
}
