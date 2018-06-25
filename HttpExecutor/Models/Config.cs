
using System.Collections.Generic;

namespace HttpExecutor
{
    class Config : IConfig
    {
        public Order Order { get; set; }

        public ulong MaxRun { get; set; }

        public Dictionary<string, string> DefaultVariables { get; set; }

        public Dictionary<string, string[]> DefaultMultiVariables { get; set; }
    }
}
