using System;
using Microsoft.AspNetCore.Http.Features;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace HenryHiles.Helpers;
public static class HttpRequestExtensions
{
    public static string GetIp(this HttpRequest request)
    {
        string ip = string.Empty;
        if (!string.IsNullOrEmpty(request.Headers["X-Forwarded-For"])) {
            ip = request.Headers["X-Forwarded-For"];
        }
        else {
            ip = request.HttpContext.Features.Get<IHttpConnectionFeature>().RemoteIpAddress.ToString();
        }
        return ip;
    } 
}