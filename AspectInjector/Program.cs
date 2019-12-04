using System;


namespace AspectInjector
{
    /// <summary>
    /// https://medium.com/@yurexus/aspect-oriented-programming-in-net-with-aspectinjector-cb56d8f80254
    /// https://github.com/pamidur/aspect-injector
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {

            MyClass service = new MyClass();
            int result = service.DoSomthing();

            Console.WriteLine($"The result is {result}");
            Console.Read();
        }


    }
}
