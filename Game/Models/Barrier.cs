using System;

namespace Game.Models
{
    public class Barrier
    {
        public const int Width = 200;
        public const int Height = 30;
        public int X { get; private set; }
        public int Y { get; private set; }
        private static int _xSpeed;
        public static int YSpeed { get; private set; }
        private bool _moveLeft;
        public bool IsCollided { get; private set; }
        public int XSpeed => _moveLeft ? -_xSpeed : _xSpeed;


        public Barrier(int x, int y)
        {
            X = x;
            Y = y;
            _xSpeed = 0;
            YSpeed = 3;
        }

        public void Move()
        {
            X += XSpeed;
            Y += YSpeed;
        }

        public void MoveUp(int rightBorder)
        {
            X = new Random().Next(rightBorder - Width);
            Y = -200;
            IsCollided = false;
        }

        public void ChangeDirection() => _moveLeft = !_moveLeft;

        private bool ContainsPoint(int x, int y)
        {
            if (IsCollided || x < X || x > X + Width || y < Y || y > Y + Height) 
                return false;
            IsCollided = true;
            return true;
        }

        public bool CollidePlayer(Player player) =>
            ContainsPoint(player.X, player.Y)
            || ContainsPoint(player.X + Player.Width, player.Y)
            || ContainsPoint(player.X, player.Y + Player.Height)
            || ContainsPoint(player.X + Player.Width, player.Y + Player.Height);
        
        public bool CollideBullet(Bullet bullet) =>
            ContainsPoint(bullet.X, bullet.Y)
            || ContainsPoint(bullet.X + Player.Width, bullet.Y)
            || ContainsPoint(bullet.X, bullet.Y + Player.Height)
            || ContainsPoint(bullet.X + Player.Width, bullet.Y + Player.Height);
        
        public bool CollideEnemy(Enemy enemy) =>
            ContainsPoint(enemy.X, enemy.Y)
            || ContainsPoint(enemy.X + Player.Width, enemy.Y)
            || ContainsPoint(enemy.X, enemy.Y + Player.Height)
            || ContainsPoint(enemy.X + Player.Width, enemy.Y + Player.Height);
        

        public static void CalculateSpeed(ulong time)
        {
            if (time % 10 != 0) 
                return;
            
            YSpeed++;
            if (time >= 50)
                _xSpeed++;
        }

        public static void DownSpeed()
        {
            if (_xSpeed > 0)
                _xSpeed -= 2;
            if (YSpeed > 3)
                YSpeed -= 2;
        }
    }
}