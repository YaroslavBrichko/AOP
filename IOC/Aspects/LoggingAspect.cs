using System;
using System.Collections.Generic;
using Unity.Interception.InterceptionBehaviors;
using Unity.Interception.PolicyInjection.Pipeline;

namespace IOC.Aspects
{
    public class LoggingAspect: IInterceptionBehavior
    {
    
        public bool WillExecute => true;

        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return Type.EmptyTypes;
        }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            // Before invoking the method on the original target.   
            WriteLog(string.Format("Invoking method {0} at {1}", input.MethodBase, DateTime.Now.ToLongTimeString()));
            // Invoke the next behavior in the chain. 
            IMethodReturn result = getNext()(input, getNext);
            // After invoking the method on the original target. 
            if (result.Exception != null)
            {
                WriteLog(string.Format("Method {0} threw exception {1} at {2}", input.MethodBase, result.Exception.Message, DateTime.Now.ToString()));
            }
            else
            {
                WriteLog(string.Format("Method {0} returned {1} at {2}", input.MethodBase, result.ReturnValue, DateTime.Now.ToString()));
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

