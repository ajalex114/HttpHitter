
namespace HttpExecutor
{
    using HttpExecutor.Impl;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using System.Linq;

    static class Processor
    {
        public async static Task<IConfig> ProcessConfigFile(string configFile)
        {
            Config config = null;

            using (var reader = new StreamReader(configFile))
            {
                var configContent = await reader.ReadToEndAsync();
                config = JsonConvert.DeserializeObject<Config>(configContent);
            }

            return config;
        }

        internal async static Task<IHeaderHolder> ProcessHeaderFile(string headerFile)
        {
            var header = new HeaderHolder();

            using (var reader = new StreamReader(headerFile))
            {
                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync();

                    // Commented line
                    if (line.StartsWith("#"))
                    {
                        continue;
                    }

                    if (!string.IsNullOrEmpty(line?.Trim()))
                    {
                        var split = line.Split(new[] { ": " }, StringSplitOptions.None);

                        if (split.Length != 2)
                        {
                            throw new Exception("Invalid Headers");
                        }

                        header.Add(split[0], split[1]);
                    }
                }
            }

            return header;
        }

        public async static Task<IEnumerable<IContent>> ProcessContentFile(string contentFile)
        {
            var contentList = new List<IContent>();

            using (var reader = new StreamReader(contentFile))
            {
                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync();

                    line = line?.Trim();

                    // Commented line or line == null
                    if (line?.StartsWith("#") ?? true)
                    {
                        continue;
                    }

                    if (!string.IsNullOrEmpty(line))
                    {
                        var csvSplit = line.Split(new[] { ',' });

                        if (csvSplit.Length > 0)
                        {
                            var content = new Content();

                            foreach (var csv in csvSplit)
                            {
                                var split = csv.Split(new[] { "::" }, StringSplitOptions.RemoveEmptyEntries);
                                var length = split.Length;

                                if (length < 2)
                                {
                                    throw new Exception("Invalid csv content file");
                                }

                                var contentModel = new ContentModel
                                {
                                    Key = split[0],
                                    ContentType = ParseContentType(split[1]),
                                    Value = length > 2 ? split[2] : null
                                };


                                if(contentModel.ContentType == ContentType.Url && content.ContentModels.Any(x=>x.ContentType == ContentType.Url))
                                {
                                    throw new Exception("Content can have only 1 url");
                                }

                                if (contentModel.ContentType == ContentType.Invalid)
                                {
                                    throw new Exception($"Invalid Content Type : {split[1]}");
                                }

                                content.ContentModels.Add(contentModel);
                            }

                            if(!content.ContentModels.Any(x => x.ContentType == ContentType.Url))
                            {
                                throw new Exception("Content can have exactly 1 url");
                            }

                            if (content.ContentModels.Count > 0)
                            {
                                contentList.Add(content);
                            }
                        }

                    }
                }
            }

            return contentList;
        }

        private static ContentType ParseContentType(string type) =>
            Enum.TryParse<ContentType>(type, true, out var result) ? result : ContentType.Invalid;
    }
}