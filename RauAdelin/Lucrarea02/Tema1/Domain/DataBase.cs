using LanguageExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L01.Fake
{
    public static class DataBase
    {
        public static IEnumerable<Product> LoadProducts()
        {
            return new List<Product>
            {
                new Product(1, "Prod1", 1.5f),
                new Product(2, "Prod2", 2.5f),
                new Product(3, "Prod3", 3.5f),
                new Product(4, "Prod4", 4.5f),
                new Product(5, "Prod5", 5.5f),
            };
        }
    }
}
