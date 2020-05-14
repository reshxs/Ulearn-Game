using System;
using System.Net;

namespace Game
{
    public sealed class GameModel
    {
        public const int Width = 750;
        public const int Height = 1000;
        private const int BarriersCount = 4;
        public readonly Player Player = new Player(Width/2, Height - 100);
        public readonly Barrier[] Barriers = new Barrier[BarriersCount];
        public GameBonus Bonus { get; private set; }
        public Rocket Rocket { get; private set; }
        public Bullet Bullet { get; private set; }
        public int BulletsCount { get; private set; }
        public int Score { get; private set; }
        private ulong _prevTime;
        private readonly Random _random = new Random();
        public event ModelEventHandler NewTick;
        public event ModelEventHandler CollideBarrier;
        public event ModelEventHandler CollideBonus;
        public event ModelEventHandler CollideRocket;
        private readonly bool _godMode;

        public GameModel(bool godMode = false)
        {
            _godMode = godMode;
            BulletsCount = 1;
            CreateBarriers(BarriersCount);
            _prevTime = 0;
        }

        public bool NextTick(MoveDirections currentDirection, ulong time, bool bulletFlag)
        {
            if (time != _prevTime)
            {
                _prevTime = time;
                Score++;
                CalculateSpeed(time);
                CreateBonus(time);
                CreateRocket(time);
                OnNewTick(new ModelEventArgs());
            }
            
            CreateBullet(bulletFlag);
            MoveBullet();
            MoveBonus();
            MoveRocket();
            MoveBarriers();
            MovePlayer(currentDirection);

            return _godMode || Player.IsAlive();
        }

        private static void CalculateSpeed(ulong time)
        {
            Barrier.CalculateSpeed(time);
            Player.CalculateSpeed(time);
            GameBonus.CalculateSpeed(time);
        }

        private void MovePlayer(MoveDirections direction)
        {
            if(!_godMode)
            {
                CheckBarriersCollisions();
                CheckRocketCollision();
            }

            CheckBonusCollision();
            if ((Player.X - Player.Speed < 0 && direction == MoveDirections.Left)
                || (Player.X + Player.Speed >= Width - Player.Width && direction == MoveDirections.Right)
                || (Player.Y - Player.Speed < 0 && direction == MoveDirections.Up)
                || (Player.Y + Player.Speed >= Height - Player.Height) && direction == MoveDirections.Down)
                return;
            Player.Move(direction);
        }

        #region Barrier

        private void CreateBarriers(int barriersCount)
        {
            var y = Height - 300;
            for (var i = 0; i < barriersCount; i++)
            {
                var x = _random.Next(Width - Barrier.Width);
                y -= 300;
                Barriers[i] = new Barrier(x, y);
            }
        }

        private void MoveBarriers()
        {
            foreach (var barrier in Barriers) 
                MoveBarrier(barrier);
        }

        private static void MoveBarrier(Barrier barrier)
        {
            if (barrier.Y >= Height)
            {
                barrier.MoveUp(Width);
                return;
            }

            if (barrier.X + barrier.XSpeed < 0 || barrier.X + barrier.XSpeed > Width - Barrier.Width) 
                barrier.ChangeDirection();

            barrier.Move();
        }

        private void CheckBarriersCollisions()
        {
            foreach (var barrier in Barriers)
            {
                if (barrier.CollidePlayer(Player))
                {
                    Player.KickPlayer();
                    OnCollideBarrier(new ModelEventArgs());
                }

                if (Bullet != null && barrier.CollideBullet(Bullet))
                    Bullet = null;
            }
        }

        #endregion

        #region Bonus
        
        private void CreateBonus(ulong time)
        {
            if ((time + 10) % 20 != 0) return;
            Bonus = new GameBonus((BonusType)_random.Next((int)BonusType.Count - 1), 
                _random.Next(Width - GameBonus.Width), -10 * GameBonus.Height);
        }

        private void MoveBonus()
        {
            if (Bonus == null) return;
            Bonus.Move();
            if (Bonus.Y <= Height) return;
            Bonus = null;
        }

        private void CheckBonusCollision()
        {
            if (Bonus == null || !Bonus.CollidePlayer(Player))
                return;
            
            UseBonus(Bonus.Type);
            Bonus = null;
            OnCollideBonus(new ModelEventArgs());
        }

        private void UseBonus(BonusType bonus)
        {
            switch (bonus)
            {
                case BonusType.SpeedDown:
                    Barrier.DownSpeed();
                    Player.DownSpeed();
                    break;
                case BonusType.ExtraLive:
                    Player.HealPlayer();
                    break;
                case BonusType.ExtraScore:
                    Score += 30;
                    break;
                case BonusType.SpeedUp:
                    Player.UpSpeed();
                    break;
                case BonusType.ExtraBullet:
                    BulletsCount++;
                    break;
                case BonusType.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion

        #region Rocket

        private void CreateRocket(ulong time)
        {
            if ((time + 10) % 5 != 0) return;
            Rocket = new Rocket(_random.Next(Width - Rocket.Height));
        }

        private void MoveRocket()
        {
            if (Rocket == null) return;
            Rocket.Move();
            if (Rocket.Y < Height) return;
            Rocket = null;
        }

        private void CheckRocketCollision()
        {
            if (Rocket == null || !Rocket.CollidePlayer(Player))
                return;
            Player.KickPlayer();
            Rocket = null;
            OnCollideRocket(new ModelEventArgs());
        }

        #endregion
        
        #region Bullet

        private void CreateBullet(bool flag)
        {
            if (!flag || BulletsCount <= 0 || Bullet != null) 
                return;
            Bullet = new Bullet(Player.X, Player.Y);
            BulletsCount--;
        }

        private void MoveBullet()
        {
            if (Bullet == null)
                return;
            Bullet.Move();
            if (Bullet.Y + Bullet.Height <= 0)
                Bullet = null;
        }

        #endregion

        #region Events
        
        private void OnCollideBarrier(ModelEventArgs args) => CollideBarrier?.Invoke(this, args);

        private void OnNewTick(ModelEventArgs args) => NewTick?.Invoke(this, args);

        private void OnCollideBonus(ModelEventArgs args) => CollideBonus?.Invoke(this, args);

        private void OnCollideRocket(ModelEventArgs args) => CollideRocket?.Invoke(this, args);
        
        #endregion
    }
} 