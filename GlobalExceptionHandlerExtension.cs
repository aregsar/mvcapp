using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Net;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Diagnostics;
using System.Text;

namespace mvcapp
{
    public static class GlobalExceptionHandlerExtension
    {
        //This method will globally handle logging unhandled execptions.
        //It will respond json response for ajax calls that send the json accept header
        //otherwise it will redirect to an error page
        public static void UseGlobalExceptionHandler(this IApplicationBuilder app
                                                    , ILogger logger
                                                    , string errorPagePath
                                                    , bool respondWithJsonErrorDetails=false)
        {
            app.UseExceptionHandler(appBuilder =>
            {
                appBuilder.Run(async context =>
                {
                    //============================================================
                    //Log Exception
                    //============================================================
                    var exception = context.Features.Get<IExceptionHandlerFeature>().Error;

                    string errorDetails = $@"{exception.Message}
                                             {Environment.NewLine}
                                             {exception.StackTrace}";

                    int statusCode = (int)HttpStatusCode.InternalServerError;

                    context.Response.StatusCode = statusCode;

                    var problemDetails = new ProblemDetails
                    {
                        Title = "Unexpected Error",
                        Status = statusCode,
                        Detail = errorDetails,
                        Instance = Guid.NewGuid().ToString()
                    };

                    var json = JsonConvert.SerializeObject(problemDetails);

                    logger.LogError(json);

                    //============================================================
                    //Return response
                    //============================================================
                    var matchText="JSON";

                    bool requiresJsonResponse = context.Request
                                                       .GetTypedHeaders()
                                                       .Accept
                                                       .Any(t => t.Suffix.Value?.ToUpper() == matchText
                                                              || t.SubTypeWithoutSuffix.Value?.ToUpper() == matchText);

                    if (requiresJsonResponse)
                    {
                        context.Response.ContentType = "application/json; charset=utf-8";

                        if(!respondWithJsonErrorDetails)
                            json = JsonConvert.SerializeObject(new {Title = "Unexpected Error", Status = statusCode});

                        await context.Response
                                     .WriteAsync(json, Encoding.UTF8);
                    }
                    else
                    {
                        context.Response.Redirect(errorPagePath);

                        await Task.CompletedTask;
                    }
                });
            });
        }
    }
}