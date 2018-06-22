
namespace HttpExecutor.Formulators
{
    using HttpExecutor.Helpers;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    abstract class AbstractFormulateRequestor : IFormulateRequests
    {
        protected readonly int _maxRun;
        protected readonly IHeaderHolder _headers;
        protected readonly IEnumerable<IContent> _contents;
        protected readonly IConfig _config;

        private AppConfiguration _appConfiguration = AppConfiguration.Instance;

        protected AbstractFormulateRequestor()
        {
            _contents = _appConfiguration.Contents;
            _headers = _appConfiguration.Headers;
            _config = _appConfiguration.Config;
            _maxRun = _config.MaxRun;
        }

        public abstract IEnumerable<ExecutorRequest> FormulateRequests();

        protected string GetUrlWithQueryParametersAdded(IContent content, string urlValue)
        {
            if (content.HasQueryParameters())
            {
                var query = new StringBuilder();
                query.Append("?");
                content.GetQueryParameters().ToList().ForEach(x => { query.Append(x.Key); query.Append("="); query.Append(x.Value); query.Append("&"); });
                query.Remove(query.Length - 1, 1);
                urlValue = urlValue + query.ToString();
            }

            return urlValue;
        }

        protected string ApplyDefaultVariables(string urlValue)
        {
            _config.DefaultVariables.ToList().ForEach(x => urlValue = ApplyVariable(urlValue, x));
            return urlValue;
        }

        protected IEnumerable<string> ApplyDefaultMultiVariables(string url)
        {
            var result = new List<string>();

            var multiVariables = _config.DefaultMultiVariables;
            var size = multiVariables?.Count ?? 0;
            var keys = multiVariables?.Keys?.ToList();

            ApplyMultiVariablesToResult(url, multiVariables, keys, 0, size, result);

            if (result.Count == 0)
            {
                result.Add(url);
            }

            return result.Distinct();
        }

        private void ApplyMultiVariablesToResult(string url, Dictionary<string, string[]> multiVariables, List<string> keys, int pos, int size, List<string> result)
        {
            if (pos < size)
            {
                var key = keys[pos];

                foreach (var var in multiVariables[key])
                {
                    var temp = url;
                    url = ApplyVariable(url, new KeyValuePair<string, string>(key, var));
                    ApplyMultiVariablesToResult(url, multiVariables, keys, pos + 1, size, result);

                    // Add to list only once all the variables are applied
                    if (size == pos + 1)
                    {
                        result.Add(url);
                    }

                    // Back track
                    url = temp;
                }
            }
        }

        protected static string ApplyVariables(IContent content, string urlValue)
        {
            content.GetVariables().ToList().ForEach(x => urlValue = ApplyVariable(urlValue, new KeyValuePair<string, string>(x.Key, x.Value)));
            return urlValue;
        }

        protected IEnumerable<string> ApplyMultiVariables(IContent content, string url)
        {
            var result = new List<string>();

            var multiVariables = content.GetMultiVariables()?.ToDictionary(
                keySelector: x => x.Key,
                elementSelector: x => x?.Value?.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries));

            var size = multiVariables?.Count ?? 0;
            var keys = multiVariables?.Keys?.ToList();

            ApplyMultiVariablesToResult(url, multiVariables, keys, 0, size, result);

            if (result.Count == 0)
            {
                result.Add(url);
            }

            return result.Distinct();
        }

        protected static string ApplyVariable(string urlValue, KeyValuePair<string, string> var) =>
            urlValue.Replace("{" + var.Key + "}", var.Value);

        protected static string GetUrl(IContent content) =>
            content.GetUrls().FirstOrDefault()?.Key;

        protected ExecutorRequest CreateExecutorRequest(string urlValue)
        {
            var url = new Uri(urlValue);

            return new ExecutorRequest
            {
                Request = url
            };
        }
    }
}
