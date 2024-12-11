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
        public static BattleUI instance = null;

        private int enemyIndex { get; set; }

        public BattleUI(int enemyIndex)
        { //later prevent this from running again if the player is already in battle
            this.enemyIndex = enemyIndex;
            instance = this;
        }

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
                case Keys.Enter:
                    BattleHandler.instance.playerChoice(options.Keys.ElementAt(optionIndex));
                    break;
            }
        }
        public void drawUI(Graphics g) {
            drawSprite(enemyIndex, g);
            var drawingYOffset = 0;
            Rectangle drawLater = new(0,0,0,0);
            foreach (var option in options)
            {
                var rectPoint = new Point(optionRect.X, optionRect.Y + drawingYOffset);
                g.DrawRectangle(Pens.White, rectPoint.X, rectPoint.Y, optionRect.Width, optionRect.Height);
                g.DrawString(option.Key, GameForm.font, Brushes.White, optionRect.X, rectPoint.Y);
                if (option.Value)
                {
                    drawLater = new Rectangle(optionRect.X, optionRect.Y + drawingYOffset, optionRect.Width, optionRect.Height);
                }
                drawingYOffset += optionRect.Height;
            }
            g.DrawRectangle(Pens.Red, drawLater); //need to draw this overtop so it isnt covered
        }

        private void drawSprite(int spriteIndex, Graphics g) {
            var test = new Bitmap(300, 300);
            using (var graphics = Graphics.FromImage(test))
            {
                graphics.Clear(Color.WhiteSmoke);
                graphics.DrawString("I am a test image", GameForm.font, Brushes.Black, new Point(50, 50));
            }
            g.DrawImage(test, GameForm.windowWidth/2-150, GameForm.windowHeight/2-150);
            test.Dispose();
        }
    }
}
