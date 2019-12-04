using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Threading.Tasks;
using Unity;
using Unity.Interception.PolicyInjection.Pipeline;
using Unity.Interception.PolicyInjection.Policies;

namespace IOC.Aspects
{
    public class LogUnityAsyncAttribute : HandlerAttribute
    {
        public override ICallHandler CreateHandler(IUnityContainer container)
        {
            return new LogUnityAsyncAttributeHandler();
        }

        private class LogUnityAsyncAttributeHandler : ICallHandler
        {
            private static ConcurrentDictionary<Type, Func<Task, IMethodInvocation, Task>> _wrapperCreators =
                        new ConcurrentDictionary<Type, Func<Task, IMethodInvocation, Task>>();

            #region ICallHandler implementation
            int ICallHandler.Order { get; set; }

            IMethodReturn ICallHandler.Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
            {
                WriteLog(String.Format("LogUnityAsyncAttributeHandler: Invoking method {0} at {1}", input.MethodBase, DateTime.Now.ToLongTimeString()));

                IMethodReturn value = getNext()(input, getNext);
                var method = input.MethodBase as MethodInfo;

                if (value.ReturnValue != null
                    && method != null
                    && typeof(Task).IsAssignableFrom(method.ReturnType))
                {
                    // if this method returns a Task, override the original return value
                    var task = (Task)value.ReturnValue;
                    return input.CreateMethodReturn(this.GetWrapperCreator(method.ReturnType)(task, input), value.Outputs);
                }

                return value;
            }
            #endregion

            private Func<Task, IMethodInvocation, Task> GetWrapperCreator(Type taskType)
            {
                return _wrapperCreators.GetOrAdd(
                    taskType,
                    (Type t) =>
                    {
                        if (t == typeof(Task))
                        {
                            return this.CreateWrapperTask;
                        }
                        else if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Task<>))
                        {
                            return (Func<Task, IMethodInvocation, Task>)this.GetType()
                                    .GetMethod("CreateGenericWrapperTask", BindingFlags.Instance | BindingFlags.NonPublic)
                                    .MakeGenericMethod(new Type[] { t.GenericTypeArguments[0] })
                                    .CreateDelegate(typeof(Func<Task, IMethodInvocation, Task>), this);

                        }
                        else
                        {
                        // other cases are not supported
                        return (task, _) => task;
                        }
                    });
            }

            private async Task CreateWrapperTask(Task task, IMethodInvocation input)
            {
                try
                {
                    await task.ConfigureAwait(false);
                    WriteLog(string.Format("LogUnityAttributeHandler: Successfully finished async operation {0}", input.MethodBase.Name));
                }
                catch (Exception e)
                {
                    WriteLog(string.Format("LogUnityAttributeHandler: Async operation {0} threw: {1}", input.MethodBase.Name, e));
                    throw;
                }
            }
            private Task CreateGenericWrapperTask<T>(Task task, IMethodInvocation input)
            {
                return this.DoCreateGenericWrapperTask<T>((Task<T>)task, input);
            }

            private async Task<T> DoCreateGenericWrapperTask<T>(Task<T> task, IMethodInvocation input)
            {
                try
                {
                    T value = await task.ConfigureAwait(false);
                    WriteLog(string.Format("LogUnityAttributeHandler: Successfully finished async operation {0} with value: {1}", input.MethodBase.Name, value));
                    return value;
                }
                catch (Exception e)
                {
                    WriteLog(string.Format("LogUnityAttributeHandler: Async operation {0} threw: {1}", input.MethodBase.Name, e));
                    throw;
                }
            }

            private void WriteLog(string message)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("LOG:" + message);
                Console.ResetColor();
            }
        }
    }
}
