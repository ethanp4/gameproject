using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameproject
{
    public static class UI
    {
        public static void drawPlayerBars(Graphics g)
        {
            
            int barWidth = 300;
            int barHeight = 20;
            int barX = 20; 
            int hpBarY = 20; 
            int mpBarY = 50; 

            
            using (Font smallFont = new Font(GameForm.font.FontFamily, 12)) 
            {
                // Player HP Bar
                float hpPercentage = (float)Player.health / Player.getMaxHealth();
                g.FillRectangle(Brushes.Gray, barX, hpBarY, barWidth, barHeight); // background
                g.FillRectangle(Brushes.Green, barX, hpBarY, (int)(barWidth * hpPercentage), barHeight); 
                g.DrawRectangle(Pens.Black, barX, hpBarY, barWidth, barHeight); //border 
                string hpText = $"HP: {Player.health}/{Player.getMaxHealth()}";
                SizeF hpTextSize = g.MeasureString(hpText, smallFont);
                g.DrawString(hpText, smallFont, Brushes.White,
                    barX + (barWidth - hpTextSize.Width) / 2, hpBarY + (barHeight - hpTextSize.Height) / 2); 

                // Player MP Bar
                float mpPercentage = (float)Player.MP / Player.maxMP;
                g.FillRectangle(Brushes.Gray, barX, mpBarY, barWidth, barHeight); // background
                g.FillRectangle(Brushes.Blue, barX, mpBarY, (int)(barWidth * mpPercentage), barHeight); 
                g.DrawRectangle(Pens.Black, barX, mpBarY, barWidth, barHeight); // border
                string mpText = $"MP: {Player.MP}/{Player.maxMP}";
                SizeF mpTextSize = g.MeasureString(mpText, smallFont);
                g.DrawString(mpText, smallFont, Brushes.White,
                    barX + (barWidth - mpTextSize.Width) / 2, mpBarY + (barHeight - mpTextSize.Height) / 2); 
            }
        }

        public static void drawUI(Graphics g)
        {
            var canadianDollarsDisplay = $"C$: {Player.canadianDollars}";
            var displayPoint = new Point(10, 70); 
            g.DrawString(canadianDollarsDisplay, GameForm.font, Brushes.White, displayPoint);
        }
        public static void drawMinimap(Graphics g, int windowWidth, int windowHeight)
        {
            // Constants for the minimap
            const int minimapSize = 200; // Total size of the minimap in pixels
            const int minimapCellSize = 10; // Size of each grid cell on the minimap
            const int minimapMargin = 10; // Margin from the edge of the screen

            // Calculate the top-left position of the minimap
            int mapStartX = windowWidth - minimapSize - minimapMargin;
            int mapStartY = minimapMargin;

            // Draw the minimap background
            g.FillRectangle(Brushes.Black, mapStartX, mapStartY, minimapSize, minimapSize);

            // Iterate over the world map
            for (int y = 0; y < Game.worldMap.GetLength(0); y++)
            {
                for (int x = 0; x < Game.worldMap.GetLength(1); x++)
                {
                    // Determine the color of each cell
                    Color cellColor;
                    if (Game.worldMap[y, x] > 0)
                    {
                        cellColor = Color.Gray; // Walls
                    }
                    else if (Game.worldMap[y, x] == -1)
                    {
                        cellColor = Color.Yellow; // Goal
                    }
                    else
                    {
                        cellColor = Color.Black; // Empty spaces
                    }

                    // Draw each cell
                    Brush cellBrush = new SolidBrush(cellColor);
                    g.FillRectangle(cellBrush,
                        mapStartX + x * minimapCellSize, // X-coordinate
                        mapStartY + y * minimapCellSize, // Y-coordinate
                        minimapCellSize,
                        minimapCellSize);
                }
            }

            // Draw the player's position on the minimap
            Brush playerBrush = Brushes.Red;

            // Adjust player's position for the minimap
            float playerX = mapStartX + (float)(Game.posX * minimapCellSize);
            float playerY = mapStartY + (float)(Game.posY * minimapCellSize);

            g.FillEllipse(playerBrush, playerX - 3, playerY - 3, 6, 6); // Small dot for the player
        }
    }
}

