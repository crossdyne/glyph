using Crossdyne.Toolkit.Results;

namespace Crossdyne.Glyph.Application.Errors
{
    public static class AppErrors
    {
        public static readonly ErrorCode Validation = ErrorCode.Custom(nameof(Validation), 10000);
        public static readonly ErrorCode InsufficientAccess  = ErrorCode.Custom(nameof(InsufficientAccess), 10001);
        public static readonly ErrorCode Http = ErrorCode.Custom(nameof(Http), 10002);
        public static readonly ErrorCode CategoryIsPersonal = ErrorCode.Custom(nameof(CategoryIsPersonal), 10003);
    }
}