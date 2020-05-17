using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;

namespace cw3.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            var filePath = @"./requestsLog.txt";

            httpContext.Request.EnableBuffering();

            if (httpContext.Request != null)
            {
                string path = httpContext.Request.Path;
                string queryString = httpContext.Request.QueryString.ToString();
                string method = httpContext.Request.Method.ToString();
                string bodyStr = "";

                using (StreamReader reader
                 = new StreamReader(httpContext.Request.Body, Encoding.UTF8, true, 1024, true))
                {
                    bodyStr = await reader.ReadToEndAsync();
                    reader.Close();
                    httpContext.Request.Body.Position = 0;
                }

                using (StreamWriter writer = File.AppendText(filePath))
                {
                    writer.WriteLine("LOG[METHOD]: " + method);
                    writer.WriteLine("LOG[PATH]: " + path);
                    writer.WriteLine("LOG[QUERY]: " + queryString);
                    writer.WriteLine("LOG[BODY]: " + bodyStr);
                    writer.WriteLine();
                }
            }
            if (_next != null)
                await _next(httpContext);
        }


    }
}
