using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WarehouseApi.DTOs;

namespace WarehouseApi.Filters;

public class ApiResponseFilter : IResultFilter
{
    public void OnResultExecuting(ResultExecutingContext context)
    {
        if (context.Result is ObjectResult objectResult && objectResult.Value is not null)
        {
            var valueType = objectResult.Value.GetType();

            if (!valueType.IsGenericType || valueType.GetGenericTypeDefinition() != typeof(ApiResponse<>))
            {
                var statusCode = objectResult.StatusCode ?? context.HttpContext.Response.StatusCode;

                var apiResponseType = typeof(ApiResponse<>).MakeGenericType(valueType);
                var wrappedResult = Activator.CreateInstance(apiResponseType);

                apiResponseType.GetProperty("StatusCode")?.SetValue(wrappedResult, statusCode);
                apiResponseType.GetProperty("Data")?.SetValue(wrappedResult, objectResult.Value);
                apiResponseType.GetProperty("Version")?.SetValue(wrappedResult, "1.0.0");

                objectResult.Value = wrappedResult;
            }
        }
    }

    public void OnResultExecuted(ResultExecutedContext context)
    {
    }
}
