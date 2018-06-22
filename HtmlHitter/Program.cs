
namespace HttpExecutor
{
    using HttpExecutor.Helpers;
    using System;

    class Program
    {
        static void Main(string[] args)
        {
            var parameters = CommandLineParser.Parse(args);

            var settings = Settings.Instance;
            settings.InitializeAsync(parameters.ConfigFile, parameters.HeaderFile, parameters.ContentFile).Wait();

            var httpExecutor = Provider.GetHttpRequestExecutor();
            var executor = Provider.GetExecutor(httpExecutor);

            var status = executor.Execute();

            Console.WriteLine($"Total : {status.TotalExecutions}");
            Console.WriteLine($"Success : {status.SuccessCount}");
            Console.WriteLine($"Failure : {status.FailureCount}");

            Console.WriteLine("Press any key to exit");
            Console.ReadLine();
        }
    }
}
