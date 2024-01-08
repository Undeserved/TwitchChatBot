using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuoteDatabaseChatBot.Application.Common.Dtos.Wrapped {
    public class QuoteFrequencyDto {
        public string Game { get; set; }
        public int Year { get; set; }
        public DateTime MinDate { get; set; }
        public DateTime MaxDate { get; set; }
        public int QuoteCount { get; set; }
        public double Frequency { get; set; }
    }
}
