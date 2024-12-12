using gameproject.Enemies;
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


        public BattleUI()
        { //later prevent this from running again if the player is already in battle
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
            drawOptions(g);
            drawSprite(BattleHandler.instance.enemy.sprite, g);
            drawEnemyInfo(g);

        }

        private void drawEnemyInfo(Graphics g) {
            var textPoint = new Point(GameForm.windowWidth / 2 - 150, GameForm.windowHeight / 2 - 240);
            g.DrawString(BattleHandler.instance.enemy.name, GameForm.font, Brushes.White, textPoint.X, textPoint.Y+32);
            var healthOutlineRect = new RectangleF(textPoint.X, textPoint.Y, 300, 30);
            var filledWidth = (double)BattleHandler.instance.enemy.health / (double)BattleHandler.instance.enemy.maxHealth * 300.0;
            var healthFillRect = new RectangleF(textPoint.X, textPoint.Y, (float)filledWidth, 30);
            g.DrawRectangle(Pens.Red, healthOutlineRect);
            g.FillRectangle(Brushes.Red, healthFillRect);
        }

        private void drawOptions(Graphics g) {
            var drawingYOffset = 0;
            Rectangle drawLater = new();
            foreach (var option in options) {
                var rectPoint = new Point(optionRect.X, optionRect.Y + drawingYOffset); //top left of the rectangle
                if (option.Value) {
                    drawLater = new Rectangle(optionRect.X, optionRect.Y + drawingYOffset, optionRect.Width, optionRect.Height);
                } else {
                    g.DrawRectangle(Pens.White, rectPoint.X, rectPoint.Y, optionRect.Width, optionRect.Height);
                }
                g.DrawString(option.Key, GameForm.font, Brushes.White, optionRect.X, rectPoint.Y);
                drawingYOffset += optionRect.Height;
            }
            g.DrawRectangle(Pens.Red, drawLater); //need to draw this last so its overtop
        }

        private void drawSprite(Image sprite, Graphics g) {
            var scaledSprite = new Bitmap(sprite, new Size(300,300));
            g.DrawImage(scaledSprite, GameForm.windowWidth/2-150, GameForm.windowHeight/2-150);
            scaledSprite.Dispose();
        }
    }
}
