using Chipis.Application.Abstractions;
using Chipis.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chipis.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatsController : ControllerBase
    {
        private readonly IChatsService _chatsService;

        public ChatsController(IChatsService chatsService)
        {
            this._chatsService = chatsService;
        }

        [HttpGet("getAllMessagesInChat/{chatId:guid}")]
        public async Task<ActionResult<List<Message>>> GetAllMessagesInChat(Guid chatId)
        {
            return Ok(await _chatsService.GetAllMessagesByChatId(chatId));
        }
    }
}
