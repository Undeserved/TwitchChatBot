using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuoteDatabaseChatBot.Application.Common.Dtos.Wrapped {
    public class GameTotalsDto {
        public string Game { get; set; }
        public int Year { get; set; }
        public int Count { get; set; }
    }
}
