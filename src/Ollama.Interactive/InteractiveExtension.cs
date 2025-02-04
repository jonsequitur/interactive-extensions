using Microsoft.DotNet.Interactive;

namespace Ollama.Interactive;

public static class InteractiveExtension
{
    public static void Configure()
    {
        if (Kernel.Current.RootKernel is CompositeKernel compositeKernel)
        {
            var connectOllamaDirective =
                new ConnectOllamaDirective("ollama", "Connects a kernel for a specified model using Ollama");
            compositeKernel.AddConnectDirective(connectOllamaDirective);
        }
    }
}