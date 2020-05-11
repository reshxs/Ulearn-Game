using System.ComponentModel;

namespace Game
{
    public class GameBonus
    {
        public const int Width = 30;
        public const int Height = 30;
        public int X { get; private set; }
        public int Y { get; private set; }
        public readonly BonusType Type;
        private static int _speed;
        
        public GameBonus(BonusType type, int x, int y)
        {
            Type = type;
            X = x;
            Y = y;
            _speed = 3;
        }

        public void Move() => Y += _speed;

        public static void CalculateSpeed(ulong time)
        {
            if (time % 10 != 0) 
                return;
            _speed += 2;
        }

        private bool ContainsPoint(int x, int y) => x >= X && y >= Y && x <= X + Width && y <= Y + Height;
        
        public bool CollidePlayer(Player player) =>
            ContainsPoint(player.X, player.Y)
            || ContainsPoint(player.X + Player.Width, player.Y)
            || ContainsPoint(player.X, player.Y + Player.Height)
            || ContainsPoint(player.X + Player.Width, player.Y + Player.Height);
    }
}