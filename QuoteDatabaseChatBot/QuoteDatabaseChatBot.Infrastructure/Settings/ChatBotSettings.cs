using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuoteDatabaseChatBot.Infrastructure.Settings {
    public class ChatBotSettings {
        public string ClientId { get; set; }
        public string AppSecret { get; set; }
        public string TwitchAccount { get; set; }
        public string OAuthToken { get; set; }
        public string RefreshToken { get; set; }
        public string TargetChannel { get; set; }
        public string QuoteSource { get; set; }
        public Dictionary<string, string> RegexPatterns { get; set; }

        public ChatBotSettings() {
            RegexPatterns = new Dictionary<string, string>();
        }
    }
}
