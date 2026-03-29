using Chipis.Application.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

[Authorize]
public class ChatHub : Hub
{
    private readonly IMessagesService _messagesService;

    public ChatHub(IMessagesService messagesService)
    {
        _messagesService = messagesService;
    }

    public override async Task OnConnectedAsync()
    {
        Console.WriteLine($"User {Context.UserIdentifier} connected");
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        Console.WriteLine($"User {Context.UserIdentifier} disconnected");
        await base.OnDisconnectedAsync(exception);
    }

    public async Task JoinChat(string chatId)
    {
        if (string.IsNullOrEmpty(chatId))
            throw new ArgumentException("ChatId cannot be null or empty");

        if (Context.UserIdentifier == null)
            throw new UnauthorizedAccessException("User not authenticated");

        await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
        Console.WriteLine($"User {Context.UserIdentifier} joined chat {chatId}");
    }

    public async Task SendMessage(SendMessageRequest request)
    {
        // Проверки
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        if (string.IsNullOrEmpty(request.ChatId))
            throw new ArgumentException("ChatId is required");

        if (string.IsNullOrEmpty(request.Text))
            throw new ArgumentException("Text is required");

        if (Context.UserIdentifier == null)
            throw new UnauthorizedAccessException("User not authenticated");

        // Парсим ID
        if (!Guid.TryParse(request.ChatId, out Guid chatGuid))
            throw new ArgumentException("Invalid ChatId format");

        Guid senderId = Guid.Parse(Context.UserIdentifier);

        // Сохраняем сообщение
        var saved = await _messagesService.SaveNewMessage(
            request.Text,
            chatGuid,
            senderId);

        // Формируем ответ
        var outgoing = new MessageResponse(
            saved.MessageId,
            saved.Sender.UserId,
            saved.Text,
            saved.SentAt,
            false,
            false
        );

        // Отправляем всем в чат
        await Clients.Group(request.ChatId).SendAsync("ReceiveMessage", outgoing);
        Console.WriteLine($"Message sent to chat {request.ChatId} from user {senderId}");
    }
}

// Request/Response models
public class SendMessageRequest
{
    public string ChatId { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
}

public record MessageResponse(
    Guid MessageId,
    Guid SenderId,
    string Text,
    DateTime SentAt,
    bool IsChanged,
    bool IsReaded
);