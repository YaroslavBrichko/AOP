using Polly;
using Polly.Wrap;
using PollyDemo.Policies;
using System;


namespace PollyDemo
{
    /// <summary>
    /// https://github.com/App-vNext/Polly
    /// https://github.com/App-vNext/Polly-Samples
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            var retryPolicy = new RetryPolicy(3).Build();
            var fallbackPolicy = new FallBackPolicy(-1).Build();

            PolicyWrap<int> policyWrap = Policy.Wrap<int>(fallbackPolicy, retryPolicy);

            MyClass obj = new MyClass();
            int result = policyWrap.Execute(() => obj.Execute());

            Console.WriteLine($"Result is {result}");
            Console.Read();
        }
    }
}
