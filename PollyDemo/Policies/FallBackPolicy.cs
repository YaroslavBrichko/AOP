using Polly;

namespace PollyDemo.Policies
{
    public class FallBackPolicy : BasePolicy
    {
        private int _default;

        #region .ctor
        public FallBackPolicy(int defValue)
        {
            _default = defValue;
        }
        #endregion

        public override Policy<int> Build() =>  Policy<int>
                                       .Handle<MyOtherException>()
                                       .Fallback<int>((context)=>
                                       {
                                           Log($"inside Fallback - return defalult value - {_default}");
                                           return _default;
                                       }, onFallback: (exception, context) =>
                                       {
                                           Log($"inside onFallback. Reason - {exception.Exception?.Message}");
                                       });
        
    }
}
