using BestPracticesSK.API.Interfaces.Services;
using Microsoft.SemanticKernel;

namespace BestPracticesSK.API.Services;
public class ChatService : IChatService
{
    private readonly Kernel _kernel;

    public ChatService(Kernel kernel)
    {
        _kernel = kernel;
    }

    public async Task<string> GetChatResponseAsync(string userMessage)
    {
        var response = await _kernel.InvokePromptAsync(userMessage);
        return response.ToString();
    }
}
