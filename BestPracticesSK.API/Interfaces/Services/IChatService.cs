namespace BestPracticesSK.API.Interfaces.Services;
public interface IChatService
{
    Task<string> GetChatResponseAsync(string userMessage);
}
