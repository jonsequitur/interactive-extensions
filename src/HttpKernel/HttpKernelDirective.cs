// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Net.Http;
using Microsoft.DotNet.Interactive;

namespace HttpKernel;

public class HttpKernelDirective : ChooseKernelDirective
{
    public HttpKernelDirective(HttpRequestKernel kernel) : base(kernel, "Send HTTP requests")
    {
        Kernel = kernel;

        UrlArgument = new("URL");

        Add(UrlArgument);
        SetHandler(this, HttpMethod.Get);

        DeleteCommand = new("DELETE")
        {
            UrlArgument
        };
        AddCommand(DeleteCommand);

        HeadCommand = new("HEAD")
        {
            UrlArgument
        };
        AddCommand(HeadCommand);

        GetCommand = new("GET")
        {
            UrlArgument
        };
        SetHandler(GetCommand, HttpMethod.Get);
        AddCommand(GetCommand);

        PatchCommand = new("PATCH")
        {
            UrlArgument
        };
        AddCommand(PatchCommand);

        OptionsCommand = new("OPTIONS")
        {
            UrlArgument
        };
        AddCommand(OptionsCommand);

        PostCommand = new("POST")
        {
            UrlArgument
        };
        AddCommand(PostCommand);

        PutCommand = new("PUT")
        {
            UrlArgument
        };
        AddCommand(PutCommand);
    }

    public HttpRequestKernel Kernel { get; }

    public Command DeleteCommand { get; set; }

    public Command HeadCommand { get; set; }

    public Command OptionsCommand { get; set; }

    public Command PatchCommand { get; set; }

    public Command PutCommand { get; set; }

    public Command PostCommand { get; set; }

    public Argument<Uri> UrlArgument { get; }

    public Command GetCommand { get; }

    private void SetHandler(Command command, HttpMethod method)
    {
        command.SetHandler<KernelInvocationContext, InvocationContext>((c1, c2) => Kernel.SendRequest(method, c1, c2));
    }
}