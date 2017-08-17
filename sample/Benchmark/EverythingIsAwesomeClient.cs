namespace Benchmark
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using InfluxDB.LineProtocol.Client;

    public class EverythingIsAwesomeClient : LineProtocolClient
    {
        public EverythingIsAwesomeClient() : base(new EverythingIsAwesomeHandler(), new Uri("http://localhost:8086"), "benchmark")
        {
        }

        private class EverythingIsAwesomeHandler : HttpMessageHandler
        {
            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                var response = new HttpResponseMessage(HttpStatusCode.Continue);
                return Task.FromResult(response);
            }
        }
    }
}