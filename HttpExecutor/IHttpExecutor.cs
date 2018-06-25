
namespace HttpExecutor
{
    using System;
    using System.Threading.Tasks;

    public interface IHttpRequestExecutor
    {
        Task<IRequestStatus> ExecuteAsync(Uri uri, IHeaderHolder headers);
    }
}
