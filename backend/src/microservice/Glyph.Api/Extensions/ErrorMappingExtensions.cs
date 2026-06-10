using Crossdyne.Toolkit.Results;
using Glyph.Application.Errors;
using Microsoft.AspNetCore.Mvc;

namespace Glyph.Api.Extensions
{
    public static class ErrorMappingExtensions
    {
        public static IActionResult MapActionResult(this Controller controller, IReadOnlyList<Error> errors)
        {
            if (errors == null || errors.Count == 0)
                return controller.StatusCode(StatusCodes.Status500InternalServerError);

            var statusCode = errors[0].Code.Name switch
            {
                nameof(ErrorCode.NotFound) => StatusCodes.Status404NotFound,
                
                nameof(ErrorCode.Save) or
                nameof(ErrorCode.Empty) or 
                nameof(ErrorCode.Create) => StatusCodes.Status500InternalServerError,

                _ or 
                nameof(AppErrors.Validation) => StatusCodes.Status400BadRequest
            };

            return controller.StatusCode(statusCode, errors);
        }
    }
}