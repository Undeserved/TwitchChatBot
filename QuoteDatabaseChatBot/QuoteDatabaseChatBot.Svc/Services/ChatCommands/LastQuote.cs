using MediatR;
using QuoteDatabaseChatBot.Application.Quotes.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client.Events;

namespace QuoteDatabaseChatBot.Svc.Services.ChatCommands {
    public class LastQuote : IChatCommand {
        public string CommandCode => "lastquote";
        public bool RegexPatternCommand => false;
        private IMediator _mediator;

        public LastQuote(IMediator mediator) {
            _mediator = mediator;
        }

        public async Task<string> ExecuteCommand(OnChatCommandReceivedArgs chatCommandReceivedArgs) {
            GetLastQuoteQuery lastQuoreQuery = new GetLastQuoteQuery();
            int lastQuote = await _mediator.Send(lastQuoreQuery);
            return $"!quote {lastQuote}";
        }
    }
}
