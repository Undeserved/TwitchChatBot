using MediatR;
using QuoteDatabaseChatBot.Application.Wrapped.Queries;
using QuoteDatabaseChatBot.Domain.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Api.Helix.Models.Games;
using TwitchLib.Api.Interfaces;
using TwitchLib.Client.Events;

namespace QuoteDatabaseChatBot.Svc.Services.ChatCommands {
    public class ClipQuery : IChatCommand {
        public string CommandCode => "clipquery";
        public bool RegexPatternCommand => false;

        private readonly ITwitchAPI _twitchAPI;
        private readonly IMediator _mediator;

        public ClipQuery(ITwitchAPI twitchAPI, IMediator mediator) {
            _twitchAPI = twitchAPI;
            _mediator = mediator;
        }

        public async Task<string> ExecuteCommand(OnChatCommandReceivedArgs chatCommandReceivedArgs) {
            //_twitchAPI.Helix.Clips.GetClipsAsync();
            //GetGamesResponse response = await _twitchAPI.Helix.Games.GetGamesAsync(gameNames: chatCommandReceivedArgs.Command.ArgumentsAsList);
            //if (response.Games.Any()) {
            //    return $"{response.Games[0].Name} : {response.Games[0].Id}";
            //}

            return "Your request was blocked due to bad credentials (Do you have the right scope for your access token?). Pengun";
            //GameTotalsQuery gtq = new GameTotalsQuery { Year = 2023, Filter = true };
            //var kex = await _mediator.Send(gtq);
            //int y = kex.Count();
            //return "lmao";
        }
    }
}
