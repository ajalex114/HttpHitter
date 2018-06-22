
using System.Collections.Generic;

namespace HttpExecutor
{
    class Config : IConfig
    {
        public Order Order { get; set; }

        public int MaxRun { get; set; }

        public Dictionary<string, string> DefaultVariables { get; set; }

        public Dictionary<string, string[]> DefaultMultiVariables { get; set; }
    }
}
