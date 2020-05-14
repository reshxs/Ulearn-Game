using NUnit.Framework;

namespace Game
{
    [TestFixture]
    public class PlayerTest
    {
        [Test]
        public static void HpTest()
        {
            var player = new Player(0,0);
            while (player.Hp > 0)
                player.KickPlayer();
            Assert.False(player.IsAlive());
            player.HealPlayer();
            Assert.True(player.IsAlive());
        }

        [Test]
        public static void MoveTest()
        {
            var player = new Player(100,100);
            player.Move(MoveDirections.Down);
            player.Move(MoveDirections.Right);
            player.Move(MoveDirections.Up);
            player.Move(MoveDirections.Left);
            Assert.AreEqual(100, player.X);
            Assert.AreEqual(100, player.Y);
        }
    }
}