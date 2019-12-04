using System;


namespace PollyDemo
{
    public class MyClass
    {
        private int _count = 0;
        public int Execute()
        {
            Console.WriteLine("Inside Execute()");
            _count += 1;

            if (_count == 1)
                return 0;

            if (_count < 4)
            {
                throw new MyException();
            }
            else
                throw new MyOtherException();
            return 1;
        }
    }
}
