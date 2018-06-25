
using System.Collections.Generic;

namespace HttpExecutor
{
    interface IConfig
    {
        Order Order { get; }

        /// <summary>
        /// Max supported Value : 18,446,744,073,709,551,615
        /// </summary>
        ulong MaxRun { get; }

        Dictionary<string, string> DefaultVariables { get; }

        Dictionary<string, string[]> DefaultMultiVariables { get; }
    }
}
