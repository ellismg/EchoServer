// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

public partial class TestServer
{
    async Task Gzip(HttpContext context)
    {
        string responseBody = "Sending GZIP compressed";

        context.Response.Headers.Add("Content-MD5", Convert.ToBase64String(ContentHelper.ComputeMD5Hash(responseBody)));
        context.Response.Headers.Add("Content-Encoding", "gzip");

        context.Response.ContentType = "text/plain";

        byte[] bytes = ContentHelper.GetGZipBytes(responseBody);
        await context.Response.Body.WriteAsync(bytes, 0, bytes.Length);        
    }    
}