using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Game
{
    public static class Drawer
    {
        public static void DrawGameModel(PaintEventArgs args, GameModel model)
        {
            args.Graphics.SmoothingMode = SmoothingMode.HighSpeed;
            DrawRocket(args, model);
            DrawBarriers(args, model);
            DrawBullet(args, model.Bullet);
            DrawPlayer(args, model.Player);
            DrawBonus(args, model.Bonus);
            DrawScore(args, model.Score.ToString(), 5, 10);
            DrawScore(args, model.Player.Hp.ToString(), 600, 10);
            DrawScore(args, model.BulletsCount.ToString(), 400, 10);
        }

        private static void DrawBullet(PaintEventArgs args, Bullet bullet)
        {
            if (bullet != null)
                DrawGameObject(args, Brushes.Brown, new Pen(Color.Red, 3f), bullet.X, bullet.Y, Bullet.Width, Bullet.Height);
        }

        private static void DrawBarriers(PaintEventArgs args, GameModel model)
        {
            foreach (var barrier in model.Barriers)
            {
                if (!barrier.IsCollided)
                    DrawGameObject(args, Brushes.DarkGreen, new Pen(Color.Lime, 3f), barrier.X, barrier.Y,
                        Barrier.Width, Barrier.Height);
            }
        }

        private static void DrawRocket(PaintEventArgs args, GameModel model)
        {
            if (model.Rocket != null)
            {
                DrawGameObject(args, Brushes.DarkRed, new Pen(Color.Red, 3f), model.Rocket.X, model.Rocket.Y,
                    Rocket.Width, Rocket.Height);
                if (model.Rocket.Y <= 0)
                    args.Graphics.FillRectangle(
                        new SolidBrush(Color.FromArgb((model.Rocket.Y + 500) / 5, Color.Red)),
                        model.Rocket.X, 0, Rocket.Width, GameModel.Height);
            }
        }

        private static void DrawBonus(PaintEventArgs args, GameBonus bonus)
        {
            if (bonus != null)
            {
                DrawGameObject(args, Brushes.SaddleBrown, new Pen(Color.Gold, 3f), bonus.X, bonus.Y,
                    GameBonus.Width, GameBonus.Height);
            }
        }

        private static void DrawPlayer(PaintEventArgs args, Player player) =>
            DrawGameObject(args, Brushes.Indigo, new Pen(Color.Fuchsia, 3f), player.X, player.Y,
                Player.Width, Player.Height);

        private static void DrawGameObject(PaintEventArgs args, Brush innerBrush, Pen outerPen, 
            int x, int y, int width, int height)
        {
            args.Graphics.FillRectangle(innerBrush, x, y, width, height);
            args.Graphics.DrawRectangle(outerPen, x, y, width, height);
        }
        
        private static void DrawScore(PaintEventArgs args, string str, int x, int y) => 
            args.Graphics.DrawString(str, new Font("Consolas", 40), Brushes.Azure, x,y);
    }
}