
namespace HttpExecutor
{
    internal static class CommandLineParser
    {
        public static CommandLineParameterModel Parse(string[] args)
        {
            var parameterType = CommandLineParameterType.None;
            var parameterHolder = new CommandLineParameterModel();

            if (args != null)
            {
                foreach (var arg in args)
                {
                    switch (parameterType)
                    {
                        case CommandLineParameterType.None:

                            switch (arg)
                            {
                                case "/config":
                                    parameterType = CommandLineParameterType.Config;
                                    break;

                                case "/content":
                                    parameterType = CommandLineParameterType.Content;
                                    break;

                                case "/header":
                                    parameterType = CommandLineParameterType.Header;
                                    break;

                                default:
                                    parameterType = CommandLineParameterType.None;
                                    break;
                            }

                            break;

                        case CommandLineParameterType.Config:

                            parameterHolder.ConfigFile = arg;
                            parameterType = CommandLineParameterType.None;
                            break;

                        case CommandLineParameterType.Content:

                            parameterHolder.ContentFile = arg;
                            parameterType = CommandLineParameterType.None;
                            break;

                        case CommandLineParameterType.Header:

                            parameterHolder.HeaderFile = arg;
                            parameterType = CommandLineParameterType.None;
                            break;
                    }
                }
            }

            return parameterHolder;
        }
    }
}
