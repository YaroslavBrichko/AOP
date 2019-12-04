using IOC.Aspects;
using System.Threading.Tasks;

namespace IOC
{
    public interface IMyInterface
    {
        [LogUnity]
        int Execute();
        Task<int> ExecuteAsync();
    }
}
