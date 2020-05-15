using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Game
{
    public class Rocket
    {
        public const int Width = 30;
        public const int Height = 50;
        public int X { get; private set; }
        public int Y { get; private set; }
        private const int Speed = 15;

        public Rocket(int x)
        {
            X = x;
            Y = -10 * Height;
        }
        
        private bool ContainsPoint(int x, int y) => x >= X && y >= Y && x <= X + Width && y <= Y + Height;
        
        public bool CollidePlayer(Player player) =>
            ContainsPoint(player.X, player.Y)
            || ContainsPoint(player.X + Player.Width, player.Y)
            || ContainsPoint(player.X, player.Y + Player.Height)
            || ContainsPoint(player.X + Player.Width, player.Y + Player.Height);

        public void Move() => Y += Speed;
    }
}