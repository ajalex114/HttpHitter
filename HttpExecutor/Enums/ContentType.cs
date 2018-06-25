
namespace HttpExecutor
{

    // Priority Order : Url, Query, Var, MultiVar

    enum ContentType
    {
        Invalid,
        Url,
        Var,
        MultiVar,
        Query
    }
}