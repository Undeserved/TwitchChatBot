using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client.Events;

namespace QuoteDatabaseChatBot.Svc.Services.ChatCommands {
    public class Quote502 : IChatCommand {
        public string CommandCode => "onlyfans";
        public bool RegexPatternCommand => false;

        public async Task<string> ExecuteCommand(OnChatCommandReceivedArgs chatCommandReceivedArgs) {
            return "!quote 502";
        }
    }
}
