using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameproject
{
    internal class BattleUI
    {
        private Dictionary<string, bool> options = new() { { "Attack", true }, { "Defend", false }, { "Run", false } };
        private Rectangle optionRect = new((int)(GameForm.windowWidth * 0.75), (int)(GameForm.windowHeight * 0.75), 90, 30);
        private int optionIndex = 0;
        public static BattleUI instance = new BattleUI();

        private void upArrow()
        {
            options[options.Keys.ElementAt(optionIndex)] = false; //set existing option to false
            optionIndex--;
            if (optionIndex < 0)
            {
                optionIndex = options.Count - 1;
            }
            options[options.Keys.ElementAt(optionIndex)] = true; //set new option to true

        }
        private void downArrow()
        {
            options[options.Keys.ElementAt(optionIndex)] = false;
            optionIndex++;
            if (optionIndex >= options.Count)
            {
                optionIndex = 0;
            }
            options[options.Keys.ElementAt(optionIndex)] = true;
        }
        public void handleInput(KeyEventArgs e) {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    upArrow();
                    break;
                case Keys.Down:
                    downArrow();
                    break;
                case Keys.Space:
                    BattleHandler.instance.playerChoice(options.Keys.ElementAt(optionIndex));
                    break;
            }
        }
        public void drawUI(Graphics g) {
            var drawingYOffset = 0;
            Rectangle drawLater = new();
            foreach (var option in options)
            {
                var rectPoint = new Point(optionRect.X, optionRect.Y + drawingYOffset); //top left of the rectangle
                if (option.Value)
                {
                    drawLater = new Rectangle(optionRect.X, optionRect.Y + drawingYOffset, optionRect.Width, optionRect.Height);
                } else {
                    g.DrawRectangle(Pens.White, rectPoint.X, rectPoint.Y, optionRect.Width, optionRect.Height);
                }
                g.DrawString(option.Key, GameForm.font, Brushes.White, optionRect.X, rectPoint.Y);
                drawingYOffset += optionRect.Height;
            }
            g.DrawRectangle(Pens.Red, drawLater); //need to draw this last so its overtop
        }
    }
}
