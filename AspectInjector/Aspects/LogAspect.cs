using AspectInjector.Broker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspectInjector.Aspects
{
    [Aspect(Scope.Global, Factory = typeof(MyFactory))]
    public class LogAspect
    {
        [Advice(Kind.Before, Targets = Target.Method)]
        public void OnEnter([Argument(Source.Name)] string name)
        {
            Log($"LogAspect: OnEnter before method {name}");
        }

        [Advice(Kind.After, Targets = Target.Method)]
        public void OnAfter([Argument(Source.Name)] string name, [Argument(Source.ReturnValue)] object value)
        {
            Log($"LogAspect: OnAfter before method {name}");
        }

        private void Log(string text)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("LOG:" + text);
            Console.ResetColor();
        }
    }

    public class MyFactory
    {
        public static object GetInstance(Type aspectType)
        {
            // Here you can implement any instantiation approach, this one is shown for the sake of simplicity
            if (aspectType == typeof(LogAspect))
            {
                return new LogAspect();
            }
            throw new ArgumentException($"Unknown aspect type '{aspectType.FullName}'");
        }
    }
}
/*
          [Advice(Kind.Around, Targets = Target.Method)]
        public object Handle([Argument(Source.Name)] string name,
                             [Argument(Source.Arguments)] object[] arguments,
                             [Argument(Source.Target)] Func<object[], object> method)
        {
            Log($"LogAspect: inside 'Handle()' - start method {name}");

            try
            {
                return method(arguments);
            }
            catch
            {
                throw;
            }
            finally
            {
                Log($"LogAspect: inside Handle() - end of method {name}");
            }

        }
 */
