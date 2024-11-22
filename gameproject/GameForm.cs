using System.Drawing;
using System.Security.Principal;

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
            g.DrawString(info, font, Brushes.Black, new Point(0,0));


        }
    }

    public static class Game {
        const int width = GameForm.windowWidth;
        const int height = GameForm.windowHeight;

        //const int mapWidth = 24;
        //const int mapHeight = 24;
        //static int[,] worldMap = {
        //  {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        //  {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
        //  {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
        //  {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
        //  {1,0,0,0,0,0,2,2,2,2,2,0,0,0,0,3,0,3,0,3,0,0,0,1},
        //  {1,0,0,0,0,0,2,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,1},
        //  {1,0,0,0,0,0,2,0,0,0,2,0,0,0,0,3,0,0,0,3,0,0,0,1},
        //  {1,0,0,0,0,0,2,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,1},
        //  {1,0,0,0,0,0,2,2,0,2,2,0,0,0,0,3,0,3,0,3,0,0,0,1},
        //  {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
        //  {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
        //  {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
        //  {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
        //  {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
        //  {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
        //  {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
        //  {1,4,4,4,4,4,4,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
        //  {1,4,0,4,0,0,0,0,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
        //  {1,4,0,0,0,0,5,0,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
        //  {1,4,0,4,0,0,0,0,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
        //  {1,4,0,4,4,4,4,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
        //  {1,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
        //  {1,4,4,4,4,4,4,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
        //  {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}
        //};

        //const int mapWidth = 8;
        //const int mapHeight = 6;
        public static double posX = 6.5, posY = 1.5;
        static int[,] worldMap = {
            { 1,1,1,1,1,1,1,1,1,1,1,1,1 },
            { 1,1,1,1,0,0,0,0,1,1,1,1,1 },
            { 1,1,1,0,0,0,0,0,0,1,1,1,1 },
            { 1,1,0,0,0,0,0,0,0,0,1,1,1 },
            { 1,2,0,0,0,0,0,0,0,0,2,1,1 },
            { 3,0,0,0,0,0,0,0,0,0,0,0,1 },
            { 3,0,0,0,0,0,0,0,0,0,0,0,1 },
            { 3,0,0,0,0,0,0,0,0,0,0,0,1 },
            { 1,2,0,0,0,0,0,0,0,0,2,1,1 },
            { 1,1,0,0,0,0,0,0,0,0,1,1,1 },
            { 1,1,1,0,0,0,0,0,0,1,1,1,1 },
            { 1,1,1,1,0,0,0,0,1,1,1,1,1 },
            { 1,1,1,1,1,1,1,1,1,1,1,1,1 },
        };
        
        //public static double posX = 21.5, posY = 11.5;  //x and y start position, only ever modified by +/- 1
        public static double dirX = 0, dirY = 1; //initial direction vector
        public static double planeX = 0.66, planeY = 0; //the 2d raycaster version of camera plane

        static double time = 0; //time of current frame
        static double oldTime = 0; //time of previous frame
        static double frameTime {  get { return (time - oldTime) / 1000f; } }

        private static System.Windows.Forms.Timer rotationTimer = new();
        static Game()
        {
            rotationTimer.Interval = 16;
            rotationTimer.Tick += rotationAnim;
        }

        private static double rotationAnimLength = 0.5;
        private static bool rotationDir = false;
        private static int rotationSteps = 20; //0.5 seconds at 60 fps
        private static int rotationStepProgress = 0;
        private static void rotationAnim(object sender, EventArgs e)
        {
            rotationStepProgress++;
            if (rotationStepProgress == rotationSteps)
            {
                rotationStepProgress = 0;
                rotationTimer.Stop();
                //setAngle(rotationDir);
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

        private static void setAngle(bool which) {
            var dir = which ? 1 : -1;

            var temp = dirX;
            dirX = -dirY * dir;
            dirY = temp * dir;

            temp = planeX;
            planeX = -planeY * dir;
            planeY = temp * dir;
            //if (dirY >= 0) {
            //    dirX = dirY;
            //} else {
            //    dirX = -dirY;
            //}
            //if (dirX <= 0) {
            //    dirY = -temp;
            //} else {
            //    dirY = temp;
            //}
        }
        //    var rotations = new Dictionary<int, int[]>() {
        //       { 0, new[]{-1, 1 } },
        //       { 1, new[]{1, -1 } }
        //    };
        //    int[] r = rotations[idx];

        //    //double oldDirX = dirX;
        //    //double oldPlaneX = planeX;

        //double odirX = -1, odirY = 0; //initial direction vector
        //double oplaneX = 0, oplaneY = 0.66; //the 2d raycaster version of camera plane
        //dirX = r[0];
        //    dirY = r[1];
        //    planeX = r[0];
        //    planeY = r[1];
        //}

        public static void rotate(bool dir) {
            setAngle(dir);
            return;
            if (!rotationTimer.Enabled)
            {
                rotationDir = dir;
                rotationTimer.Start();
            }
            //double oldDirX = dirX;
            //var intDir = dir ? 1.0 : -1.0;
            //intDir *= 1.5708; //90 degrees in radians
            //dirX = dirX * Math.Cos(intDir) - dirY * Math.Sin(intDir);
            //dirY = oldDirX * Math.Sin(intDir) + dirY * Math.Cos(intDir);
            //double oldPlaneX = planeX;
            //planeX = planeX * Math.Cos(intDir) - planeY * Math.Sin(intDir);
            //planeY = oldPlaneX * Math.Sin(intDir) + planeY * Math.Cos(intDir);
        }

        public static void move(bool dir) {
            if (rotationTimer.Enabled) //dont move if rotating
            {
                return;
            }
            var intDir = dir ? 1 : -1;
            try
            {
                if (worldMap[Convert.ToInt32(posX + (intDir * dirX)), Convert.ToInt32(posY)] == 0)
                {
                    posX += (intDir * dirX);
                }
                if (worldMap[Convert.ToInt32(posX), Convert.ToInt32(posY + (intDir * dirY))] == 0)
                {
                    posY += (intDir * dirY);
                }
            }
            catch { } //sometimes this can index out of bounds

            //double moveSpeed = frameTime * 5.0;
            //if (worldMap[Convert.ToInt32(posX + (intDir * dirX) * moveSpeed), Convert.ToInt32(posY)] == 0) {
            //    posX += (intDir * dirX) * moveSpeed;
            //}
            //if (worldMap[Convert.ToInt32(posX), Convert.ToInt32(posY + (intDir * dirY) * moveSpeed)] == 0) {
            //    posY += (intDir * dirY) * moveSpeed;
            //}
        }

        public static void drawGame(Graphics g) {
            oldTime = time;
            time = Environment.TickCount;
            //renderering code in this for loop taken from https://lodev.org/cgtutor/raycasting.html
            g.FillRectangle(Brushes.Black, new Rectangle(0, 0, width, height)); //draw background black first
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

                    if (worldMap[mapX, mapY] > 0) hit = 1;
                }

                if (side == 0) perpWallDist = (sideDistX - deltaDistX);
                else perpWallDist = (sideDistY - deltaDistY);

                int lineHeight = (int)(height / perpWallDist);

                int drawStart = -lineHeight / 2 + height / 2;
                if (drawStart < 0) drawStart = 0;
                int drawEnd = lineHeight / 2 + height / 2;
                if (drawEnd >= height) drawEnd = height - 1;

                Color color;
                switch (worldMap[mapX,mapY]) {
                    case 1: color = Color.Red; break; 
                    case 2: color = Color.Green; break; 
                    case 3: color = Color.Blue; break; 
                    case 4: color = Color.White; break; 
                    default: color = Color.Yellow; break; 
                }

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
