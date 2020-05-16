using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Game.Models;

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
            DrawEnemy(args, model.Enemy);
            DrawBonus(args, model.Bonus, Brushes.SaddleBrown, new Pen(Color.Gold, 3f), "?");
            DrawBonus(args, model.AmmoBox, Brushes.DarkSlateGray, new Pen(Color.Azure, 3f), "A");
            DrawScore(args, $"Score: {model.Score.ToString()}", 5, GameModel.Height - 75);
            DrawScore(args, $"HP: {model.Player.Hp.ToString()}", 600, GameModel.Height - 75);
            DrawScore(args, $"Ammo: {model.Ammo.ToString()}", 400, GameModel.Height - 75);
        }

        private static void DrawEnemy(PaintEventArgs args, Enemy enemy)
        {
            if (enemy != null)
                DrawGameObject(args, Brushes.Indigo, new Pen(Color.Azure, 3f), enemy.X, enemy.Y, Enemy.Width,
                    Enemy.Height);
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

        private static void DrawBonus(PaintEventArgs args, GameBonus bonus, Brush brush, Pen pen, string text)
        {
            if (bonus == null) return;
            DrawGameObject(args, brush, pen, bonus.X, bonus.Y,
                GameBonus.Width, GameBonus.Height);
            args.Graphics.DrawString(text, 
                new Font("Consolas", 17), 
                Brushes.Azure, 
                new RectangleF(bonus.X, bonus.Y, GameBonus.Width, GameBonus.Height),
                new StringFormat{ Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center, 
                });
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
            args.Graphics.DrawString(str, new Font("Consolas", 20), Brushes.Azure, x,y);
    }
}