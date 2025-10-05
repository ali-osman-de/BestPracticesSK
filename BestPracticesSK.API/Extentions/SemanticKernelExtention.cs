using BestPracticesSK.API.Interfaces.Services;
using BestPracticesSK.API.Services;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace BestPracticesSK.API.Extentions;

public static class SemanticKernelExtention
{

    public static IServiceCollection AddSemanticKernel(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<Kernel>(sp =>
        {
            var kernelBuilder = Kernel.CreateBuilder();

            kernelBuilder.AddOpenAIChatCompletion(
                modelId: configuration["OpenAI:ChatModelId"],
                apiKey: configuration["OpenAI:ApiKey"]
            );

            kernelBuilder.Plugins.AddFromType<WeatherPlugin>("Weather");

            return kernelBuilder.Build();
        });

        services.AddScoped<IChatService, ChatService>();

        return services;
    }

}
