using TwitchLib.Client.Events;

namespace QuoteDatabaseChatBot.Svc.Services.ChatCommands {
    internal interface IChatCommand {
        string CommandCode { get; }
        bool RegexPatternCommand { get; }
        Task<string> ExecuteCommand(OnChatCommandReceivedArgs chatCommandReceivedArgs);
    }
}
