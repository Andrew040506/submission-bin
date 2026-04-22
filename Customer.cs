using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Customer
    {
        public string Name { get; set; }
        public bool IsPremium { get; set; }

        public Customer(string name, bool isPremium)
        {
            Name = name;
            IsPremium = isPremium;
        }
    }
}
