using System;

namespace Game
{
    public class Player
    {
        public const int Width = 30;
        public const int Height = 30;
        public int X { get; private set; }
        public int Y { get; private set; }
        public static int Speed { get; private set; }
        public int Hp { get; private set; }

        public Player(int x, int y)
        {
            X = x;
            Y = y;
            Hp = 3;
            Speed = 10;
        }

        public void KickPlayer() => Hp--;
        
        public void HealPlayer() => Hp++;

        public bool IsAlive() => Hp > 0;

        public void Move(MoveDirections direction)
        {
            switch (direction)
            {
                case(MoveDirections.Up):
                    Move(0, -Speed);
                    break;
                case(MoveDirections.Down):
                    Move(0, Speed);
                    break;
                case(MoveDirections.Right):
                    Move(Speed, 0);
                    break;
                case(MoveDirections.Left):
                    Move(-Speed, 0);
                    break;
                case MoveDirections.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }
        
        private void Move(int dx, int dy)
        {
            X += dx;
            Y += dy;
        }

        public static void CalculateSpeed(ulong time)
        {
            if (time % 20 == 0)
                Speed++;
        }

        public static void DownSpeed()
        {
            if (Speed > 3)
                Speed--;
        }

        public static void UpSpeed() => Speed += 2;
    }
}