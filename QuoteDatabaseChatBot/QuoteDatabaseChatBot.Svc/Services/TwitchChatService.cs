using Microsoft.Extensions.Options;
using QuoteDatabaseChatBot.Infrastructure.Settings;
using QuoteDatabaseChatBot.Svc.Services.ChatCommands;
using QuoteDatabaseChatBot.Svc.Services.MessageParsers;
using System.Text.RegularExpressions;
using TwitchLib.Api;
using TwitchLib.Api.Interfaces;
using TwitchLib.Client.Events;
using TwitchLib.Client.Interfaces;

namespace QuoteDatabaseChatBot.Svc.Services {
    internal class TwitchChatService {
        private readonly ITwitchClient _twitchClient;
        private readonly ChatBotSettings _chatBotSettings;
        private readonly IEnumerable<IChatCommand> _commands;
        private readonly IEnumerable<IMessageParser> _messageParsers;

        public TwitchChatService(IOptions<ChatBotSettings> chatBotSettings, ITwitchClient twitchClient, IEnumerable<IChatCommand> commands, IEnumerable<IMessageParser> messageParsers) {
            _chatBotSettings = chatBotSettings.Value;
            _messageParsers= messageParsers;
            _twitchClient = twitchClient;
            _commands = commands;

            _twitchClient.OnConnected += TwitchClient_OnConnected;
            _twitchClient.OnMessageReceived += TwitchClient_OnMessageReceived;
            _twitchClient.OnChatCommandReceived += TwitchClient_OnChatCommandReceived;
            _twitchClient.OnLog += TwitchClient_OnLog;

            if (!_twitchClient.Connect()) {
                Console.WriteLine($"Failed to connect to {_chatBotSettings.TargetChannel}");
            }
        }

        private void TwitchClient_OnLog(object? sender, OnLogArgs e) {
            Console.WriteLine($"{e.DateTime.ToString("yyyy-MM-ddT HH:mm:ssz")}: {e.Data}");
        }

        private async void TwitchClient_OnChatCommandReceived(object? sender, OnChatCommandReceivedArgs e) {            
            IChatCommand? command = _commands
                .Where(x => (x.CommandCode == e.Command.CommandText.ToLower())
                        || (x.RegexPatternCommand && Regex.Match(e.Command.CommandText, x.CommandCode).Success))
                .FirstOrDefault();

            if (command == null) {
                return;
            }
            
            string message = await command.ExecuteCommand(e);
            if (!string.IsNullOrWhiteSpace(message)) {
                _twitchClient.SendMessage(e.Command.ChatMessage.Channel, message);
            }
        }

        private async void TwitchClient_OnMessageReceived(object? sender, OnMessageReceivedArgs e) {
            if(e.ChatMessage.Username.ToLower()  != _chatBotSettings.QuoteSource.ToLower()) {
                return;
            }

            IMessageParser? messageParser = _messageParsers
                .Where(x => Regex.Match(e.ChatMessage.Message, x.MessageRegexFormat).Success)
                .FirstOrDefault();
            if (messageParser == null) { 
                return;
            }
            await messageParser.ParseMessage(e);
        }

        private void TwitchClient_OnConnected(object? sender, OnConnectedArgs e) {
            Console.WriteLine($"Connected to {e.AutoJoinChannel}");
        }
    }
}
