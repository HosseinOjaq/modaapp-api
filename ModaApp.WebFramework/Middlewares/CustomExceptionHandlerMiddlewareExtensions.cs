using System.Net;
using Newtonsoft.Json;
using ModaApp.Common.Enums;
using ModaApp.Common.Models;
using ModaApp.WebFramework.Api;
using Microsoft.AspNetCore.Http;
using ModaApp.Common.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace ModaApp.WebFramework.Middlewares;

public class CustomExceptionHandlerMiddleware(RequestDelegate next, ILogger<CustomExceptionHandlerMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        string? message = null;
        HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError;
        OperationStatusCode apiStatusCode = OperationStatusCode.ServerError;

        try
        {
            await next(context);
        }
        catch (AppException exception)
        {
            logger.LogError(exception, exception.Message);
            httpStatusCode = exception.HttpStatusCode;
            apiStatusCode = exception.ApiStatusCode;

#if DEBUG
            var dic = new Dictionary<string, string>
            {
                ["Exception"] = exception.Message,
                ["StackTrace"] = exception.StackTrace,
            };
            if (exception.InnerException != null)
            {
                dic.Add("InnerException.Exception", exception.InnerException.Message);
                dic.Add("InnerException.StackTrace", exception.InnerException.StackTrace);
            }
            if (exception.AdditionalData != null)
                dic.Add("AdditionalData", JsonConvert.SerializeObject(exception.AdditionalData));

            message = JsonConvert.SerializeObject(dic);
#endif

#if RELEASE

            message = exception.Message;
#endif
            await WriteToResponseAsync();
        }
        catch (SecurityTokenExpiredException exception)
        {
            logger.LogError(exception, exception.Message);
            SetUnAuthorizeResponse(exception);
            await WriteToResponseAsync();
        }
        catch (UnauthorizedAccessException exception)
        {
            logger.LogError(exception, exception.Message);
            SetUnAuthorizeResponse(exception);
            await WriteToResponseAsync();
        }
        catch (AppValidationException exception)
        {
            var result = new ApiResult(false, OperationStatusCode.OK, exception.Errors.Select(x => ErrorModel.Create("GeneralException", x)).ToArray());
            var json = JsonConvert.SerializeObject(result);

            context.Response.StatusCode = (int)OperationStatusCode.OK;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(json);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, exception.Message);
#if DEBUG
            var dic = new Dictionary<string, string>
            {
                ["Exception"] = exception.Message,
                ["StackTrace"] = exception.StackTrace ?? "No Stack Trace.",
            };
            message = JsonConvert.SerializeObject(dic);
#endif
            await WriteToResponseAsync();
        }

        async Task WriteToResponseAsync()
        {
            if (context.Response.HasStarted)
                throw new InvalidOperationException("The response has already started, the http status code middleware will not be executed.");

            var result = new ApiResult(false, apiStatusCode, ErrorModel.Create("GeneralException", message));
            var json = JsonConvert.SerializeObject(result);

            context.Response.StatusCode = (int)httpStatusCode;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(json);
        }

        void SetUnAuthorizeResponse(Exception exception)
        {
            httpStatusCode = HttpStatusCode.Unauthorized;
            apiStatusCode = OperationStatusCode.UnAuthorized;

#if DEBUG
            var dic = new Dictionary<string, string>
            {
                ["Exception"] = exception.Message,
                ["StackTrace"] = exception.StackTrace ?? "No Stack Trace."
            };
            if (exception is SecurityTokenExpiredException tokenException)
                dic.Add("Expires", tokenException.Expires.ToString());

            message = JsonConvert.SerializeObject(dic);
#endif            
        }
    }
}