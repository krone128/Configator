using System;
using System.Threading;
using Configator;

namespace ConsoleApplication1
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var obj = new ConfigatorObject();
            
            Configator.Configator.Get(obj);
            var newObj = Configator.Configator.Get<ConfigatorObject>();
            
            Console.ReadLine();
        }
    }
}
