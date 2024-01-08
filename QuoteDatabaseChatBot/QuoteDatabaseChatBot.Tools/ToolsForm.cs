using MediatR;
using QuoteDatabaseChatBot.Application.Common.Dtos;
using QuoteDatabaseChatBot.Application.Quotes.Commands;
using QuoteDatabaseChatBot.Application.Quotes.Queries;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace QuoteDatabaseChatBot.Tools {
    public partial class ToolsForm : Form {
        private IMediator _mediator;
        public ToolsForm(IMediator mediator) {
            _mediator = mediator;
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e) {
            GetQuoteQuery quoteQuery = new GetQuoteQuery { To = DateTime.Now };
            IEnumerable<QuoteDto> quotes = await _mediator.Send(quoteQuery);
            string quotesAsString = JsonSerializer.Serialize(quotes);
            int i = 0;
        }

        private async void button2_Click(object sender, EventArgs e) {
            UpsertQuoteCommand command = new UpsertQuoteCommand { Game = "Metal Gear Solid V: The Phantom Pain", Quote = "We'll get another child for the road. Little road child action!", QuoteDate = DateTime.Now.Date, QuoteId = 670 };
            await _mediator.Send(command);
        }
    }
}