using Polly;
using System;


namespace PollyDemo.Policies
{
    public abstract class BasePolicy
    {
        public abstract Policy<int> Build();
        protected void Log(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("LOG:" + message);
            Console.ResetColor();
        }
    }
}
