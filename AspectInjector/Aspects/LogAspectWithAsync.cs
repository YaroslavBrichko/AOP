using AspectInjector.Broker;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace AspectInjector.Aspects
{
    [Aspect(Scope.Global)]
    public class LogAspectWithAsync
    {
        private static ConcurrentDictionary<Type, Func<Task, string, Task>> _methods = new ConcurrentDictionary<Type, Func<Task, string, Task>>();


        [Advice(Kind.Around, Targets = Target.Method)]
        public object HandleAsync([Argument(Source.Name)] string name,
                                  [Argument(Source.Arguments)] object[] arguments,
                                  [Argument(Source.Target)] Func<object[], object> method)
        {
            Log($"LogAspectAsync: inside 'HandleAsync()' - Executing method {name}");

            var sw = Stopwatch.StartNew();
            object result = null;
            bool isTask = false;

            try
            {
                //call the method
                result = method(arguments);

                isTask = result != null && typeof(Task).IsAssignableFrom(result.GetType());
                if (isTask)
                {
                    var asyncMethodWrapper = _methods.GetOrAdd(result.GetType(), (type) =>
                    {
                          return (Func<Task, string, Task>)this.GetType()
                              .GetMethod("CreateGenericWrapperTask", BindingFlags.Instance | BindingFlags.NonPublic)
                              .MakeGenericMethod(new Type[] { type.GenericTypeArguments[0] })
                              .CreateDelegate(typeof(Func<Task, string, Task>), this);
                    });
                    return asyncMethodWrapper((Task)result, name);
                }
                else
                    return result;
            }
            catch
            {
                throw;
            }
            finally
            {
                if(!isTask)
                    Log($"LogAspectAsync: inside HandleAsync() - Executed sync method {name} in {sw.ElapsedMilliseconds} ms");
            }

        }


        private Task CreateGenericWrapperTask<T>(Task task, string method)
        {
            return this.DoCreateGenericWrapperTask<T>((Task<T>)task, method);
        }

        private async Task<T> DoCreateGenericWrapperTask<T>(Task<T> task, string method)
        {
            try
            {
                T value = await task.ConfigureAwait(false);
                Log(string.Format("LogAspectAsync: Successfully finished async generic operation {0} with value: {1}", method, value));
                return value;
            }
            catch (Exception e)
            {
                Log(string.Format("LogAspectAsync: Async generic operation {0} threw: {1}", method, e));
                throw;
            }
            finally
            {
                Log($"LogAspectAsync: inside HandleAsync() - Executed async method {method}");
            }
        }

        private void Log(string text)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("LOG:" + text);
            Console.ResetColor();
        }
    }
}
