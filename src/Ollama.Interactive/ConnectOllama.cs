using Microsoft.DotNet.Interactive.Commands;

namespace Ollama.Interactive;

public class ConnectOllama(string connectedKernelName, string model, string? uri = null) : ConnectKernelCommand(connectedKernelName)
{
    public string Model { get; } = model;

    public string? Uri { get; } = uri;
}