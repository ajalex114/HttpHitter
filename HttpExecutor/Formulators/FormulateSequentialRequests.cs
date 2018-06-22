
namespace HttpExecutor.Formulators
{
    using System.Collections.Generic;

    sealed class FormulateSequentialRequests : AbstractFormulateRequestor
    {
        public FormulateSequentialRequests() { }

        public override IEnumerable<ExecutorRequest> FormulateRequests()
        {
            var executorRequests = new List<ExecutorRequest>();

            int count = 0;
            foreach (var content in _contents)
            {
                // Get the Url
                var urlValue = GetUrl(content);

                urlValue = GetUrlWithQueryParametersAdded(content, urlValue);

                // Apply variables
                urlValue = ApplyVariables(content, urlValue);

                // Apply multiVariables from content file
                var urlValuesWithVariables = ApplyMultiVariables(content, urlValue);

                foreach (var urlVar in urlValuesWithVariables)
                {
                    // Apply default variables
                    var urlOutput = ApplyDefaultVariables(urlVar);

                    // Apply default multi variables
                    var urlsWithDefaultVariables = ApplyDefaultMultiVariables(urlOutput);

                    foreach (var url in urlsWithDefaultVariables)
                    {
                        if (count++ >= _maxRun)
                        {
                            break;
                        }


                        executorRequests.Add(CreateExecutorRequest(url));
                    }
                }
            }

            return executorRequests;
        }
    }
}
