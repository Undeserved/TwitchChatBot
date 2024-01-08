using MediatR;
using QuoteDatabaseChatBot.Application.Common.Dtos;
using QuoteDatabaseChatBot.Application.Quotes.Queries;
using TwitchLib.Client.Events;

namespace QuoteDatabaseChatBot.Svc.Services.ChatCommands {
    public class PullQuote : IChatCommand {
        public string CommandCode => "pullquote";
        public bool RegexPatternCommand => false;
        private IMediator _mediator;

        public PullQuote(IMediator mediator) {
            _mediator = mediator;
        }

        public async Task<string> ExecuteCommand(OnChatCommandReceivedArgs chatCommandReceivedArgs) {
            int.TryParse(chatCommandReceivedArgs.Command.ArgumentsAsString, out int quoteId);
            PullQuoteQuery query = new PullQuoteQuery { QuoteId = quoteId };
            QuoteDto response = await _mediator.Send(query);
            return response.FormattedQuote;
        }
    }
}
