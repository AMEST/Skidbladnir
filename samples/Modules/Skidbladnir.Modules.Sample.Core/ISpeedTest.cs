using System.Threading;
using System.Threading.Tasks;

namespace Skidbladnir.Modules.Sample.Core
{
    public interface ISpeedTest
    {
        Task<SpeedTestResult> Check(string url, CancellationToken token = default);
    }
}