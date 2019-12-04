using System;


namespace AOP_Demo
{
    public class MyDecorator : IMyInterface
    {
        private IMyInterface _decorated;

        #region .ctor
        public MyDecorator(IMyInterface decorated)
        {
            _decorated = decorated;
        }
        #endregion

        #region IMyInterface implementation
        void IMyInterface.DoSomething()
        {
            try
            {
                Console.WriteLine("MyDecorator has been entered");
                _decorated.DoSomething();
            }
            catch(Exception e)
            {
                Console.WriteLine($"MyDecorator got exception finished {e}");
            }
            finally
            {
                Console.WriteLine("MyDecorator has been finished");
            }
        }
        #endregion
    }
}
