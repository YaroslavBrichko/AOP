using AspectInjector.Aspects;
using System;
using System.Threading.Tasks;

namespace AspectInjector
{
    public class MyClass 
    {
        [Log]
        public int DoSomthing()
        {
            Console.WriteLine("Inside 'DoSomthing()' method");
            return 1;
        }


        public async Task<int> DoSomthingAsync()
        {
            Console.WriteLine("Inside 'DoSomthingAsync()' method");
            await Task.Yield();
            return 1;
        }
    }
}
