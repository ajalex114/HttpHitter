using System.Collections.Generic;

namespace HttpExecutor
{
    interface IFormulateRequests
    {
        IEnumerable<ExecutorRequest> FormulateRequests();
    }
}
