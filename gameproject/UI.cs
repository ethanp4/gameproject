using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameproject
{
    public static class UI
    {
        private static string info = "";
        public static void drawUI(Graphics g)
        {
            info = $"{Game.posX} {Game.posY} {Game.dirX} {Game.dirY} {Game.planeX} {Game.planeY}";
            g.DrawString(info, GameForm.font, Brushes.White, new Point(0, 0));
        }
    }
}
