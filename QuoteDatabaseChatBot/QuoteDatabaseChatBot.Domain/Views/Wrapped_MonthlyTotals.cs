using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuoteDatabaseChatBot.Domain.Views {
    [Keyless]
    public class Wrapped_MonthlyTotals {
        public DateTime Month { get; set; }
        public int Count { get; set; }
    }
}
