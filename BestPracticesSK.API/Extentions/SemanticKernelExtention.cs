using BestPracticesSK.API.Interfaces.Services;
using BestPracticesSK.API.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace BestPracticesSK.API.Extentions;

public static class SemanticKernelExtention
{

    public static IServiceCollection AddSemanticKernel(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IChatCompletionService>(sp =>
        {
            return new OpenAIChatCompletionService(
                modelId: configuration["OpenAI:ChatModelId"],
                apiKey: configuration["OpenAI:ApiKey"]
            );
        });

        services.AddSingleton<Kernel>(sp =>
        {
            var kernelBuilder = Kernel.CreateBuilder();
            kernelBuilder.AddOpenAIChatCompletion(
                modelId: configuration["OpenAI:ChatModelId"],
                apiKey: configuration["OpenAI:ApiKey"]
            );
            return kernelBuilder.Build();
        });

        services.AddScoped<IChatService, ChatService>();

        return services;
    }

}
