// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Collections.Specialized;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Primitives;

using Newtonsoft.Json;

public class RequestInformation
{
    public string Method { get; private set; }

    public string Url { get; private set; }

    public NameValueCollection Headers { get; private set; }

    public NameValueCollection Cookies { get; private set; }

    public string BodyContent { get; private set; }

    public int BodyLength { get; private set; }

    public bool SecureConnection { get; private set; }

    public static RequestInformation Create(HttpRequest request)
    {
        var info = new RequestInformation();
        info.Method = request.Method;
        info.Url = request.GetEncodedUrl();
        
        var headers = new NameValueCollection();
        foreach (KeyValuePair<string, StringValues> kvp in request.Headers)
        {
            headers.Add(kvp.Key, kvp.Value);
        } 
        info.Headers = headers;

        var cookies = new NameValueCollection();
        CookieCollection cookieCollection = RequestHelper.GetRequestCookies(request);
        foreach (Cookie cookie in cookieCollection)
        {
            cookies.Add(cookie.Name, cookie.Value);
        }
        info.Cookies = cookies;

        Stream stream = request.Body;
        using (var reader = new StreamReader(stream))
        {
            string body = reader.ReadToEnd();
            info.BodyContent = body;
            info.BodyLength = body.Length;
        }

        info.SecureConnection = request.IsHttps;

        return info;
    }

    public static RequestInformation DeSerializeFromJson(string json)
    {
        return (RequestInformation)JsonConvert.DeserializeObject(
            json,
            typeof(RequestInformation),
            new NameValueCollectionConverter());
    }

    public string SerializeToJson()
    {
        return JsonConvert.SerializeObject(this, new NameValueCollectionConverter());
    }

    private RequestInformation()
    {
    }
}