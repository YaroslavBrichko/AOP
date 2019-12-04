using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Unity.Interception.InterceptionBehaviors;
using Unity.Interception.PolicyInjection.Pipeline;

namespace IOC.Aspects
{
    public class LoggingAspectWithAsync : IInterceptionBehavior
    {
        private static ConcurrentDictionary<Type, Func<Task, IMethodInvocation, Task>> _wrapperCreators =
                new ConcurrentDictionary<Type, Func<Task, IMethodInvocation, Task>>();

        #region IInterceptionBehavior implementation
        bool IInterceptionBehavior.WillExecute => true;

        IEnumerable<Type> IInterceptionBehavior.GetRequiredInterfaces()
        {
            return Type.EmptyTypes;
        }

        IMethodReturn IInterceptionBehavior.Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            // Before invoking the method on the original target.   
            WriteLog(string.Format("Invoking method {0} at {1}", input.MethodBase, DateTime.Now.ToLongTimeString()));
            // Invoke the next behavior in the chain. 
            IMethodReturn result = getNext()(input, getNext);

            var method = input.MethodBase as MethodInfo;

            if (result.ReturnValue != null
               && method != null
               && typeof(Task).IsAssignableFrom(method.ReturnType))
            {
                // if this method returns a Task, override the original return value
                var task = (Task)result.ReturnValue;
                return input.CreateMethodReturn(this.GetWrapperCreator(method.ReturnType)(task, input), result.Outputs);
            }
            else
            {
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
                WriteLog(string.Format("IInterceptionBehavior: Successfully finished async operation {0}", input.MethodBase.Name));
            }
            catch (Exception e)
            {
                WriteLog($"IInterceptionBehavior: Async operation {input.MethodBase.Name} threw: {e}");
                throw;
            }
            finally
            {
                WriteLog($"IInterceptionBehavior: Async operation {input.MethodBase.Name} completted");

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
                WriteLog(string.Format("IInterceptionBehavior: Successfully finished async operation {0} with value: {1}", input.MethodBase.Name, value));
                return value;
            }
            catch (Exception e)
            {
                WriteLog(string.Format("LogUnityAttributeHandler: Async operation {0} threw: {1}", input.MethodBase.Name, e));
                throw;
            }
            finally
            {
                WriteLog($"IInterceptionBehavior: Async operation {input.MethodBase.Name} completted");

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
