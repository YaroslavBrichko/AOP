using PostSharpApp.Aspects;
using System;
using System.Threading.Tasks;

namespace PostSharpApp.Bll
{
    public class MyClass 
    {
        [LoggingAspect]
        [RetryAspect(3)]
        public int DoSomething()
        {
            Console.WriteLine("Inside 'DoSomthing()' method");
            return 1;
        }

        public async Task<int> DoSomethingAsync()
        {
            Console.WriteLine("Inside 'DoSomthingAsync()' method");
            await Task.Yield();
            return 1;
        }
    }
}
