using Microsoft.DotNet.Interactive;
using Microsoft.DotNet.Interactive.Commands;
using OllamaSharp;

namespace Ollama.Interactive;

public class OllamaKernel :
    Kernel,
    IKernelCommandHandler<SubmitCode>
{
    private readonly OllamaApiClient _ollamaClient;
    private Chat? _chat;

    public OllamaKernel(string name, OllamaApiClient ollamaClient) : base(name)
    {
        _ollamaClient = ollamaClient;
    }

    public async Task HandleAsync(SubmitCode command, KernelInvocationContext context)
    {
        var question = command.Code;

        if (string.IsNullOrWhiteSpace(question))
        {
            return;
        }

        _chat ??= new Chat(_ollamaClient);

        await foreach (var answerToken in _chat.SendAsync(question))
        {
            Console.Write(answerToken);
        }
    }
}