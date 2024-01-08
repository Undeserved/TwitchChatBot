using MediatR;
using Microsoft.Extensions.Options;
using QuoteDatabaseChatBot.Application.Common.Extensions;
using QuoteDatabaseChatBot.Application.Quotes.Commands;
using QuoteDatabaseChatBot.Infrastructure.Settings;
using System.Text.RegularExpressions;
using TwitchLib.Client.Events;

namespace QuoteDatabaseChatBot.Svc.Services.MessageParsers {
    internal class QuoteScanner : IMessageParser {
        public string MessageRegexFormat { get; }
        private readonly IMediator _mediator;
        private readonly Regex QuoteRegex;

        public QuoteScanner(IMediator mediator, IOptions<ChatBotSettings> chatBotSettings) {
            _mediator = mediator;
            MessageRegexFormat = chatBotSettings.Value.RegexPatterns[nameof(QuoteScanner)];
            QuoteRegex = new Regex(MessageRegexFormat);
        }

        public async Task ParseMessage(OnMessageReceivedArgs messageReceivedArgs) {
            Match matches = QuoteRegex.Match(messageReceivedArgs.ChatMessage.Message);
            if (matches.Success) {
                UpsertQuoteCommand quote = new UpsertQuoteCommand {
                    QuoteId = int.Parse(matches.Groups[nameof(quote.QuoteId)].Value),
                    Quote = matches.Groups.GetValueOrDefault(nameof(quote.Quote))?.Value,
                    Game = matches.Groups.GetValueOrDefault(nameof(quote.Game))?.Value,
                    QuoteDate = matches.Groups[nameof(quote.QuoteDate)].Value.ToDateFromDayMonthYearString()
                };
                await _mediator.Send(quote);
            }
        }
    }
}
