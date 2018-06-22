
namespace HttpExecutor
{
    using System.Threading.Tasks;

    public class Settings
    {
        private AppConfiguration _appConfiguration = AppConfiguration.Instance;

        public static Settings Instance { get; } = new Settings();

        private Settings() { }

        public async Task InitializeAsync(string configFile, string headerFile, string contentFile) =>
            await _appConfiguration.InitializeAsync(configFile, headerFile, contentFile);
    }
}
