using System;
using System.Windows.Forms;

namespace Game
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.Run(new GameForm());
        }
    }
}
