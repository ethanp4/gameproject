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
        private Dictionary<string, bool> options = new() { { "Attack", true }, { "Skill", false }, { "Defend", false }, { "Run", false } };
        private Dictionary<string, bool> skillsMenu = new() { { "Heal", true } }; // Submenu for skills
        private bool isInSkillsMenu = false; // Tracks if the player is in the Skills submenu
        private Rectangle optionRect = new((int)(GameForm.windowWidth * 0.75), (int)(GameForm.windowHeight * 0.75), 90, 30);
        private int optionIndex = 0;
        private int skillIndex = 0; // Tracks the selected skill index
        public static BattleUI instance = null;

        public BattleUI()
        { //later prevent this from running again if the player is already in battle
            instance = this;
        }

        private void upArrow()
        {
            if (isInSkillsMenu)
            {
                skillsMenu[skillsMenu.Keys.ElementAt(skillIndex)] = false; //set existing skill to false
                skillIndex--;
                if (skillIndex < 0)
                {
                    skillIndex = skillsMenu.Count - 1;
                }
                skillsMenu[skillsMenu.Keys.ElementAt(skillIndex)] = true; //set new skill to true
            }
            else
            {
                options[options.Keys.ElementAt(optionIndex)] = false; //set existing option to false
                optionIndex--;
                if (optionIndex < 0)
                {
                    optionIndex = options.Count - 1;
                }
                options[options.Keys.ElementAt(optionIndex)] = true; //set new option to true
            }
        }

        private void downArrow()
        {
            if (isInSkillsMenu)
            {
                skillsMenu[skillsMenu.Keys.ElementAt(skillIndex)] = false; //set existing skill to false
                skillIndex++;
                if (skillIndex >= skillsMenu.Count)
                {
                    skillIndex = 0;
                }
                skillsMenu[skillsMenu.Keys.ElementAt(skillIndex)] = true; //set new skill to true
            }
            else
            {
                options[options.Keys.ElementAt(optionIndex)] = false; //set existing option to false
                optionIndex++;
                if (optionIndex >= options.Count)
                {
                    optionIndex = 0;
                }
                options[options.Keys.ElementAt(optionIndex)] = true; //set new option to true
            }
        }

        public void handleInput(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    upArrow();
                    break;
                case Keys.Down:
                    downArrow();
                    break;
                case Keys.Enter:
                    if (isInSkillsMenu)
                    {
                        handleSkillSelection(skillsMenu.Keys.ElementAt(skillIndex));
                    }
                    else
                    {
                        handleMainOptionSelection(options.Keys.ElementAt(optionIndex));
                    }
                    break;
                case Keys.Escape:
                    if (isInSkillsMenu)
                    {
                        isInSkillsMenu = false; // Exit the skills submenu
                    }
                    break;
            }
        }

        private void handleMainOptionSelection(string option)
        {
            if (option == "Skill")
            {
                isInSkillsMenu = true; // Open the skills submenu
            }
            else
            {
                BattleHandler.instance.playerChoice(option); // Handle other options like Attack, Defend, Run
            }
        }

        private void handleSkillSelection(string skill)
        {
            if (skill == "Heal")
            {
                if (Player.MP >= 5)
                {
                    BattleHandler.instance.playerChoice("Heal");
                }
                else
                {
                    ActionLog.appendAction("Not enough MP!", ActionLog.COLORS.SYSTEM);
                }
                isInSkillsMenu = false; // Return to the main menu
            }
        }

        public void drawUI(Graphics g)
        {
            if (isInSkillsMenu)
            {
                drawSkillsMenu(g);
            }
            else
            {
                drawOptions(g);
            }
            drawSprite(BattleHandler.instance.enemy.sprite, g);
            drawEnemyInfo(g);
        }

        private void drawEnemyInfo(Graphics g)
        {
            var textPoint = new Point(GameForm.windowWidth / 2 - 150, GameForm.windowHeight / 2 - 240);
            g.DrawString(BattleHandler.instance.enemy.ToString(), GameForm.font, Brushes.White, textPoint.X, textPoint.Y + 32);
            var healthOutlineRect = new RectangleF(textPoint.X, textPoint.Y, 300, 30);
            var filledWidth = (double)BattleHandler.instance.enemy.health / (double)BattleHandler.instance.enemy.maxHealth * 300.0;
            var healthFillRect = new RectangleF(textPoint.X, textPoint.Y, (float)filledWidth, 30);
            g.DrawRectangle(Pens.Red, healthOutlineRect);
            g.FillRectangle(Brushes.Red, healthFillRect);
        }

        private void drawSkillsMenu(Graphics g)
        {
            var drawingYOffset = 0;
            Rectangle drawLater = new();
            foreach (var skill in skillsMenu)
            {
                var rectPoint = new Point(optionRect.X, optionRect.Y + drawingYOffset); //top left of the rectangle
                if (skill.Value)
                {
                    drawLater = new Rectangle(optionRect.X, optionRect.Y + drawingYOffset, optionRect.Width, optionRect.Height);
                }
                else
                {
                    g.DrawRectangle(Pens.White, rectPoint.X, rectPoint.Y, optionRect.Width, optionRect.Height);
                }
                g.DrawString(skill.Key, GameForm.font, Brushes.White, optionRect.X, rectPoint.Y);
                drawingYOffset += optionRect.Height;
            }
            g.DrawRectangle(Pens.Red, drawLater); //need to draw this last so it's overtop
        }

        private void drawOptions(Graphics g)
        {
            var drawingYOffset = 0;
            Rectangle drawLater = new();
            foreach (var option in options)
            {
                var rectPoint = new Point(optionRect.X, optionRect.Y + drawingYOffset); //top left of the rectangle
                if (option.Value)
                {
                    drawLater = new Rectangle(optionRect.X, optionRect.Y + drawingYOffset, optionRect.Width, optionRect.Height);
                }
                else
                {
                    g.DrawRectangle(Pens.White, rectPoint.X, rectPoint.Y, optionRect.Width, optionRect.Height);
                }
                g.DrawString(option.Key, GameForm.font, Brushes.White, optionRect.X, rectPoint.Y);
                drawingYOffset += optionRect.Height;
            }
            g.DrawRectangle(Pens.Red, drawLater); //need to draw this last so it's overtop
        }

        private void drawSprite(Image sprite, Graphics g)
        {
            var scaledSprite = new Bitmap(sprite, new Size(300, 300));
            g.DrawImage(scaledSprite, GameForm.windowWidth / 2 - 150, GameForm.windowHeight / 2 - 150);
            scaledSprite.Dispose();
        }
    }
}
