using Crossdyne.Toolkit.Results;

namespace Shared.Kernel.Errors
{
    public static class AppErrors
    {
        public static readonly ErrorCode Api = ErrorCode.Custom(nameof(Api), 10001);
    }
}