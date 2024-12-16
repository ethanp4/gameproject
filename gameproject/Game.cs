using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameproject
{

    public static class Game
    {
        const int width = GameForm.windowWidth;
        const int height = GameForm.windowHeight;

        public static double dirX = 0, dirY = 1; //initial direction vector
        public static double planeX = 0.66, planeY = 0; //the 2d raycaster version of camera plane

        static double time = 0; //time of current frame
        static double oldTime = 0; //time of previous frame
        //setup rotation animation
        private static System.Windows.Forms.Timer rotationTimer = new();
        static Game()
        {
            rotationTimer.Interval = 16;
            rotationTimer.Tick += rotationAnim;

        }
        private static bool rotationDir = false;
        private static int rotationSteps = 10; //one step is 1/rotationSteps of the entire rotation and happens each frame
        private static int rotationStepProgress = 0;

        //actually important stuff
        public static double spawnX = 5.5, spawnY = 1.5;
        public static double posX = spawnX, posY = spawnY;
        public static int[,] worldMap { get { return worldMaps[currentStage % worldMaps.Length]; } }
        public static int[][,] worldMaps = {
            new[,]{ //stage 0
            { 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1 },
            { 1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1 },
            { 1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1 },
            { 1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1 },
            { 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,-1,1 },
            { 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,-1,1 },
            { 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,-1,1 },
            { 1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1 },
            { 1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1 },
            { 1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1 },
            { 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1 },},
            new[,]{ //stage 1
            { 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1 },
            { 1,1,1,0,0,0,0,0,0,2,0,0,0,0,0,0,1,1,1,1 },
            { 1,1,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,1,1,1 },
            { 1,1,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,1,1,1 },
            { 1,0,0,0,0,0,0,0,0,2,0,0,0,2,0,0,0,0,-1,1 },
            { 1,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,-1,1 },
            { 1,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,-1,1 },
            { 1,1,0,0,0,0,0,0,0,0,0,0,0,2,0,0,0,1,1,1 },
            { 1,1,0,0,0,0,0,0,0,0,0,0,0,2,0,0,0,1,1,1 },
            { 1,1,1,0,0,0,0,0,0,0,0,0,0,2,0,0,1,1,1,1 },
            { 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1 }},
            new[,]{ //stage 2
            { 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1 },
            { 1,1,1,0,0,0,2,0,0,0,0,2,0,0,0,0,1,1,1,1 },
            { 1,1,0,0,0,0,2,2,2,2,2,0,0,0,0,0,0,1,1,1 },
            { 1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1 },
            { 1,0,0,0,0,0,2,2,2,2,2,2,0,0,0,0,0,0,-1,1 },
            { 1,0,0,0,0,0,2,2,2,2,2,2,2,0,0,0,0,0,-1,1 },
            { 1,0,0,0,0,0,2,2,0,0,0,0,2,0,0,0,0,0,-1,1 },
            { 1,1,0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,1,1,1 },
            { 1,1,0,0,0,0,2,2,0,0,0,0,2,0,0,0,0,1,1,1 },
            { 1,1,1,0,0,0,2,2,2,2,2,2,2,0,0,0,1,1,1,1 },
            { 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1 }},
            new[,]{ //stage 3
            { 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1 },
            { 1,1,1,0,0,0,0,2,0,0,0,2,0,2,0,0,1,1,1,1 },
            { 1,1,0,0,0,0,0,0,0,2,0,0,2,0,0,0,0,1,1,1 },
            { 1,1,0,0,0,0,0,2,0,0,0,2,0,2,0,0,0,1,1,1 },
            { 1,0,0,0,0,0,0,0,0,2,0,2,0,0,0,0,0,0,-1,1 },
            { 1,0,0,0,0,0,0,2,0,0,0,2,0,2,0,0,0,0,-1,1 },
            { 1,0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,0,-1,1 },
            { 1,1,0,0,0,0,0,2,0,0,0,2,0,2,0,0,0,1,1,1 },
            { 1,1,0,0,0,0,0,0,0,2,0,2,0,0,0,0,0,1,1,1 },
            { 1,1,1,0,0,0,0,2,0,0,0,0,0,2,0,0,1,1,1,1 },
            { 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1 }},
            new[,]{ //stage 4
            { 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1 },
            { 1,1,1,0,0,0,0,2,2,2,0,0,0,0,2,0,1,1,1,1 },
            { 1,1,0,0,0,0,0,0,0,2,0,0,0,0,2,0,0,1,1,1 },
            { 1,1,0,0,0,0,0,2,0,2,2,0,0,0,2,2,2,1,1,1 },
            { 1,0,0,0,0,0,0,0,0,2,2,2,0,0,0,0,0,0,-1,1 },
            { 1,0,0,0,0,0,0,2,0,2,0,2,0,0,0,0,0,0,-1,1 },
            { 1,0,0,0,0,0,0,0,0,2,0,2,0,0,2,0,0,0,-1,1 },
            { 1,1,0,0,0,0,0,2,0,2,2,0,0,2,0,0,0,1,1,1 },
            { 1,1,0,0,0,0,0,2,0,0,0,0,2,0,0,0,0,1,1,1 },
            { 1,1,1,0,0,0,0,2,0,2,2,2,0,0,0,0,1,1,1,1 },
            { 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1 }},
        };

        private static Color[][] stageColors = new Color[][]
        {
            // Stage 0
            new Color[]
            {
                Color.FromArgb(24, 5, 26),        // Background: Black
                Color.FromArgb(255, 0, 0),      // Wall: Red
                Color.FromArgb(0, 255, 255),    // Accent 1: Cyan
                Color.FromArgb(255, 255, 0)     // Accent 2: Yellow
            },
    
            // Stage 1
            new Color[]
            {
                Color.FromArgb(0, 25, 25),  // Background: White
                Color.FromArgb(0, 0, 255),      // Wall: Blue
                Color.FromArgb(255, 165, 0),    // Accent 1: Orange
                Color.FromArgb(255, 105, 180)   // Accent 2: HotPink
            },
    
            // Stage 2
            new Color[]
            {
                Color.FromArgb(34, 34, 34),     // Background: Dark Gray
                Color.FromArgb(0, 128, 0),      // Wall: Green
                Color.FromArgb(255, 20, 147),   // Accent 1: Deep Pink
                Color.FromArgb(240, 230, 140)   // Accent 2: Khaki
            },
    
            // Stage 3
            new Color[]
            {
                Color.FromArgb(30, 0, 30),        // Background: Black
                Color.FromArgb(255, 165, 0),    // Wall: Orange
                Color.FromArgb(75, 0, 130),     // Accent 1: Indigo
                Color.FromArgb(255, 20, 147)    // Accent 2: Deep Pink
            },
    
            // Stage 4
            new Color[]
            {
                Color.FromArgb(0, 100, 100),    // Background: Cyan
                Color.FromArgb(255, 0, 255),    // Wall: Magenta
                Color.FromArgb(0, 128, 128),    // Accent 1: Teal
                Color.FromArgb(255, 105, 180)   // Accent 2: HotPink
            }
        };

        public static void restart() {
            currentStage = 0;
            gameState = STATE.FREE_MOVEMENT;
            posX = spawnX; posY = spawnY;
            Player.reset();
        }

        public static int currentStage { get; private set; } = 0; //can be read by ui class

        public enum STATE { FREE_MOVEMENT, BATTLE, GAME_OVER, IN_SHOP, YOU_WIN } //shop is unused
        public static STATE gameState { get; set; } = STATE.FREE_MOVEMENT; //dictates where input is handled and which draw functions are called

        private static void rotationAnim(object sender, EventArgs e) //rotation timer tick function
        {
            rotationStepProgress++;
            double pct = (double)rotationStepProgress / (double)rotationSteps;
            if (rotationStepProgress == rotationSteps)
            {
                rotationStepProgress = 0;
                rotationTimer.Stop();
            }

            var angleDifference = rotationDir ? 1.0 : -1.0;

            angleDifference *= 90 * (Math.PI / 180); //90 degrees in radians
            angleDifference /= (double)rotationSteps;

            double oldDirX = dirX;
            dirX = dirX * Math.Cos(angleDifference) - dirY * Math.Sin(angleDifference);
            dirY = oldDirX * Math.Sin(angleDifference) + dirY * Math.Cos(angleDifference);
            double oldPlaneX = planeX;
            planeX = planeX * Math.Cos(angleDifference) - planeY * Math.Sin(angleDifference);
            planeY = oldPlaneX * Math.Sin(angleDifference) + planeY * Math.Cos(angleDifference);

        }

        public static void rotate(bool dir)
        {
            if (!rotationTimer.Enabled)
            {
                rotationDir = dir;
                rotationTimer.Start();
            }
        }

        public static void move(bool dir)
        {
            var prevPos = new Point((int)posX, (int)posY);
            if (rotationTimer.Enabled) //dont move if rotating
            {
                return;
            }
            var intDir = dir ? 1 : -1;
            try
            {
                if (worldMap[Convert.ToInt32(posX - .5 + (intDir * dirX)), Convert.ToInt32(posY - .5)] <= 0) //values below zero will have no collision
                {
                    posX += (intDir * dirX);
                }
                if (worldMap[Convert.ToInt32(posX - .5), Convert.ToInt32(posY - .5 + (intDir * dirY))] <= 0)
                {
                    posY += (intDir * dirY);
                }
            }
            catch { Debug.WriteLine("index out of bounds in move()"); }
            if (worldMap[(int)posX, (int)posY] == -1)
            {
                moveToNextRoom();
            }
            if (new Point((int)posX, (int)posY) != prevPos)
            {
                considerSpawningEnemy();
            }
        }

        private static void moveToNextRoom() {
            if (currentStage == worldMaps.Length-1) {
                gameState = STATE.YOU_WIN;
                ImportantMessageText.setMessage("You win!\nPress enter to restart", 99999);
            } else {
                currentStage++;
                posX = spawnX; posY = spawnY;
            }
            //if (currentStage == 2) {
            //    gameState = STATE.IN_SHOP;
            //    ActionLog.appendAction("You entered the shop!", ActionLog.COLORS.SPECIAL);
            //    Shop.instance = new Shop(); //create new shop instance, from here it could be possible to make stage specific shop contents and stuff
            //}
        }
        static Random random = new Random();
        public static void considerSpawningEnemy()
        {
            if (random.NextDouble() < 0.10)
            {
                BattleHandler.initBattle(); //main entry point for a battle is this function
                Debug.WriteLine("Spawn enemy");
            }
        }

        public static void drawGame(Graphics g)
        {
            oldTime = time;
            time = Environment.TickCount;
            //rendering code in this for loop taken from https://lodev.org/cgtutor/raycasting.html
            var currentStageColors = stageColors[currentStage % stageColors.Length];
            g.FillRectangle(new SolidBrush(currentStageColors[0]), new Rectangle(0, 0, width, height)); //draw background first
            var pen = new Pen(Color.White);
            //draw lines
            for (int x = 0; x < width; x++)
            {
                double cameraX = 2.0 * x / width - 1.0;
                double rayDirX = dirX + planeX * cameraX;
                double rayDirY = dirY + planeY * cameraX;

                int mapX = (int)posX;
                int mapY = (int)posY;

                double sideDistX;
                double sideDistY;

                double deltaDistX = (rayDirX == 0) ? 1e30 : Math.Abs(1.0 / rayDirX);
                double deltaDistY = (rayDirY == 0) ? 1e30 : Math.Abs(1.0 / rayDirY);

                double perpWallDist;

                int stepX;
                int stepY;

                int hit = 0;
                int side = 0;

                if (rayDirX < 0)
                {
                    stepX = -1;
                    sideDistX = (posX - mapX) * deltaDistX;
                }
                else
                {
                    stepX = 1;
                    sideDistX = (mapX + 1.0 - posX) * deltaDistX;
                }
                if (rayDirY < 0)
                {
                    stepY = -1;
                    sideDistY = (posY - mapY) * deltaDistY;
                }
                else
                {
                    stepY = 1;
                    sideDistY = (mapY + 1.0 - posY) * deltaDistY;
                }

                while (hit == 0)
                {
                    if (sideDistX < sideDistY)
                    {
                        sideDistX += deltaDistX;
                        mapX += stepX;
                        side = 0;
                    }
                    else
                    {
                        sideDistY += deltaDistY;
                        mapY += stepY;
                        side = 1;
                    }
                    try
                    {
                        if (worldMap[mapX, mapY] > 0) hit = 1;
                    }
                    catch { Debug.WriteLine("index out of bounds in while(hit == 0)"); hit = 1; }
                }

                if (side == 0) perpWallDist = (sideDistX - deltaDistX);
                else perpWallDist = (sideDistY - deltaDistY);

                int lineHeight = (int)(height / perpWallDist);

                int drawStart = -lineHeight / 2 + height / 2;
                if (drawStart < 0) drawStart = 0;
                int drawEnd = lineHeight / 2 + height / 2;
                if (drawEnd >= height) drawEnd = height - 1;

                Color color = currentStageColors[worldMap[mapX, mapY]];

                //give x and y sides different brightness
                if (side == 1) { color = Color.FromArgb(color.R / 2, color.G / 2, color.B / 2); }
                pen.Color = color; //this pen is reused to use way less memory
                //draw the pixels of the stripe as a vertical line
                g.DrawLine(pen, new Point(x, drawStart), new Point(x, drawEnd));
            }
            //done for loop

        }
    }
}
