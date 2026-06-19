using System.Text.Json;
using Crossdyne.Toolkit.Results;

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
            catch (HttpRequestException ex)
            {
                return Result<T>.Failure(new Error(ErrorCode.Server, $"Код: {ex.StatusCode} : {ex.Message}"));
            }
            catch (JsonException ex)
            {
                return Result<T>.Failure(new Error(ErrorCode.Server, $"Ошибка десериализации: {ex.Message}"));
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
            catch (HttpRequestException ex)
            {
                return Result<T>.Failure(new Error(ErrorCode.Server, $"Код: {ex.StatusCode} : {ex.Message}"));
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
            catch (HttpRequestException ex)
            {
                return Result.Failure(new Error(ErrorCode.Server, $"Код: {ex.StatusCode} : {ex.Message}"));
            }
            catch (Exception ex)
            {
                return Result.Failure(new Error(ErrorCode.Server, ex.Message));
            }
        }
    }
}