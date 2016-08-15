// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Primitives;

public partial class TestServer
{
    Task StatusCode(HttpContext context)
    {
        string statusCodeString = context.Request.Query["statuscode"];
        StringValues statusDescription = context.Request.Query["statusdescription"];
        try
        {
            int statusCode = int.Parse(statusCodeString);
            context.Response.StatusCode = statusCode;
            if (statusDescription != StringValues.Empty)
            {
                context.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = statusDescription;
            }
        }
        catch (Exception)
        {
            context.Response.StatusCode = 500;
            context.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Error parsing statuscode: " + statusCodeString;
        }

        return Task.CompletedTask;        
    }    
}