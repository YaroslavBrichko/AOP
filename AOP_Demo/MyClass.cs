using System;


namespace AOP_Demo
{
    public class MyClass : IMyInterface
    {
        public void DoSomething()
        {
            Console.WriteLine("Inside: MyInterface.DoSomething()");
        }
    }
}
