// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using System.Security.Cryptography;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

public partial class TestServer
{
    Task VerifyUpload(HttpContext context)
    {
        // Report back original request method verb.
        context.Response.Headers.Add("X-HttpRequest-Method", context.Request.Method);

        // Report back original entity-body related request headers.
        string contentLength = context.Request.Headers["Content-Length"];
        if (!string.IsNullOrEmpty(contentLength))
        {
            context.Response.Headers.Add("X-HttpRequest-Headers-ContentLength", contentLength);
        }

        string transferEncoding = context.Request.Headers["Transfer-Encoding"];
        if (!string.IsNullOrEmpty(transferEncoding))
        {
            context.Response.Headers.Add("X-HttpRequest-Headers-TransferEncoding", transferEncoding);
        }

        // Get expected MD5 hash of request body.
        string expectedHash = context.Request.Headers["Content-MD5"];
        if (string.IsNullOrEmpty(expectedHash))
        {
            context.Response.StatusCode = 500;
            context.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Missing 'Content-MD5' request header";
            return Task.CompletedTask;
        }

        // Compute MD5 hash of received request body.
        string actualHash;
        using (MD5 md5 = MD5.Create())
        {
            byte[] hash = md5.ComputeHash(context.Request.Body);
            actualHash = Convert.ToBase64String(hash);
        }

        if (expectedHash == actualHash)
        {
            context.Response.StatusCode = 200;
        }
        else
        {
            context.Response.StatusCode = 500;
            context.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Request body not verfied";           
        }

        return Task.CompletedTask;
    }    
}