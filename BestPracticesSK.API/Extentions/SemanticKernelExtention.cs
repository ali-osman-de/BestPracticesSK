using BestPracticesSK.API.Interfaces.Services;
using BestPracticesSK.API.Services;
using Microsoft.SemanticKernel;

namespace BestPracticesSK.API.Extentions;

public static class SemanticKernelExtention
{

    public static IServiceCollection AddSemanticKernel(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<Kernel>(sp =>
        {
            return Kernel.CreateBuilder()
                .AddOpenAIChatCompletion(
                    modelId: configuration["OpenAI:ChatModelId"],
                    apiKey: configuration["OpenAI:ApiKey"]
                )
                .Build();
        });

        services.AddScoped<IChatService, ChatService>();

        return services;
    }

}
