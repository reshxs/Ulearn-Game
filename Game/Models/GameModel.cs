using System;
using System.Windows.Forms;

namespace Game
{
    public class GameModel
    {
        public const int Width = 750;
        public const int Height = 1000;
        private const int BarriersCount = 4;
        public readonly Player Player = new Player(Width/2, Height - 100);
        public readonly Barrier[] Barriers = new Barrier[BarriersCount];
        public GameBonus Bonus { get; private set; }
        public GameBonus AmmoBox { get; private set; }
        public Rocket Rocket { get; private set; }
        public Bullet Bullet { get; private set; }
        public int Ammo { get; private set; }
        public int Score { get; private set; }
        public Enemy Enemy { get; private set; }
        private ulong _prevTime;
        private readonly Random _random = new Random();
        public event ModelEventHandler NewTick;
        public event ModelEventHandler CollideBarrier;
        public event ModelEventHandler CollideBonus;
        public event ModelEventHandler CollideRocket;

        public GameModel()
        {
            CreateBarriers(BarriersCount);
            Ammo = 5;
            _prevTime = 0;
        }
        
        #region Events
        
        private void OnCollideBarrier(ModelEventArgs args) => CollideBarrier?.Invoke(this, args);

        private void OnNewTick(ModelEventArgs args) => NewTick?.Invoke(this, args);

        private void OnCollideBonus(ModelEventArgs args) => CollideBonus?.Invoke(this, args);

        private void OnCollideRocket(ModelEventArgs args) => CollideRocket?.Invoke(this, args);
        
        #endregion

        #region mainActions

        public bool NextTick(MoveDirections currentDirection, ulong time, bool bulletFlag)
        {
            NextTimeTick(time);
            CreateBullet(bulletFlag);
            MoveObjects(currentDirection);
            return Player.IsAlive();
        }

        private void MoveObjects(MoveDirections currentDirection)
        {
            MoveBullet();
            MoveBonus();
            MoveRocket();
            MoveBarriers();
            MoveAmmoBox();
            MovePlayer(currentDirection);
            MoveEnemy(currentDirection);
        }

        private void NextTimeTick(ulong time)
        {
            if (time == _prevTime) 
                return;
            _prevTime = time;
            Score++;
            CalculateSpeed(time);
            CreateBonus(time);
            CreateAmmoBox(time);
            CreateRocket(time);
            CreateEnemy(time);
            OnNewTick(new ModelEventArgs());
        }

        private static void CalculateSpeed(ulong time)
        {
            Barrier.CalculateSpeed(time);
            Player.CalculateSpeed(time);
            GameBonus.CalculateSpeed(time);
        }

        private void MovePlayer(MoveDirections direction)
        {
            CheckBarriersCollisions();
            CheckRocketCollision();
            CheckBonusCollision();
            CheckAmmoBoxCollision();
            if ((Player.X - Player.Speed < 0 && direction == MoveDirections.Left)
                || (Player.X + Player.Speed >= Width - Player.Width && direction == MoveDirections.Right)
                || (Player.Y - Player.Speed < 0 && direction == MoveDirections.Up)
                || (Player.Y + Player.Speed >= Height - Player.Height) && direction == MoveDirections.Down)
                return;
            Player.Move(direction);
        }
        
        #endregion

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
                    Ammo++;
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
            if (!flag || Ammo <= 0 || Bullet != null) 
                return;
            Bullet = new Bullet(Player.X + Player.Width / 2 - Bullet.Width / 2, Player.Y);
            Ammo--;
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

        #region AmmoBox

        private void CreateAmmoBox(ulong time)
        {
            if (time % 20 != 0) return;
            AmmoBox = new GameBonus(BonusType.None, 
                _random.Next(Width - GameBonus.Width),- GameBonus.Height);
        }

        private void CheckAmmoBoxCollision()
        {
            if (AmmoBox == null || !AmmoBox.CollidePlayer(Player))
                return;
            Ammo += 5;
            AmmoBox = null;
            OnCollideBonus(new ModelEventArgs());
        }

        private void MoveAmmoBox()
        {
            if (AmmoBox == null)
                return;
            AmmoBox.Move();
            if (AmmoBox.Y >= Height)
                AmmoBox = null;
        }

        #endregion

        #region Enemy

        private void CreateEnemy(ulong time)
        {
            if (time % 15 != 0)
                return;
            Enemy = new Enemy(Player.X, -Enemy.Height);
        }

        private void MoveEnemy(MoveDirections direction)
        {
            CheckEnemyCollision();
            if (Enemy == null 
                || Enemy.X - Enemy.Speed < 0 && direction == MoveDirections.Left 
                || Enemy.X + Enemy.Speed > Width - Enemy.Height && direction == MoveDirections.Right)
                return;
            Enemy.Move(Player);
            if (Enemy.Y <= Height) return;
            Player.KickPlayer();
            Enemy = null;
            OnCollideBarrier(new ModelEventArgs());
        }
        
        private void CheckEnemyCollision()
        {
            if (Enemy != null && Enemy.CollidePlayer(Player))
            {
                Player.KickPlayer();
                Enemy = null;
                OnCollideBarrier(new ModelEventArgs());
            }

            if (Enemy == null || Bullet == null || !Enemy.CollideBullet(Bullet)) 
                return;
            Enemy.KickEnemy();
            Bullet = null;
            if (!Enemy.IsAlive())
                Enemy = null;
        }

        #endregion
    }
} 