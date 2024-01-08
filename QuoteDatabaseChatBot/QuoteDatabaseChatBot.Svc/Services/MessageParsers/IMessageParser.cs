using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client.Events;

namespace QuoteDatabaseChatBot.Svc.Services.MessageParsers {
    internal interface IMessageParser {
        string MessageRegexFormat { get; }
        Task ParseMessage(OnMessageReceivedArgs messageReceivedArgs);
    }
}
