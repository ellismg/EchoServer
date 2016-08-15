// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;

using Microsoft.AspNetCore.Http;

public partial class TestServer
{
    async Task Echo(HttpContext context)
    {
        if (!await AuthenticationHelper.HandleAuthentication(context))
        {
            return;
        }

        RequestHelper.AddResponseCookies(context);

        // Add original request method verb as a custom response header.
        context.Response.Headers.Add("X-HttpRequest-Method", context.Request.Method);

        // Echo back JSON encoded payload.
        RequestInformation info = RequestInformation.Create(context.Request);
        string echoJson = info.SerializeToJson();

        // Compute MD5 hash to clients can verify the received data.
        using (MD5 md5 = MD5.Create())
        {
            byte[] bytes = Encoding.UTF8.GetBytes(echoJson);
            byte[] hash = md5.ComputeHash(bytes);
            string encodedHash = Convert.ToBase64String(hash);

            context.Response.Headers.Add("Content-MD5", encodedHash);
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(echoJson);
        }       
    }
}