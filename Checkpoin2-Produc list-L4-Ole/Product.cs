using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkpoin2_Produc_list_L4_Ole
{
    class Product
    {
        public Category partOf { get; set; }    // The product is part of a category
        public string Name { get; set; }
        public int Price { get; set; }

        // Constructor
        public Product(Category partOf, string name, int price)
        {
            this.partOf = partOf;
            Name = name;
            Price = price;
        }

        // Print method
        public void print(int pad = 10)
        {
            Console.WriteLine(Name.PadRight(pad) + (" $" + Price.ToString()).PadLeft(pad) + "     " + partOf.Name);
        }

    }
}
