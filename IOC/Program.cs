using IOC.Aspects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using Unity.Interception;
using Unity.Interception.ContainerIntegration;
using Unity.Interception.Interceptors.InstanceInterceptors.InterfaceInterception;
using Unity.Interception.Interceptors.InstanceInterceptors.TransparentProxyInterception;

namespace IOC
{
    class Program
    {
        static void Main(string[] args)
        {
            MyServiceLocator.Contaiter.AddNewExtension<Interception>()
                                      .Configure<Interception>()
                                      .SetDefaultInterceptorFor<IMyInterface>(new InterfaceInterceptor());

            MyServiceLocator.Contaiter.RegisterType<IMyInterface, MyClass>(new InterceptionBehavior<LoggingAspect>());


            IMyInterface item = MyServiceLocator.Resolve<IMyInterface>();

            int result =  item.Execute();
            Console.WriteLine($"Result is {result}");

            Console.Read();
        }
    }
}
