using QuoteDatabaseChatBot.Application.Common.Extensions;

namespace QuoteDatabaseChatBot.Application.Common.Dtos {
    public class QuoteDto {
        public int QuoteId { get; set; }
        public string QuoteContent { get; set; }
        public string Game { get; set; }
        public DateTime Date { get; set; }

        public string FormattedQuote => $"Quote #{QuoteId} {QuoteContent} [{Game}] [{Date.ToISO8601String()}]";
    }
}
