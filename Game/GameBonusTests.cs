using NUnit.Framework;

namespace Game
{
    [TestFixture]
    public class GameBonusTests
    {
        [Test]
        [TestCase(0,0,0,0, true)]
        [TestCase(0,0,GameBonus.Width + 1,0, false)]
        [TestCase(0,0,0,GameBonus.Height + 1, false)]
        [TestCase(0,0,GameBonus.Width,GameBonus.Height, true)]
        public static void CollisionTest(int bonusX, int bonusY, int playerX, int playerY, bool expected)
        {
            var bonus = new GameBonus(BonusType.ExtraScore, bonusX, bonusY);
            var player = new Player(playerX, playerY);
            var actual = bonus.CollidePlayer(player);
            Assert.AreEqual(expected, actual);
        }
    }
}