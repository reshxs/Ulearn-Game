using System.Drawing;
using System.Windows.Forms;

namespace Game
{
    public class GameForm : Form
    {
        private GameModel _model;
        private readonly Timer _animationTimer = new Timer() {Interval = 1};
        private readonly Timer _scoreTimer = new Timer() {Interval = 500};
        private ulong _time;
        private bool _isPaused;
        private MoveDirections _currentDirection = MoveDirections.None;
        private bool _xPressed;

        public GameForm()
        {
            _model = new GameModel();
            _time = 0;
            _isPaused = false;
            SetFormProperties();
            SetAnimationTimer();
            SetMinimizeActions();
            SetKeyboardActions();
            SetAnimators();

            _scoreTimer.Tick += (sender, args) => _time++;
            Paint += (sender, args) => Drawer.DrawGameModel(args, _model);
        }

        private void SetFormProperties()
        {
            Size = new Size(GameModel.Width, GameModel.Height);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            BackColor = Color.MidnightBlue;
            Location = new Point(50,50);
            DoubleBuffered = true;
            Text = @"ASS BURNER";
        }

        private void SetAnimationTimer()
        {
            _animationTimer.Tick += (sender, args) =>
            {
                var result = _model.NextTick(_currentDirection, _time, _xPressed);
                if (!result)
                    Lose();
            };
            _animationTimer.Tick += (sender, args) => Invalidate();
        }

        private void SetKeyboardActions()
        {
            KeyDown += (sender, args) =>
            {
                if (args.KeyData != Keys.Space) return;
                if (_isPaused)
                {
                    UnpauseGame();
                    BackColor = Color.MidnightBlue;
                }
                else
                {
                    PauseGame();
                    BackColor = Color.DimGray;
                }
            };
            KeyDown += (sender, args) =>
            {
                if (args.KeyData == Keys.X)
                    _xPressed = true;
            };
            KeyUp +=(sender, args) =>
            {
                if (args.KeyData == Keys.X)
                    _xPressed = false;
            };
            KeyDown += (sender, args) => { _currentDirection = ChooseDirection(args); };
            KeyUp += (sender, args) =>
            {
                if (ChooseDirection(args) == _currentDirection)
                    _currentDirection = MoveDirections.None;
            };
        }
        
        private void SetMinimizeActions()
        {
            Activated += (sender, args) => { UnpauseGame(); };
            Deactivate += (sender, args) => { PauseGame(); };
        }

        private void SetAnimators()
        {
            _model.CollideBarrier += (sender, args) => BackColor = Color.Brown;
            _model.CollideBonus += (sender, args) => BackColor = Color.RoyalBlue; 
            _model.NewTick += (sender, args) => BackColor = Color.MidnightBlue;
            _model.CollideRocket += (sender, args) => BackColor = Color.Brown;
        }

        private void PauseGame()
        {
            _animationTimer.Stop();
            _scoreTimer.Stop();
            _isPaused = true;
        }

        private void UnpauseGame()
        {
            _animationTimer.Start();
            _scoreTimer.Start();
            _isPaused = false;
        }
        
        private void Lose()
        {
            PauseGame();
            var result = MessageBox.Show(@"Try again?", @"Game over!", 
                MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
                Restart();
            else
                Close();
        }

        private void Restart()
        {
            _model = new GameModel();
            _time = 0;
            _currentDirection = MoveDirections.None;
            UnpauseGame();
            BackColor = Color.MidnightBlue;
            SetAnimators();
        }

        private MoveDirections ChooseDirection(KeyEventArgs args)
        {
            switch (args.KeyData)
            {
                case (Keys.Up):
                    return  MoveDirections.Up;
                case (Keys.Down):
                    return  MoveDirections.Down;
                case (Keys.Right):
                    return MoveDirections.Right;
                case (Keys.Left):
                    return MoveDirections.Left;
                default:
                    return MoveDirections.None;
            }
        }
    }
}