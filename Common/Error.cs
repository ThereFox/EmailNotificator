using CSharpFunctionalExtensions;

namespace Common
{
    public record Error
    (
        string ErrorMessage
    );

    public static class AsResultClass
    {
        public static Result AsResult(this Error error)
        {
            return Result.Failure(error.ErrorMessage);
        }
        public static Result<T> AsResult<T>(this Error error)
        {
            return Result.Failure<T>(error.ErrorMessage);
        }
    }
}
