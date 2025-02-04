using Microsoft.DotNet.Interactive;
using Microsoft.DotNet.Interactive.Connection;
using Microsoft.DotNet.Interactive.Directives;
using OllamaSharp;


namespace Ollama.Interactive;

public class ConnectOllamaDirective : ConnectKernelDirective<ConnectOllama>
{
    public ConnectOllamaDirective(string name, string description) : base(name, description)
    {
        var modelParameter = new KernelDirectiveParameter("--model", "The name of the model to use")
        {
            Required = true
        };

        modelParameter.AddCompletions(async context =>
        {
            var uri = new Uri("http://localhost:11434");
            using var ollama = new OllamaApiClient(uri);
            var localModels = await ollama.ListLocalModelsAsync();
            foreach (var model in localModels)
            {
                context.CompletionItems.Add(new(model.Name.Split(":")[0], "Parameter"));
            }
        });

        var uriParameter = new KernelDirectiveParameter("--uri",
            "The URI of the ollama server. (The default is http://127.0.0.1:11434.)");

        Parameters.Add(modelParameter);

        Parameters.Add(uriParameter);
    }

    public override async Task<IEnumerable<Kernel>> ConnectKernelsAsync(
        ConnectOllama connectCommand,
        KernelInvocationContext context)
    {
        var uri = new Uri(connectCommand.Uri ?? "http://localhost:11434");
        var modelName = connectCommand.Model;
        var ollamaClient = new OllamaApiClient(uri, defaultModel: modelName);
        context.HandlingKernel.RootKernel.RegisterForDisposal(ollamaClient);

        var localModels = await ollamaClient.ListLocalModelsAsync();

        var kernel = new OllamaKernel(connectCommand.ConnectedKernelName, ollamaClient);
        var model = localModels.SingleOrDefault(m => m.Name == modelName);
        kernel.KernelInfo.LanguageName = modelName;
        kernel.KernelInfo.LanguageVersion = model?.Digest;
        kernel.KernelInfo.Description = model?.Details.ToString();

        return [kernel];
    }
}