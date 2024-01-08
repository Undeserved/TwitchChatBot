using LinqKit;
using MediatR;
using QuoteDatabaseChatBot.Application.Common.Dtos;
using QuoteDatabaseChatBot.Application.Common.Extensions;
using QuoteDatabaseChatBot.Application.Quotes.Queries;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using TwitchLib.Client.Events;

namespace QuoteDatabaseChatBot.Svc.Services.ChatCommands {
    internal class QuoteContains : IChatCommand {
        private const string query_parameter_random = "-rand";
        private const string query_parameter_equals = "-exact";
        public string CommandCode => "quotecontains";
        public bool RegexPatternCommand => false;
        private readonly IMediator _mediator;
        private readonly List<string> _parameters;

        public QuoteContains(IMediator mediator) {
            _mediator = mediator;
            _parameters = new List<string> {
                query_parameter_equals,
                query_parameter_random
            };
        }

        public async Task<string> ExecuteCommand(OnChatCommandReceivedArgs chatCommandReceivedArgs) {
            List<string> keywords = chatCommandReceivedArgs.Command.ArgumentsAsList.Where(x => !_parameters.Contains(x.ToLower())).ToList();

            if (chatCommandReceivedArgs.Command.ArgumentsAsList.Contains(query_parameter_equals)) {
                keywords = new List<string>() { string.Join(" ", keywords).Trim() };
            }

            GetQuoteQuery quoteQuery = new GetQuoteQuery {
                Contains = keywords                
            };

            IEnumerable<QuoteDto> matchingQuotes = await _mediator.Send(quoteQuery);
            int quoteCount = matchingQuotes.Count();

            if(quoteCount == 0) {
                return "No matching records found Pengun" ;
            }

            if (quoteCount == 1) {
                return  $"!quote {matchingQuotes.First().QuoteId}" ;
            }

            if (chatCommandReceivedArgs.Command.ArgumentsAsList.Any(x => x.ToLower() == query_parameter_random)) {
                int randomId = matchingQuotes.Random().QuoteId;
                return $"!quote {randomId}";
            }

            string message = $"{quoteCount} matches found: {String.Join("\t", matchingQuotes.Select(x => x.QuoteId.ToString()))}.";
            if(message.Length > 500) {
                message = $"{quoteCount} matches found, the list exceeds the maximum message length.";
            }
            return message;
        }
    }
}
