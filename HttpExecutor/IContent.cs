
using System.Collections.Generic;

namespace HttpExecutor
{
    interface IContent
    {
        IList<ContentModel> ContentModels { get; }
    }
}
