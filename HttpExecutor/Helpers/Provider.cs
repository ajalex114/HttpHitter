
namespace HttpExecutor.Helpers
{
    using HttpExecutor.Formulators;
    using HttpExecutor.Impl;
    using System;

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

                case Order.RandomRepeat:
                    return new FormulateRandomRepeatRequests();

                default:
                    throw new Exception("Invalid Order Type provided");
            }
        }
    }
}
