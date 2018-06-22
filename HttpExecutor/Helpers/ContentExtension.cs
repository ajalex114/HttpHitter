
namespace HttpExecutor.Helpers
{
    using System.Collections.Generic;
    using System.Linq;

    static class ContentExtension
    {
        public static IEnumerable<ContentModel> GetUrls(this IContent content) =>
            content.ContentModels.Where(x => x.ContentType == ContentType.Url);

        public static IEnumerable<ContentModel> GetVariables(this IContent content) =>
            content.ContentModels.Where(x => x.ContentType == ContentType.Var);

        public static IEnumerable<ContentModel> GetQueryParameters(this IContent content) =>
            content.ContentModels.Where(x => x.ContentType == ContentType.Query);

        public static IEnumerable<ContentModel> GetMultiVariables(this IContent content) =>
            content.ContentModels.Where(x => x.ContentType == ContentType.MultiVar);

        public static bool HasQueryParameters(this IContent content) =>
            content.ContentModels.Any(x => x.ContentType == ContentType.Query);
    }
}
