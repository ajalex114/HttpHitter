
namespace HttpExecutor
{
    using System;
    using System.Threading.Tasks;

    public interface IHttpRequestExecutor
    {
        Task ExecuteAsync(Uri uri, IHeaderHolder headers);
    }
}
