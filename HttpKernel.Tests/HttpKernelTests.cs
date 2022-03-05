using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.DotNet.Interactive;
using Microsoft.DotNet.Interactive.Commands;
using Microsoft.DotNet.Interactive.Events;
using RichardSzalay.MockHttp;
using TestUtilities;
using Xunit;

namespace HttpKernel.Tests;

public class HttpKernelTests
{
    private readonly MockHttpMessageHandler _http = new();

    [Fact]
    public async Task Default_behavior_is_GET()
    {
        using var kernel = await CreateCompositeKernel();

        var url = "https://example.com/";

        var expect = _http.When(HttpMethod.Get, url)
                          .Respond("text/html", "hello!");
        _http.AddRequestExpectation(expect);

        await kernel.SendAsync(new SubmitCode($"#!http {url}"));

        _http.GetMatchCount(expect).Should().Be(1);
    }

    [Fact]
    public async Task GET_sends_GET_request_to_specified_URI()
    {
        using var kernel = await CreateCompositeKernel();

        var url = "https://example.com/";

        var expect = _http.When(HttpMethod.Get, url)
                          .Respond("text/html", "hello!");
        _http.AddRequestExpectation(expect);

        await kernel.SendAsync(new SubmitCode($@"#!http GET {url}"));

        _http.GetMatchCount(expect).Should().Be(1);
    }

    [Fact]
    public async Task GET_displays_response()
    {
        using var kernel = await CreateCompositeKernel();

        var url = "https://example.com/";

        var expect = _http.When(HttpMethod.Get, url)
                          .Respond("text/html", "hello!");
        _http.AddRequestExpectation(expect);

        var result = await kernel.SendAsync(new SubmitCode($"#!http {url}"));

        var events = result.KernelEvents.ToSubscribedList();

        events.Should()
              .ContainSingle<DisplayedValueProduced>()
              .Which
              .Value
              .Should()
              .BeOfType<HttpResponseMessage>();
    }

    private async Task<CompositeKernel> CreateCompositeKernel()
    {
        var kernel = new CompositeKernel();

        await new KernelExtension().OnLoadAsync(kernel);

        if (kernel.FindKernel("http") is HttpRequestKernel httpKernel)
        {
            httpKernel.HttpClient = new HttpClient(_http);
        }

        return kernel;
    }
}