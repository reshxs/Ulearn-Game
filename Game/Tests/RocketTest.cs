using Game.Models;
using NUnit.Framework;

namespace Game.Tests
{
    [TestFixture]
    public class RocketTest
    {
        [Test]
        [TestCase(0,0,10, true)]
        [TestCase(0,Rocket.Width + 1,10, false)]
        [TestCase(0,0,11 + Rocket.Height, false)]
        [TestCase(0,Rocket.Width,10 + Rocket.Height, true)]
        public static void CollisionTest(int rocketX, int playerX, int playerY, bool expected)
        {
            var rocket = new Rocket(rocketX);
            var player = new Player(playerX, playerY);
            while (rocket.Y <= 0)
                rocket.Move();
            var actual = rocket.CollidePlayer(player);
            Assert.AreEqual(expected, actual);
        }
    }
}