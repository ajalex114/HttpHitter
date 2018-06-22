
using System.Collections.Generic;

namespace HttpExecutor.Impl
{
    class Content : IContent
    {
        public IList<ContentModel> ContentModels { get; } = new List<ContentModel>();
    }
}
