using PostSharp.Aspects;
using PostSharp.Serialization;
using System;
using System.Threading.Tasks;

namespace PostSharpApp.Aspects
{
    [PSerializable]
    public class RetryAspect : MethodInterceptionAspect
    {

        private int _maxRetries;
        public RetryAspect(int maxRetries)
        {
            _maxRetries = 3;
        }

        public override void OnInvoke(MethodInterceptionArgs args)
        {
            int retriesCounter = 0;
            bool isContinue = true;

            while (isContinue)
            {
                try
                {
                    args.Proceed();
                    isContinue = false;
                }
                catch (Exception e)
                {
                    retriesCounter++;
                    if (retriesCounter > _maxRetries) throw;

                    Log($"Retry aspect: Exception during attempt {retriesCounter} of calling method {args.Method.DeclaringType}.{args.Method.Name}: {e.Message}");
                }
            }
        }

        public async override Task OnInvokeAsync(MethodInterceptionArgs args)
        {
            int retriesCounter = 0;
            bool isContinue = true;

            while (isContinue)
            {
                try
                {
                    await args.ProceedAsync();
                    isContinue = false;
                }
                catch (Exception e)
                {
                    retriesCounter++;
                    if (retriesCounter > _maxRetries) throw;

                    Log( $"Retry async aspect: Exception during attempt {retriesCounter} of calling method {args.Method.DeclaringType}.{args.Method.Name}: {e.Message}");
                }
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
