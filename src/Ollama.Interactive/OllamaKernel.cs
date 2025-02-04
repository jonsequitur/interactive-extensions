using Microsoft.DotNet.Interactive;
using Microsoft.DotNet.Interactive.Commands;
using OllamaSharp;

namespace Ollama.Interactive;

public class OllamaKernel :
    Kernel,
    IKernelCommandHandler<SubmitCode>
{
    private readonly OllamaApiClient _ollamaClient;

    public OllamaKernel(string name, OllamaApiClient ollamaClient) : base(name)
    {
        _ollamaClient = ollamaClient;
    }

    public async Task HandleAsync(SubmitCode command, KernelInvocationContext context)
    {
        var chat = new Chat(_ollamaClient);

        var question = command.Code;

        if (string.IsNullOrWhiteSpace(question))
        {
            return;
        }

        await foreach (var answerToken in chat.SendAsync(question))
        {
            Console.Write(answerToken);
        }
    }
}