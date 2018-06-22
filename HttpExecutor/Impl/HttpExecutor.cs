
namespace HttpExecutor.Impl
{
    using HttpExecutor.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    class HttpRequestExecutor : IHttpRequestExecutor
    {
        private readonly IList<string> _skipHeaders;

        public HttpRequestExecutor()
        {
            _skipHeaders = Enum.GetNames(typeof(PredefinedHeaders));
        }

        public async Task ExecuteAsync(Uri uri, IHeaderHolder headers)
        {
            var requestHandler = new WebRequestHandler
            {
                CookieContainer = GetCookies(uri, headers)
            };

            var authorizationHeader = GetAuthorizationToken(headers);

            using (var httpClient = new HttpClient(requestHandler))
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, uri))
                {
                    request.Headers.Clear();
                    request.Headers.Authorization = authorizationHeader;
                    headers.Where(x => !_skipHeaders.Contains(x.Key)).ToList().ForEach(x => request.Headers.Add(x.Key, x.Value));

                    var response = await httpClient.SendAsync(request);
                    response.EnsureSuccessStatusCode();
                }
            }
        }

        private static CookieContainer GetCookies(Uri uri, IHeaderHolder headers)
        {
            var cookieContainer = new CookieContainer();
            var cookieValue = headers.Where(x => x.Key == PredefinedHeaders.Cookie.ToString()).FirstOrDefault().Value;

            if (cookieValue != null)
            {
                var cookies = cookieValue.Split(new[] { ';' });
                foreach (var cookie in cookies)
                {
                    var c = cookie.Split(new[] { '=' });
                    cookieContainer.Add(uri, new Cookie(c[0]?.Trim(), c[1]?.Trim()));
                }
            }

            return cookieContainer;
        }

        private static AuthenticationHeaderValue GetAuthorizationToken(IHeaderHolder headers)
        {
            AuthenticationHeaderValue authenticationHeader = null;
            var auth = headers.Where(x => x.Key == PredefinedHeaders.Authorization.ToString()).FirstOrDefault().Value;

            if (auth != null)
            {
                var index = auth.IndexOf(' ');
                var scheme = auth.Substring(0, index);
                var value = auth.Substring(index + 1, auth.Length - index - 1);

                authenticationHeader = new AuthenticationHeaderValue(scheme, value);
            }

            return authenticationHeader;
        }
    }
}
