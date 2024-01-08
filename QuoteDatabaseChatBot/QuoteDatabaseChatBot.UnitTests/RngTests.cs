using QuoteDatabaseChatBot.Application.Common.Extensions;

namespace QuoteDatabaseChatBot.UnitTests {
    [TestClass]
    public class RngTests {

        Random _random= new Random();

        [TestMethod]
        public void OneSidedDice() {
            var results = RollDice(100000, 1);
            Assert.AreEqual(results.Sum(x => x.Value), 100000);
            Assert.AreEqual(results.Where(x => x.Value > 0).Count(), 1);
        }

        [TestMethod]
        public void NSidedDice() {
            var results = RollDice(100000, 100);
            Assert.AreEqual(results.Sum(x => x.Value), 100000);
            Assert.AreEqual(results.Where(x => x.Value > 0).Count(), 100);
        }

        [TestMethod]
        public void DiceBias() {
            var results = RollDice(100000, 3);
            Assert.IsFalse(results.Where(x => x.Value > 35000).Any());
        }

        [TestMethod]
        public void FlipCoin() {
            Dictionary<bool, int> results = new Dictionary<bool, int>();
            results.Add(true, 0);
            results.Add(false, 0);
            int coins = 100000;
            double biasThreshold = 0.51;
            for (int i = 0; i < 100000; i++) {
                results[_random.NextBool()]++;
            }
            Assert.IsFalse(results[true] >= coins * biasThreshold);
            Assert.IsFalse(results[false] >= coins * biasThreshold);
        }

        private Dictionary<int, int> RollDice(int rollCount, int sides) {
            Dictionary<int, int> results = new Dictionary<int, int>();
            for (int i = 0; i < 100000; i++) {
                int val = _random.RollDice(sides);
                if (!results.ContainsKey(val)) {
                    results.Add(val, 0);
                }
                results[val]++;
            }
            return results;
        }
    }
}