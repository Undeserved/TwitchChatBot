namespace QuoteDatabaseChatBot.Application.Common.Extensions {
    public static class FormattingExtensions {
        public static string ToISO8601String(this DateTime date) {
            return date.ToString("yyyy-MM-dd");
        }

        public static string ToISO8601TimeString(this DateTime date) {
            return date.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }

        public static DateTime ToDateFromDayMonthYearString(this string date) {
            string dateFormat = "dd/MM/yyyy";
            if (DateTime.TryParseExact(date, dateFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime parsedDate)) {
                return parsedDate;
            }
            throw new FormatException($"The value didn't follow the expect {dateFormat} date format.");
        }

        public static List<string> SplitMessage(this string message) {
            return message.Chunk(500)
                .Select(x => new string(x))
                .ToList();
        }
    }
}
