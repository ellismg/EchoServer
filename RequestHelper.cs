// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

public static class RequestHelper
{
    public static void AddResponseCookies(HttpContext context)
    {
        // Turn all 'X-SetCookie' request headers into 'Set-Cookie' response headers.

        foreach (KeyValuePair<string, StringValues> kvp in context.Request.Headers)
        {
            string headerName = kvp.Key;
            string headerValue = kvp.Value;
            
            if (string.Equals(headerName, "X-SetCookie", StringComparison.OrdinalIgnoreCase))
            {
                context.Response.Headers.Add("Set-Cookie", headerValue);
            }                
        }
    }

    public static CookieCollection GetRequestCookies(HttpRequest request)
    {
        var cookieCollection = new CookieCollection();
        IRequestCookieCollection cookies = request.Cookies;

        foreach (KeyValuePair<string, string> cookie in request.Cookies)
        {
            cookieCollection.Add(new Cookie(cookie.Key, cookie.Value));
        }

        return cookieCollection;
    }
}
