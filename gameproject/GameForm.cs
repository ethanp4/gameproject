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

        public static Font font = new Font("Comic Sans MS", 16);

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
            if (Game.gameState == Game.STATE.BATTLE)
            {
                BattleUI.instance.drawUI(g);
            } else
            {
                UI.drawUI(g);
            }
        }

        private void formMouseMove(object sender, MouseEventArgs e) {
            mPos = e.Location; //this needs to be set from here in order to get the local position
        }

        private void formKeyDown(object sender, KeyEventArgs e) { //keyboard input handler, this game is keyboard only
            switch (Game.gameState)
            {
                case Game.STATE.FREE_MOVEMENT:
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
                    break;
                case Game.STATE.BATTLE:
                    BattleUI.instance.handleInput(e);
                    break;
            }
        }
    }
}
