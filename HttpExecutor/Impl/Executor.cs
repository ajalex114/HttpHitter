
namespace HttpExecutor
{
    using HttpExecutor.Helpers;
    using HttpExecutor.Impl;
    using System;
    using System.Linq;

    class Executor : IExecutor
    {
        private readonly IConfig _config;
        private readonly IHttpRequestExecutor _httpRequestExecutor;
        private readonly IHeaderHolder _headers;

        private AppConfiguration _appConfiguration = AppConfiguration.Instance;

        public Executor(IHttpRequestExecutor httpRequestExecutor)
        {
            _config = _appConfiguration.Config;
            _headers = _appConfiguration.Headers;
            _httpRequestExecutor = httpRequestExecutor;
        }

        public IExecutionStatus Execute()
        {
            
            var total = 0;
            var success = 0;
            var failure = 0;

            var requestAlgorithm = Provider.GetRequestAlgorithm(_config.Order);
            var requests = requestAlgorithm.FormulateRequests();

            var requestUris = requests.Select(x => x.Request);

            foreach (var uri in requestUris)
            {
                try
                {
                    Console.WriteLine(DateTime.Now);
                    Console.WriteLine(uri);

                    _httpRequestExecutor.ExecuteAsync(uri, _headers).Wait();

                    Console.WriteLine(DateTime.Now);

                    success++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    failure++;
                }
                finally
                {
                    total++;
                }
            }

            Console.WriteLine();

            return new ExecutionStatus
            {
                TotalExecutions = total,
                SuccessCount = success,
                FailureCount = failure
            };
        }
    }
}
