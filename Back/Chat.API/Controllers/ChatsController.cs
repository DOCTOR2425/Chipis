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

        [HttpGet("chat/{chatId:guid}/messages")]
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
                    m.SentAt,
                    m.IsChanged,
                    m.IsReaded))
                .ToList();

            bool hasMore = responses.Count > take;
            Guid? nextCursor = responses.LastOrDefault()?.MessageId;

            return Ok(new
            {
                messages = responses,
                nextCursor = hasMore ? responses.Last().MessageId.ToString() : null,
                hasMore
            });
        }

        [Authorize]
        [HttpGet("chats")]
        public async Task<IActionResult> GetUserChats()
        {
            Guid userId = Guid.Parse(User
                .FindFirst(ClaimTypes.NameIdentifier).Value);

            List<Chat> chats = await _chatsService.GetChatsByUser(userId);

            return Ok(chats.Select(c => new ChatResponse(c.ChatId, c.Name)).ToList());
        }

        [Authorize]
        [HttpPost("createChat/{userId2:guid}")]
        public async Task<IActionResult> CreateChat(
            [FromRoute] Guid userId2)
        {
            Guid userId = Guid.Parse(User
                .FindFirst(ClaimTypes.NameIdentifier).Value);

            Chat chat = await _chatsService.CreateChat(userId, userId2);

            return Ok(new ChatResponse(chat.ChatId, chat.Name));
        }

        [HttpPost("createChatTest/{userId:guid}/{userId2:guid}")]
        public async Task<IActionResult> CreateChat(
            [FromRoute] Guid userId,
            [FromRoute] Guid userId2)
        {
            var chat = await _chatsService.CreateChat(userId, userId2);

            return Ok(chat);
        }

        [Authorize]
        [HttpGet("chat/{chatId:guid}/search/{text}")]
        public async Task<ActionResult<List<MessageResponse>>> SearchMessages(
            [FromRoute] Guid chatId,
            [FromRoute] string text,
            [FromQuery] bool? isSinglWord)
        {
            List<Message> messages = await _chatsService.SearchMessages(chatId, text, isSinglWord);

            return Ok(messages
                .Select(m => new MessageResponse(
                    m.MessageId,
                    m.Sender.UserId,
                    m.Text,
                    m.SentAt,
                    m.IsChanged,
                    m.IsReaded))
                .ToList());
        }
    }
}
