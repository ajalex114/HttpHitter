
using System.Collections.Generic;

namespace HttpExecutor
{
    interface IConfig
    {
        Order Order { get; }

        int MaxRun { get; }

        Dictionary<string, string> DefaultVariables { get; }

        Dictionary<string, string[]> DefaultMultiVariables { get; }
    }
}
