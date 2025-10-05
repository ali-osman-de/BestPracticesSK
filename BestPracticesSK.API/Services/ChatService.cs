using Azure;
using BestPracticesSK.API.Interfaces.Services;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using System.Text;

namespace BestPracticesSK.API.Services;
public class ChatService : IChatService
{
    private readonly Kernel _kernel;
    private readonly IChatCompletionService _chatCompletionService;

    public ChatService(Kernel kernel)
    {
        _kernel = kernel;
        _chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
    }

    public async Task<string> GetChatResponseAsync(string userMessage)
    {
        var response = await _chatCompletionService.GetChatMessageContentAsync(userMessage);
        return response.ToString();
    }

    public async Task<string> GetChatResponseWithHistoryAsync(string userMessage)
    {
        var history = new ChatHistory();
        history.AddSystemMessage(@"
        Sen bir akıllı hava durumu asistanısın.
        Kullanıcının belirttiği şehir için 'get_weather' fonksiyonunu KESİNLİKLE çağır.
        Kendi tahminini yapma, sadece fonksiyon sonucunu kullan.
        ");
        history.AddUserMessage(userMessage);

        var settings = new PromptExecutionSettings
        {
            ExtensionData = new Dictionary<string, object>
            {
                { "temperature", 0.7 },
                { "max_tokens", 150 }
            },
            FunctionChoiceBehavior = FunctionChoiceBehavior.Auto() // plugin için zorunlu!
        };

        var response = await _chatCompletionService.GetChatMessageContentAsync(history, settings, _kernel);

        return response.Content;

    }

    public async Task<string> GetChatStreamingResponseAsync(string userMessage)
    {
        var history = new ChatHistory();
        history.AddSystemMessage(@"
        Sen bir akıllı hava durumu asistanısın.
        Kullanıcının belirttiği şehir için 'get_weather' fonksiyonunu KESİNLİKLE çağır.
        Kendi tahminini yapma, sadece fonksiyon sonucunu kullan.
        ");
        history.AddUserMessage(userMessage);

        var settings = new PromptExecutionSettings
        {
            ExtensionData = new Dictionary<string, object>
            {
                { "temperature", 0.7 },
                { "max_tokens", 150 }
            },
            FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
        };

        var responseStream = _chatCompletionService.GetStreamingChatMessageContentsAsync(history, settings, _kernel);
        
        var result = new StringBuilder();

        await foreach (var message in responseStream)
        {
            result.Append(message.Content);
        }

        return result.ToString();
    }
}
