using PostSharpApp.Bll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostSharpApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var source = new MyClass();

            int result = source.DoSomething();
            Console.WriteLine($"Result = {result}");

            Console.Read();
        }
    }
}
