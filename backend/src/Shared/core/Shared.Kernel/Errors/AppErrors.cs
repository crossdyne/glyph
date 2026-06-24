using Crossdyne.Toolkit.Results;

namespace Shared.Kernel.Errors
{
    public static class AppErrors
    {
        public static readonly ErrorCode Api = ErrorCode.Custom(nameof(Api), 10001);
        public static readonly ErrorCode Http = ErrorCode.Custom(nameof(Http), 10002);
        public static readonly ErrorCode GettingUrl = ErrorCode.Custom(nameof(GettingUrl), 10003);
        public static readonly ErrorCode Forbidden = ErrorCode.Custom(nameof(Forbidden), 10004);
    }
}