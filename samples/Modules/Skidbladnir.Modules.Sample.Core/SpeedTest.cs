using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Skidbladnir.Modules.Sample.Core
{
    internal class SpeedTest : ISpeedTest
    {
        private readonly ILogger<SpeedTest> _logger;
        private readonly HttpClient _client;

        public SpeedTest(ILogger<SpeedTest> logger)
        {
            _logger = logger;
            _client = new HttpClient();
        }

        public async Task<SpeedTestResult> Check(string url, CancellationToken token = default)
        {
            var sw = Stopwatch.StartNew();
            using var response = await _client.GetAsync(url, token);
            sw.Stop();
            var elapsedSeconds = sw.ElapsedMilliseconds / 1000.0;
            var content = await response.Content.ReadAsStringAsync();
            var contentLenghtKb = content.Length / 1024;
            return new SpeedTestResult()
            {
                ElapsedSeconds = elapsedSeconds,
                Size = contentLenghtKb,
                Speed = contentLenghtKb / elapsedSeconds
            };
        }
    }
}