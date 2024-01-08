using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using QuoteDatabaseChatBot.Application.Wrapped.Queries;
using QuoteDatabaseChatBot.CalcEngine.Common.Models;
using QuoteDatabaseChatBot.CalcEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace QuoteDatabaseChatBot.CalcEngine.Services {
    internal class YearTotalsService : ICalcEngineService {
        private readonly IMediator _mediator;
        private readonly IOptions<ReportSettings> _settings;

        public YearTotalsService(IMediator mediator, IOptions<ReportSettings> settings) {
            _mediator = mediator;
            _settings = settings;
        }

        public void Calc() {
            GameTotalsQuery gameTotalsQuery = new GameTotalsQuery { Year = _settings.Value.TargetYear };
            var GameTotals = _mediator.Send(gameTotalsQuery);
            string gameTotals = JsonConvert.SerializeObject(GameTotals);
        }
    }
}
