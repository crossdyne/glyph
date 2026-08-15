using System.Net;
using System.Text.Json;
using Crossdyne.Toolkit.Results;
using Shared.Http;

namespace Glyph.Bff.Extensions
{
    public static class HttpExceptionHandler    
    {
        public static async Task<Result<T>> ToResult<T>(this Task<T> task)
        {
            try
            {
                return Result<T>.Success(await task);
            }
            catch (HttpOperationException ex) 
            {
                return Result<T>.Failure(CreateErrorFromStatusCode(ex.StatusCode, ex.ErrorBody));
            }
            catch (HttpRequestException ex)
            {
                return Result<T>.Failure(CreateErrorFromStatusCode(ex.StatusCode, ex.Message));
            }
            catch (JsonException ex)
            {
                return Result<T>.Failure(new Error(ErrorCode.InvalidResponse, $"Ошибка десериализации: {ex.Message}"));
            }
            catch (Exception ex)
            {
                return Result<T>.Failure(new Error(ErrorCode.Server, ex.Message));
            }
        }

        public static async Task<Result<T>> CatchAsync<T>(this Task<Result<T>> task)
        {
            try
            {
                return await task;
            }
            catch (HttpOperationException ex) 
            {
                return Result<T>.Failure(CreateErrorFromStatusCode(ex.StatusCode, ex.ErrorBody));
            }
            catch (HttpRequestException ex)
            {
                return Result<T>.Failure(CreateErrorFromStatusCode(ex.StatusCode, ex.Message));
            }
            catch (Exception ex)
            {
                return Result<T>.Failure(new Error(ErrorCode.Server, ex.Message));
            }
        }

        public static async Task<Result> ToResult(this Task task)
        {
            try
            {
                await task;
                return Result.Success();
            }
            catch (HttpOperationException ex) 
            {
                return Result.Failure(CreateErrorFromStatusCode(ex.StatusCode, ex.ErrorBody));
            }
            catch (HttpRequestException ex)
            {
                return Result.Failure(CreateErrorFromStatusCode(ex.StatusCode, ex.Message));
            }
            catch (Exception ex)
            {
                return Result.Failure(new Error(ErrorCode.Server, ex.Message));
            }
        }

        private static Error CreateErrorFromStatusCode(HttpStatusCode? statusCode, string message)
        {
            var errorCode = statusCode switch
            {
                HttpStatusCode.NotFound => ErrorCode.NotFound,
                HttpStatusCode.Conflict => ErrorCode.Conflict,
                HttpStatusCode.Unauthorized => ErrorCode.Unauthorized,
                HttpStatusCode.Forbidden => ErrorCode.Custom("Forbidden", 403),
                HttpStatusCode.BadRequest => ErrorCode.InvalidRequest,
                HttpStatusCode.InternalServerError => ErrorCode.Server,
                _ => ErrorCode.Server
            };

            return new Error(errorCode, message);
        }
    }
}