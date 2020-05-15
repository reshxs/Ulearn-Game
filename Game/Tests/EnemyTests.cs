using Game.Models;
using NUnit.Framework;

namespace Game.Tests
{
    [TestFixture]
    public class EnemyTests
    {
        [Test]
        [TestCase(0,0,0,0, true)]
        [TestCase(0,0,Enemy.Width + 1,0, false)]
        [TestCase(0,0,0,Enemy.Height + 1, false)]
        [TestCase(0,0,Enemy.Width,Enemy.Height, true)]
        public static void CollisionTest(int enemyX, int enemyY, int playerX, int playerY, bool expected)
        {
            var enemy = new Enemy(enemyX, enemyY);
            var player = new Player(playerX, playerY);
            var actual = enemy.CollidePlayer(player);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public static void MoveTest()
        {
            var player = new Player(50, 50);
            var enemy = new Enemy(0,0);
            var x = enemy.X;
            var y = enemy.Y;
            enemy.Move(player);
            Assert.True(enemy.X > x && enemy.Y > y);
            player = new Player(0, 0);
            enemy = new Enemy(50, 50);
            x = enemy.X;
            y = enemy.Y;
            enemy.Move(player);
            Assert.True(enemy.X < x && enemy.Y > y);
        }
    }
}