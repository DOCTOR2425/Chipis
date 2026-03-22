using Chipis.API.DTOs;
using Chipis.Application.Abstractions;
using Chipis.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        [HttpGet("chats/{chatId:guid}/messages")]
        public async Task<ActionResult<List<MessageResponse>>> GetMessagesFromChat(
            [FromRoute] Guid chatId,
            [FromQuery] int take,
            [FromQuery] Guid? cursorId)
        {
            List<Message> messages = await _chatsService.GetMessagesByChatId(
                chatId, take + 1, cursorId);

            List<MessageResponse> responses = messages
                .Select(m => new MessageResponse(
                    m.MessageId,
                    m.Sender.UserId,
                    m.Text,
                    m.SentAt))
                .ToList();

            bool hasMore = responses.Count > take;
            Guid? nextCursor = responses.LastOrDefault()?.MessageResponseId;

            return Ok(new
            {
                messages = responses,
                nextCursor = hasMore ? responses.Last().MessageResponseId.ToString() : null,
                hasMore
            });
        }

        [Authorize]
        [HttpGet("chast")]
        public async Task<IActionResult> GetUserChats()
        {
            Guid userId = Guid.Parse(User
                .FindFirst(ClaimTypes.NameIdentifier).Value);

            List<Chat> chats = await _chatsService.GetChatsByUser(userId);

            return Ok(chats.Select(c => new ChatResponse(c.ChatId, c.Name)).ToList());
        }
    }
}
