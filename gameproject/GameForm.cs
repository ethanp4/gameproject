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
            AutoScaleMode = AutoScaleMode.Dpi;
            DoubleBuffered = true;
            Width = windowWidth;
            Height = windowHeight;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            timer.Interval = (int)Math.Floor(1f / (float)framerate * 1000f); // frametime for 60 fps
            timer.Tick += invalidateTimer;
            timer.Start();

            MouseClick += (sender, e) => { ActionLog.appendAction($"Click at {mPos.X}, {mPos.Y}"); };
        }

        private void invalidateTimer(object sender, EventArgs e) {
            Invalidate(); //repaint once every 16 ms for 60 fps
        }

        protected override void OnPaint(PaintEventArgs e) {
            var g = e.Graphics; // graphics object to draw with
            
            Game.drawGame(g); // Main game rendering
            switch (Game.gameState)
            {
                case Game.STATE.FREE_MOVEMENT:
                    UI.drawUI(g);
                    break;
                case Game.STATE.BATTLE:
                    BattleUI.instance.drawUI(g);
                    BattleUI.instance.drawUI(g);
                    break;
                case Game.STATE.GAME_OVER:
                    //nothing is programmed yet
                    break;
            }
            UI.drawPlayerBars(g);
            UI.drawMinimap(g, windowWidth, windowHeight);
            ActionLog.drawLog(g);
            ImportantMessageText.updateImportantMessageText(g);
            AnimationPlayer.updateAnimations(DateTime.Now, g);
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
