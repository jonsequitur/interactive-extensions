// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.CommandLine.Invocation;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.DotNet.Interactive;
using Microsoft.DotNet.Interactive.Commands;

namespace HttpKernel;

public class HttpRequestKernel :
    Kernel,
    IKernelCommandHandler<SubmitCode>
{
    private HttpClient? _httpClient;

    public HttpRequestKernel() : base("http")
    {
        ChooseKernelDirective = new HttpKernelDirective(this);
    }

    public HttpClient HttpClient
    {
        get
        {
            if (_httpClient is null)
            {
                _httpClient = new HttpClient();
            }

            return _httpClient;
        }
        set => _httpClient = value;
    }

    public async Task HandleAsync(SubmitCode command, KernelInvocationContext context)
    {
    }

    public async Task SendRequest(
        HttpMethod method,
        KernelInvocationContext kernelInvocationContext,
        InvocationContext cmdLineInvocationContext)
    {
        var uri = cmdLineInvocationContext.ParseResult.GetValueForArgument(ChooseKernelDirective.UrlArgument);

        var request = new HttpRequestMessage(method, uri);

        var response = await HttpClient.SendAsync(request);

        kernelInvocationContext.Display(response);
    }

    public override HttpKernelDirective ChooseKernelDirective { get; }
}