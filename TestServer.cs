// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

public partial class TestServer
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddRouting();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.Map("/Deflate.ashx", a => a.Run(Deflate));
        app.Map("/Echo.ashx", a => a.Run(Echo));
        app.Map("/EmptyContent.ashx", a => a.Run(EmptyContent));
        app.Map("/GZip.ashx", a => a.Run(Gzip));
        app.Map("/Redirect.ashx", a => a.Run(Redirect));
        app.Map("/StatusCode.ashx", a => a.Run(StatusCode));
        app.Map("/Test.ashx", a => a.Run(Test));
        app.Map("/VerifyUpload.ashx", a => a.Run(VerifyUpload));
    }
}