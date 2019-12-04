using PostSharp.Aspects;
using PostSharp.Serialization;
using System;


namespace PostSharpApp.Aspects
{
    [PSerializable]
    public class LoggingAspect : OnMethodBoundaryAspect
    {
        public override void OnEntry(MethodExecutionArgs args)
        {
            Log($"OnEntry: The {args.Method.Name} method has been entered.");
        }

        public override void OnSuccess(MethodExecutionArgs args)
        {
            Log($"OnSuccess: Method {args.Method.Name}({string.Join(", ", args.Arguments)}) returned {args.ReturnValue}.");
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            Log($"OnExit: The {args.Method.Name} method has exited.");
        }
        
        public override void OnException(MethodExecutionArgs args)
        {
            Log($"OnException: The {args.Method.Name} method got exception {args.Exception.Message}.");
        }

        //public override void OnYield(MethodExecutionArgs args)
        //{
        //    Log($"OnYield: The {args.Method.Name} method entered is in await statement.");
        //}

        //public override void OnResume(MethodExecutionArgs args)
        //{
        //    Log($"OnResume: The {args.Method.Name} method resumes execution.");
        //}

        //public override bool CompileTimeValidate(System.Reflection.MethodBase method)
        //{
        //    Type targetType = method.DeclaringType;
        //    return typeof(Declaration.IMyInterface).IsAssignableFrom(targetType);
        //}

        private void Log(string text)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("LOG:" + text);
            Console.ResetColor();
        }
    }
}
