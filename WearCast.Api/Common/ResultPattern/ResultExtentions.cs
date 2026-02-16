namespace WearCast.Api.Abstractions
{
    public static class ResultExtensions
    {
        // Generic Version (Result<T>)
        public static ObjectResult ToResponse<T>(this Result<T> result)
        {
            if (result.IsSuccess)
            {
                return new OkObjectResult(new
                {
                    isSuccess = true,
                    hasData = true,
                    data = result.Value
                });
            }

            var status = result.Error.StatusCode ?? 400;

            return new ObjectResult(new
            {
                isSuccess = false,
                statusCode = status,
                error = new
                {
                    result.Error.Code,
                    description = result.Error.Description
                }
            })
            {
                StatusCode = status
            };
        }

        // Non-generic version (Result)
        public static ObjectResult ToResponse(this Result result)
        {
            if (result.IsSuccess)
            {
                return new OkObjectResult(new
                {
                    isSuccess = true,
                    hasData = false
                });
            }

            var status = result.Error.StatusCode ?? 400;

            return new ObjectResult(new
            {
                isSuccess = false,
                statusCode = status,
                error = new
                {
                    result.Error.Code,
                    description = result.Error.Description
                }
            })
            {
                StatusCode = status
            };
        }
    }
}
