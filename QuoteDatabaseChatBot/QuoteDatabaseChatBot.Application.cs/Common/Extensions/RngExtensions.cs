namespace QuoteDatabaseChatBot.Application.Common.Extensions {
    public static class RngExtensions {
        private static Random _random = new Random();

        public static T Random<T>(this IEnumerable<T> source) {
            int randomIndex = _random.Next(source.Count());
            return source.ElementAt(randomIndex);
        }

        public static int RollDice(this Random random, int sides) {
            return random.Next(1, sides + 1);
        }

        public static bool NextBool(this Random random) {
            return random.Next(2) == 1;
        }
    }
}
