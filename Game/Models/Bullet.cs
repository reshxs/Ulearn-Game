namespace Game.Models
{
    public class Bullet
    {
        public const int Width = 10;
        public const int Height = 30;
        private const int Speed = 15;
        public int X { get; private set; }
        public int Y { get; private set; }

        public Bullet(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void Move() => Y -= Speed;
    }
}