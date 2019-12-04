using System;
using Unity;


namespace IOC
{
    public class MyServiceLocator
    {
        static MyServiceLocator()
        {
            Contaiter = new UnityContainer();
        }

        public static IUnityContainer Contaiter { get; }

        public static T Resolve<T>(string name)
        {
            return Contaiter.Resolve<T>(name);
        }

        public static T Resolve<T>()
        {
            return Contaiter.Resolve<T>();
        }

        public static object Resolve(Type type)
        {
            return Contaiter.Resolve(type);
        }
    }
}
