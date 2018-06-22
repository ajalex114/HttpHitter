
namespace HttpExecutor
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    class AppConfiguration
    {

        private IConfig _config = null;
        private IHeaderHolder _header = null;
        private IEnumerable<IContent> _contents = null;
        private bool _isInitialized = false;

        public static AppConfiguration Instance { get; } = new AppConfiguration();

        private AppConfiguration() { }

        public IConfig Config
        {
            get
            {
                CheckIfInitialized();
                return _config;
            }
        }

        public IHeaderHolder Headers
        {
            get
            {
                CheckIfInitialized();
                return _header;
            }
        }

        public IEnumerable<IContent> Contents
        {
            get
            {
                CheckIfInitialized();
                return _contents;
            }
        }

        private void CheckIfInitialized()
        {
            if (_isInitialized)
            {
                return;
            }

            throw new Exception("Settings not Initialized");
        }

        public async Task InitializeAsync(string configFile, string headerFile, string contentFile)
        {
            _config = await Processor.ProcessConfigFile(configFile);
            _header = await Processor.ProcessHeaderFile(headerFile);
            _contents = await Processor.ProcessContentFile(contentFile);
            _isInitialized = true;
        }
    }
}
