// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

public partial class TestServer
{
    Task Redirect(HttpContext context)
    {
        int statusCode = 302;
        string statusCodeString = context.Request.Query["statuscode"];
        if (!string.IsNullOrEmpty(statusCodeString))
        {
            try
            {
                statusCode = int.Parse(statusCodeString);
                if (statusCode < 300 || statusCode > 307)
                {
                    context.Response.StatusCode = 500;
                    context.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Invalid redirect statuscode: " + statusCodeString;
                    return Task.CompletedTask;
                }
            }
            catch (Exception)
            {
                context.Response.StatusCode = 500;
                context.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Error parsing statuscode: " + statusCodeString;
                return Task.CompletedTask;
            }
        }

        string redirectUri = context.Request.Query["uri"];
        if (string.IsNullOrEmpty(redirectUri))
        {
            context.Response.StatusCode = 500;
            context.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Missing redirection uri";
            return Task.CompletedTask;
        }

        string hopsString = context.Request.Query["hops"];
        int hops = 1;
        if (!string.IsNullOrEmpty(hopsString))
        {
            try
            {
                hops = int.Parse(hopsString);
            }
            catch (Exception)
            {
                context.Response.StatusCode = 500;
                context.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Error parsing hops: " + hopsString;
                return Task.CompletedTask;
            }
        }

        RequestHelper.AddResponseCookies(context);

        if (hops <= 1)
        {
            context.Response.Headers.Add("Location", redirectUri);
        }
        else
        {
            context.Response.Headers.Add(
                "Location",
                string.Format("/?uri={0}&hops={1}",
                redirectUri,
                hops - 1));
        }

        context.Response.StatusCode = statusCode;

        return Task.CompletedTask;  
    }    
}