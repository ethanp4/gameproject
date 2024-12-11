using System.Diagnostics;
using System.Drawing;
using System.Security.Principal;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace gameproject {
    public partial class GameForm : Form {
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        const int framerate = 60;
        public static Point mPos;
        public const int windowWidth = 1440;
        public const int windowHeight = 1080;

        public GameForm() {
            InitializeComponent();
            DoubleBuffered = true;
            Width = windowWidth;
            Height = windowHeight;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            timer.Interval = (int)Math.Floor(1f / (float)framerate * 1000f); // frametime for 60 fps
            timer.Tick += invalidateTimer;
            timer.Start();
        }

        private void invalidateTimer(object sender, EventArgs e) {
            Invalidate(); //repaint once every 16 ms for 60 fps
        }

        protected override void OnPaint(PaintEventArgs e) {
            var g = e.Graphics; // graphics object to draw with
            Game.drawGame(g);
            UI.drawUI(g);
            //base.OnPaint(e); // idk if this is needed
        }

        private void formMouseMove(object sender, MouseEventArgs e) {
            mPos = e.Location; //this needs to be set from here in order to get the local position
        }

        private void formKeyDown(object sender, KeyEventArgs e) {
            switch (e.KeyCode) {
                case Keys.Right:
                    Game.rotate(false);
                    break;
                case Keys.Left:
                    Game.rotate(true);
                    break;
                case Keys.Up:
                    Game.move(true);
                    break;
                case Keys.Down:
                    Game.move(false);
                    break;
            }
        }
    }

    public static class UI {
        private static Font font = new Font("Times New Roman", 16);
        public static void drawUI(Graphics g) {
            var info = $"{Game.posX} {Game.posY} {Game.dirX} {Game.dirY} {Game.planeX} {Game.planeY}";
            //g.DrawString("Mouse position: " + GameForm.mPos.ToString(), font, Brushes.Black, new Point(0,0));
            g.DrawString(info, font, Brushes.White, new Point(0,0));


        }
    }

    public static class Game {
        const int width = GameForm.windowWidth;
        const int height = GameForm.windowHeight;

        //unused for now
        //private static Dictionary<int, double[]> dirSteps = new() {
        //    { 0, new[]{ 0.0, 1.0 } },
        //    { 1, new[]{ 1.0, 0.0 } },
        //    { 2, new[]{ 0.0, -1.0} },
        //    { 3, new[]{ -1.0, 0.0} },
        //};
        //private static Dictionary<int, double[]> planeSteps = new() {
        //    { 0, new[]{ 0.66, 0.0 } },
        //    { 1, new[]{ 0.0, -0.66 } },
        //    { 2, new[]{ -0.66, 0.0} },
        //    { 3, new[]{ 0.0, 0.66} },
        //};


        //public static double posX = 21.5, posY = 11.5;  //x and y start position, only ever modified by +/- 1
        public static double dirX = 0, dirY = 1; //initial direction vector
        public static double planeX = 0.66, planeY = 0; //the 2d raycaster version of camera plane

        static double time = 0; //time of current frame
        static double oldTime = 0; //time of previous frame
        static double frameTime {  get { return (time - oldTime) / 1000f; } } //currently unused

        //setup rotation
        private static System.Windows.Forms.Timer rotationTimer = new();
        static Game()
        {
            rotationTimer.Interval = 16;
            rotationTimer.Tick += rotationAnim;
        }
        //private static double rotationAnimLength = 0.5;
        private static bool rotationDir = false;
        private static int rotationSteps = 20; //0.5 seconds at 60 fps
        private static int rotationStepProgress = 0;
        
        //actually important stuff
        public static double spawnX = 5.5, spawnY = 1.5;
        public static double posX = spawnX, posY = spawnY;
        static int[,] worldMap = { //-1 is the goal
            { 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1 },
            { 1,1,1,0,0,0,0,0,0,0,0,1,1,1,1 },
            { 1,1,0,0,0,0,0,0,0,0,0,0,1,1,1 },
            { 1,1,0,0,0,0,0,0,0,0,0,0,1,1,1 },
            { 1,0,0,0,0,0,0,0,0,0,0,0,0,-1,1 },
            { 1,0,0,0,0,0,0,0,0,0,0,0,0,-1,1 },
            { 1,0,0,0,0,0,0,0,0,0,0,0,0,-1,1 },
            { 1,1,0,0,0,0,0,0,0,0,0,0,1,1,1 },
            { 1,1,0,0,0,0,0,0,0,0,0,0,1,1,1 },
            { 1,1,1,0,0,0,0,0,0,0,0,1,1,1,1 },
            { 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1 },
        };
 
        //first index is stage
        //first value in each is the background color, then each number within the map
        //private static Color[][] stageColors =
        //{ 
        //    new Color[]{ Color.Black, Color.DarkGray },
        //    new Color[]{ Color.DarkBlue , Color.DarkGray },
        //    new Color[]{ Color.Black, Color.DarkGray },
        //    new Color[]{ Color.Black, Color.DarkGray }
        //};
        //ai generated colours

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

        public static int currentStage { get; private set; } = 0; //can be read by ui class

        private static void rotationAnim(object sender, EventArgs e)
        {
            rotationStepProgress++;
            double pct = (double)rotationStepProgress / (double)rotationSteps;
            //lerpDir(pct); failed experiment
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

        ////"pre rotate vectors"; required to interpolate between them
        //private static double[] prv = { dirX, dirY, planeX, planeY }; //start with default values
        //private static void lerpDir(double t) {
        //    var dir = rotationDir ? 1.0 : -1.0;
        //    //dirX, dirY, planeX, planeY
        //    double[] rotated = { -prv[1] * dir, prv[0] * dir, -prv[3] * dir, prv[2] * dir};

        //    dirX = (1 - t) * prv[0] + t * rotated[0];
        //    dirY = (1 - t) * prv[1] + t * rotated[1];
        //    planeX = (1 - t) * prv[2] + t * rotated[2];
        //    planeY = (1 - t) * prv[3] + t * rotated[3];

        //    //var interpX = (1 - t) * originalX + t * rotatedX;
        //    //var interpY = (1 - t) * originalY + t * rotatedY;
        //}

        public static void rotate(bool dir) {
            //setAngle(dir);
            //return;
            if (!rotationTimer.Enabled)
            {
                //prv[0] = dirX;
                //prv[1] = dirY;
                //prv[2] = planeX;
                //prv[3] = planeY;
                rotationDir = dir;
                rotationTimer.Start();
            }
        }

        public static void move(bool dir) {
            if (rotationTimer.Enabled) //dont move if rotating
            {
                return;
            }
            var intDir = dir ? 1 : -1;
            try
            {
                if (worldMap[Convert.ToInt32(posX-.5 + (intDir * dirX)), Convert.ToInt32(posY-.5)] <= 0) //values below zero will have no collision
                {
                    posX += (intDir * dirX);
                }
                if (worldMap[Convert.ToInt32(posX-.5), Convert.ToInt32(posY-.5 + (intDir * dirY))] <= 0)
                {
                    posY += (intDir * dirY);
                }
            }
            catch { Debug.WriteLine("index out of bounds in move()"); } 
            if (worldMap[(int)posX, (int)posY] == -1)
            {
                posX = spawnX; posY = spawnY;
                currentStage++;
            }
        }

        

        public static void drawGame(Graphics g) {
            oldTime = time;
            time = Environment.TickCount;
            //renderering code in this for loop taken from https://lodev.org/cgtutor/raycasting.html
            var currentStageColors = stageColors[currentStage % stageColors.Length];
            g.FillRectangle(new SolidBrush(currentStageColors[0]), new Rectangle(0, 0, width, height)); //draw background first

            //draw lines
            for (int x = 0; x < width; x++) {
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

                if (rayDirX < 0) {
                    stepX = -1;
                    sideDistX = (posX - mapX) * deltaDistX;
                } else {
                    stepX = 1;
                    sideDistX = (mapX + 1.0 - posX) * deltaDistX;
                }
                if (rayDirY < 0) {
                    stepY = -1;
                    sideDistY = (posY - mapY) * deltaDistY;
                } else {
                    stepY = 1;
                    sideDistY = (mapY + 1.0 - posY) * deltaDistY;
                }

                while (hit == 0) {
                    if (sideDistX < sideDistY) {
                        sideDistX += deltaDistX;
                        mapX += stepX;
                        side = 0;
                    } else {
                        sideDistY += deltaDistY;
                        mapY += stepY;
                        side = 1;
                    }
                    try
                    {
                        if (worldMap[mapX, mapY] > 0) hit = 1;
                    } catch { Debug.WriteLine("index out of bounds in while(hit == 0)"); hit = 1; }
                }

                if (side == 0) perpWallDist = (sideDistX - deltaDistX);
                else perpWallDist = (sideDistY - deltaDistY);

                int lineHeight = (int)(height / perpWallDist);

                int drawStart = -lineHeight / 2 + height / 2;
                if (drawStart < 0) drawStart = 0;
                int drawEnd = lineHeight / 2 + height / 2;
                if (drawEnd >= height) drawEnd = height - 1;

                Color color = currentStageColors[worldMap[mapX, mapY]];
                //switch (worldMap[mapX,mapY]) {
                //    case 1: color = currentStageColors[1]; break; 
                //    case 2: color = currentStageColors[1]; break; 
                //    case 3: color = currentStageColors[1]; break; 
                //    case 4: color = Color.White; break; 
                //    default: color = Color.Yellow; break; 
                //}

                //give x and y sides different brightness
                if (side == 1) { color = Color.FromArgb(color.R / 2, color.G / 2, color.B / 2); }

                //draw the pixels of the stripe as a vertical line
                var pen = new Pen(color);
                g.DrawLine(pen, new Point(x, drawStart), new Point(x, drawEnd));
            }
            //done for loop

        }
    }
}
