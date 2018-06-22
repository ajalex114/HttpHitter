
namespace HttpExecutor.Helpers
{
    using HttpExecutor.Formulators;
    using HttpExecutor.Impl;

    public static class Provider
    {
        public static IHttpRequestExecutor GetHttpRequestExecutor() => new HttpRequestExecutor();

        public static IExecutor GetExecutor(IHttpRequestExecutor httpRequestExecutor) => new Executor(httpRequestExecutor);

        internal static IFormulateRequests GetRequestAlgorithm(Order order)
        {
            switch (order)
            {
                case Order.Sequential:
                    return new FormulateSequentialRequests();

                case Order.SequentialRepeat:
                    return new FormulateSequentialRepeatRequests();

                case Order.Random:
                    return new FormulateRandomRequests();

                default:
                    return new FormulateSequentialRequests();
            }
        }
    }
}
