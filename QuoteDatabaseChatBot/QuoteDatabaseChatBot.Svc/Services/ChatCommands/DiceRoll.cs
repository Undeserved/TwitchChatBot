using Microsoft.Extensions.Options;
using QuoteDatabaseChatBot.Application.Common.Extensions;
using QuoteDatabaseChatBot.Infrastructure.Settings;
using System.Text.RegularExpressions;
using TwitchLib.Client.Events;

namespace QuoteDatabaseChatBot.Svc.Services.ChatCommands {
    internal class DiceRoll : IChatCommand {
        public string CommandCode { get; }
        public bool RegexPatternCommand => true;

        private readonly Regex DiceRegex;
        private readonly Random diceRng;
        private readonly List<string> excludedRolls;

        public DiceRoll(IOptions<ChatBotSettings> chatBotSettings) {
            CommandCode = chatBotSettings.Value.RegexPatterns[nameof(DiceRoll)];  
            DiceRegex = new Regex(CommandCode);
            diceRng = new Random();
            excludedRolls = new List<string> {
                "8",
                "20",
                "420"
            };
        }

        public async Task<string> ExecuteCommand(OnChatCommandReceivedArgs chatCommandReceivedArgs) {
            string Sides = DiceRegex.Match(chatCommandReceivedArgs.Command.CommandText).Groups[nameof(Sides)].Value;
            if (!excludedRolls.Contains(Sides)
                && int.TryParse(Sides, out int _sides)
                && _sides > 0) {

                if (chatCommandReceivedArgs.Command.ArgumentsAsList.Any()) {
                    List<string> rolls = chatCommandReceivedArgs.Command.ArgumentsAsList
                        .Where(arg => !string.IsNullOrWhiteSpace(arg))
                        .Select(arg => $"{arg} rolled: {diceRng.RollDice(_sides)}")
                        .ToList();
                    return string.Join(",\t", rolls);
                } else {
                    return $"D{_sides} rolled: {diceRng.RollDice(_sides)}";
                }
            }
            return string.Empty;
        }
    }
}
