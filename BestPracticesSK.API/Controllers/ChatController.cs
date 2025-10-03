using BestPracticesSK.API.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace BestPracticesSK.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost("message")]
        public async Task<string> message(string userMessage)
        {
            var response = await _chatService.GetChatResponseAsync(userMessage);
            return response;
        }

        [HttpPost("messageWithHistory")]
        public async Task<string> messageWithHistory(string userMessage)
        {
            var response = await _chatService.GetChatResponseWithHistoryAsync(userMessage);
            return response;
        }
    }
}
