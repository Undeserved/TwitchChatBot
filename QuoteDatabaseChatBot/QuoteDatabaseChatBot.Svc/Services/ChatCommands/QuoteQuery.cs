using MediatR;
using Newtonsoft.Json;
using QuoteDatabaseChatBot.Application.Common.Dtos;
using QuoteDatabaseChatBot.Application.Common.Extensions;
using QuoteDatabaseChatBot.Application.Quotes.Queries;
using System.Text.Json.Nodes;
using TwitchLib.Client.Events;

namespace QuoteDatabaseChatBot.Svc.Services.ChatCommands {
    public class QuoteQuery : IChatCommand {
        public string CommandCode => "query";
        public bool RegexPatternCommand => false;

        private readonly IMediator _mediator;

        public QuoteQuery(IMediator mediator) {
            _mediator = mediator;
        }

        public async Task<string> ExecuteCommand(OnChatCommandReceivedArgs chatCommandReceivedArgs) {
            GetQuoteQuery query;
            try {
                query = JsonConvert.DeserializeObject<GetQuoteQuery>(chatCommandReceivedArgs.Command.ArgumentsAsString);
                if (query.ArgsAreEmpty()) {
                    return "The query must include at least one (1) non-null argument. Pengun";
                }
            } catch (Exception) {
                return "Unexpected query format. Pengun";
            }

            IEnumerable<QuoteDto> matchingQuotes = await _mediator.Send(query);
            int quoteCount = matchingQuotes.Count();

            if (quoteCount == 0) {
                return "No matching records found. Pengun";
            }

            switch (query.Output) {
                case Application.Common.Models.QueryOutput.Count:
                    return $"{quoteCount} matches found.";
                case Application.Common.Models.QueryOutput.First:
                    return $"!quote {matchingQuotes.First().QuoteId}";
                case Application.Common.Models.QueryOutput.Random:
                    return $"!quote {matchingQuotes.Random().QuoteId}";
                case Application.Common.Models.QueryOutput.Latest:
                    return $"!quote {matchingQuotes.Max(x => x.QuoteId)}";
            }

            if (quoteCount == 1) {
                return $"!quote {matchingQuotes.First().QuoteId}";
            }

            string message = $"{quoteCount} matches found: {String.Join("\t", matchingQuotes.Select(x => x.QuoteId.ToString()))}.";
            if (message.Length > 500) {
                message = $"{quoteCount} matches found, the list exceeds the maximum message length.";
            }
            return message;
        }
    }
}
