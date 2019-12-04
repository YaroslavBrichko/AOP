using Polly;
using System;


namespace PollyDemo.Policies
{
    public class RetryPolicy : BasePolicy
    {
        private int _maxCount;

        #region .ctor
        public RetryPolicy(int maxCount)
        {
            _maxCount = maxCount;
        }
        #endregion

        public override Policy<int> Build() => Policy.Handle<MyException>()
                         .OrResult<int>(0)
                         .WaitAndRetry(_maxCount, 
                           retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                          (dlgResult, time, attemptCount, context) =>
                          {
                              string reason = dlgResult.Exception == null ? $"result = {dlgResult.Result}" : dlgResult.Exception.Message;
                              Log($"OnRetyry():  attempt - {attemptCount}, Reason - {reason}");
                          });

    }
}
