using MediatR;
using QuoteDatabaseChatBot.Application.Quotes.Queries;
using TwitchLib.Client.Events;

namespace QuoteDatabaseChatBot.Svc.Services.ChatCommands {
    public class GetLastQuote : IChatCommand {
        public string CommandCode => "getlastquote";
        public bool RegexPatternCommand => false;
        private IMediator _mediator;

        public GetLastQuote(IMediator mediator) {
            _mediator = mediator;
        }

        public async Task<string> ExecuteCommand(OnChatCommandReceivedArgs chatCommandReceivedArgs) {
            GetLastQuoteQuery lastQuoreQuery = new GetLastQuoteQuery();
            int lastQuote = await _mediator.Send(lastQuoreQuery);
            return $"!quote {lastQuote}";
        }
    }
}
