using BestPracticesSK.API.Interfaces.Services;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace BestPracticesSK.API.Services;
public class ChatService : IChatService
{
    private readonly IChatCompletionService _chatCompletionService;
    private readonly Kernel _kernel;
    public ChatService(IChatCompletionService chatCompletionService, Kernel kernel)
    {
        _chatCompletionService = chatCompletionService;
        _kernel = kernel;
    }

    public async Task<string> GetChatResponseAsync(string userMessage)
    {
        var response = await _chatCompletionService.GetChatMessageContentAsync(userMessage);
        return response.ToString();
    }

    public async Task<string> GetChatResponseWithHistoryAsync(string userMessage)
    {
        var history = new ChatHistory();
        history.AddUserMessage("saat 12 uçağı Kayseri istanbul uçağı");
        history.AddUserMessage(userMessage);

        var settings = new PromptExecutionSettings { 
            ExtensionData = new Dictionary<string, object> 
            { 
                { "temperature", 0.7 }, 
                { "max_tokens", 150 }
            }
        };

        var response = await _chatCompletionService.GetChatMessageContentAsync(history, settings, _kernel);

        return response.ToString();

    }
}
