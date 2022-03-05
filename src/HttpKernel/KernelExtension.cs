using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.DotNet.Interactive;
using Microsoft.DotNet.Interactive.Commands;
using Microsoft.DotNet.Interactive.Formatting;
using static Microsoft.DotNet.Interactive.Formatting.PocketViewTags;

namespace HttpKernel;

public class KernelExtension : IKernelExtension
{
    /// <inheritdoc/>
    public async Task OnLoadAsync(Kernel kernel)
    {
        if (kernel is CompositeKernel compositeKernel)
        {
            HttpRequestKernel httpKernel = new();

            compositeKernel.Add(httpKernel);

            await compositeKernel.SendAsync(
                new DisplayValue(GetGreeting()));
        }
    }

    private static FormattedValue GetGreeting()
    {
        if (Formatter.DefaultMimeType == HtmlFormatter.MimeType)
        {
            IHtmlContent value = b($"Added {nameof(HttpRequestKernel)}");

            return new FormattedValue(
                HtmlFormatter.MimeType,
                value.ToString());
        }

        return new FormattedValue(
            "text/plain",
            $"Added {nameof(HttpRequestKernel)}");
    }
}