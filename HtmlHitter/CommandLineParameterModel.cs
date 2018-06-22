namespace HttpExecutor
{
    class CommandLineParameterModel
    {
        public string ConfigFile { get; set; } = @"Assets\config.json";

        public string HeaderFile { get; set; } = @"Assets\headers.prop";

        public string ContentFile { get; set; } = @"Assets\content.csv";
    }
}
