namespace BestPracticesSK.API.Interfaces.Services;
public interface IChatService
{
    Task<string> GetChatResponseAsync(string userMessage);

    Task<string> GetChatResponseWithHistoryAsync(string userMessage);
    Task<string> GetChatStreamingResponseAsync(string userMessage);
}
