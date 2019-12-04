using System;
using Unity;
using Unity.Interception.PolicyInjection.Pipeline;
using Unity.Interception.PolicyInjection.Policies;

namespace IOC.Aspects
{
    public class LogUnityAttribute : HandlerAttribute
    {
        public override ICallHandler CreateHandler(IUnityContainer container)
        {
            return new LogUnityAttributeHandler();
        }
    }

    class LogUnityAttributeHandler : ICallHandler
    {
        int ICallHandler.Order { get; set ; }

        IMethodReturn ICallHandler.Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            // Before invoking the method on the original target.   
            WriteLog(string.Format("LogUnityAttributeHandler: Invoking method {0} at {1}", input.MethodBase, DateTime.Now.ToLongTimeString()));
            // Invoke the next behavior in the chain. 
            var result = getNext()(input, getNext);
            // After invoking the method on the original target. 
            if (result.Exception != null)
            {
                WriteLog(string.Format("LogUnityAttributeHandler: Method {0} threw exception {1} at {2}", input.MethodBase, result.Exception.Message, DateTime.Now.ToString()));
            }
            else
            {
                WriteLog(string.Format("LogUnityAttributeHandler: Method {0} returned {1} at {2}", input.MethodBase, result.ReturnValue, DateTime.Now.ToString()));
            }

            return result;
        }

        private void WriteLog(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("LOG:" + message);
            Console.ResetColor();
        }
    }
}
