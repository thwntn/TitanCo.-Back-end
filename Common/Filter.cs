namespace ReferenceFeature;

public class HttpException : Exception
{
    public HttpException(int statusCode, object value) => (StatusCode, Value) = (statusCode, value);

    public int StatusCode { get; }
    public object Value { get; }
}

public class ExceptionFilter : IActionFilter, IOrderedFilter
{
    public int Order => int.MaxValue - 10;

    public void OnActionExecuting(ActionExecutingContext context) { }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Exception is HttpException httpException)
        {
            context.Result = new ObjectResult(httpException.Value) { StatusCode = httpException.StatusCode };
            context.ExceptionHandled = true;
        }
    }
}
