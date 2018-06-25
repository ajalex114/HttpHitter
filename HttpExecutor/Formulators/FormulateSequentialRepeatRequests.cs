
namespace HttpExecutor.Formulators
{
    using System.Collections.Generic;

    sealed class FormulateSequentialRepeatRequests : AbstractFormulateRequestor
    {
        public FormulateSequentialRepeatRequests() { }

        public override IEnumerable<ExecutorRequest> FormulateRequests()
        {
            var executorRequests = new List<ExecutorRequest>();

            ulong count = 0;
            while (count < _maxRun)
            {
                foreach (var content in _contents)
                {
                    // Get the Url
                    var urlValue = GetUrl(content);

                    // Apply query parameters
                    urlValue = GetUrlWithQueryParametersAdded(content, urlValue);

                    // Apply variables
                    urlValue = ApplyVariables(content, urlValue);

                    // Apply multiVariables from content file
                    var urlValuesWithVariables = ApplyMultiVariables(content, urlValue);

                    foreach (var urlVar in urlValuesWithVariables)
                    {
                        // Apply default variables
                        var urlOutput = ApplyDefaultVariables(urlValue);

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

                if (count >= _maxRun)
                {
                    break;
                }
            }

            return executorRequests;
        }
    }
}
