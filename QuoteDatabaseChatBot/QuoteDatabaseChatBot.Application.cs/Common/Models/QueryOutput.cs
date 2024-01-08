using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuoteDatabaseChatBot.Application.Common.Models {
    public enum QueryOutput {
        First,
        Latest,
        Count,
        Random,
        Default
    }
}
