using System;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;

namespace Game
{
    public class Enemy
    {
        public const int Width = 30;
        public const int Height = 30;
        public int X { get; private set; }
        public int Y { get; private set; }
        private int _hp;
        public readonly int Speed;

        public Enemy(int x, int y)
        {
            X = x;
            Y = y;
            _hp = 3;
            Speed = 5;
        }

        public void Move(Player player) => Move(ChoseEnemyDirection(player));

        private void Move(MoveDirections direction)
        {
            switch (direction)
            {
                case MoveDirections.Right:
                    Move(Speed);
                    break;
                case MoveDirections.Left:
                    Move(-Speed);
                    break;
                case MoveDirections.None:
                    Move(0);
                    break;
                case MoveDirections.Down:
                    Move(0);
                    break;
                case MoveDirections.Up:
                    Move(0);
                    break;
            }
        }

        public void KickEnemy() => _hp--;

        public bool IsAlive() => _hp > 0;

        public bool CollidePlayer(Player player) =>
            ContainsPoint(player.X, player.Y)
            || ContainsPoint(player.X + Player.Width, player.Y)
            || ContainsPoint(player.X, player.Y + Player.Height)
            || ContainsPoint(player.X + Player.Width,
                player.Y + Player.Height);
        
        public bool CollideBullet(Bullet bullet) =>
            ContainsPoint(bullet.X, bullet.Y)
            || ContainsPoint(bullet.X + Player.Width, bullet.Y)
            || ContainsPoint(bullet.X, bullet.Y + Player.Height)
            || ContainsPoint(bullet.X + Player.Width, bullet.Y + Player.Height);

        private bool ContainsPoint(int x, int y) =>  x >= X && y >= Y && x <= X + Width && y <= Y + Height;
        
        private MoveDirections ChoseEnemyDirection(Player player) => 
            player.X > X
                ? MoveDirections.Right
                : ((player.X == X) ? MoveDirections.None : MoveDirections.Left);

        private void Move(int dx)
        {
            X += dx;
            Y += Speed;
        }
    }
}