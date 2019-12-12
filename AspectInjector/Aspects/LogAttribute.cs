using AspectInjector.Broker;
using System;


namespace AspectInjector.Aspects
{
    [Injection(typeof(LogAspect))]
    public class LogAttribute : Attribute
    {
    }

    [Injection(typeof(LogAspectAsync))]
    public class LogAsyncAttribute : Attribute
    {
    }

}
