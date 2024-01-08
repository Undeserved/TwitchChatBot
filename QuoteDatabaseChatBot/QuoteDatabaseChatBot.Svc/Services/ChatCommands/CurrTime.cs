using QuoteDatabaseChatBot.Application.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client.Events;

namespace QuoteDatabaseChatBot.Svc.Services.ChatCommands {
    public class CurrTime : IChatCommand {
        public string CommandCode => "currtime";

        public bool RegexPatternCommand => false;

        public async Task<string> ExecuteCommand(OnChatCommandReceivedArgs chatCommandReceivedArgs) {
            string args = chatCommandReceivedArgs.Command.ArgumentsAsString;
            DateTime currTime = DateTime.UtcNow;
            if (int.TryParse(args, out int gmtOffset)) {
                currTime = currTime.AddHours(gmtOffset);
            }
            return currTime.ToISO8601TimeString();
        }
    }
}
