using Game.Models;
using NUnit.Framework;

namespace Game.Tests
{
    [TestFixture]
    public class BarrierTests
    {
        [Test]
        [TestCase(0,0,0,0, true)]
        [TestCase(0,0,Barrier.Width + 1,0, false)]
        [TestCase(0,0,0,Barrier.Height + 1, false)]
        [TestCase(0,0,Barrier.Width,Barrier.Height, true)]
        public static void CollisionTest(int barrierX, int barrierY, int playerX, int playerY, bool expected)
        {
            var barrier = new Barrier(barrierX, barrierY);
            var player = new Player(playerX, playerY);
            var actual = barrier.CollidePlayer(player);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public static void MoveTest()
        {
            var barrier = new Barrier(0,0);
            var y = barrier.Y;
            barrier.Move();
            Assert.AreEqual(y + Barrier.YSpeed, barrier.Y);
            barrier.MoveUp(500);
            Assert.True(barrier.Y < 0 && barrier.X <= 500 - Barrier.Width);
        }
    }
}