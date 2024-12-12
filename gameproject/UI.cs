using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameproject
{
    public static class UI
    {
        public static void drawUI(Graphics g)
        {

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

