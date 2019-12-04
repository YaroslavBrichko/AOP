using System;
using System.Threading.Tasks;

namespace IOC
{
    public class MyClass : IMyInterface
    {
        int IMyInterface.Execute()
        {
            Console.WriteLine("Inside IMyInterface.Execute()");
            return 1;
        }

        async Task<int> IMyInterface.ExecuteAsync()
        {
            Console.WriteLine("Inside IMyInterface.ExecuteAsync()");
            await Task.Yield();
            return 1;
        }
    }
}
