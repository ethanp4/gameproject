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
            AutoScaleMode = AutoScaleMode.Dpi; //no more blurry sprites
            DoubleBuffered = true; //no flickering
            Width = windowWidth;
            Height = windowHeight;
            FormBorderStyle = FormBorderStyle.FixedSingle; //no maximise button
            MaximizeBox = false;

            timer.Interval = (int)Math.Floor(1f / (float)framerate * 1000f); // frametime for 60 fps
            timer.Tick += invalidateTimer; //on Invalidate(), OnPaint is called automatically, which redraws the frame and handles game state
            timer.Start();

            BattleHandler.initThemes(); //battle handler is handling bgm since there are only two states 
            //MouseClick += (sender, e) => { ActionLog.appendAction($"Click at {mPos.X}, {mPos.Y}", ActionLog.COLORS.SYSTEM); };
        }

        private void invalidateTimer(object sender, EventArgs e) {
            Invalidate(); //repaint once every 16 ms for 60 fps
        }

        //main game loop
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
                    break;
                case Game.STATE.GAME_OVER: //nothing to draw since ImportantMessageText is used elsewhere (line 243 game.cs and line 110 battlehandler.cs)
                    break;
                case Game.STATE.YOU_WIN:
                    break;
                case Game.STATE.IN_SHOP:
                    Shop.instance.drawShop(g); //handle everything related to the shop
                    break;
            }
            UI.drawPlayerBars(g);
            //UI.drawMinimap(g, windowWidth, windowHeight);
            ActionLog.drawLog(g);
            ImportantMessageText.updateImportantMessageText(g);
            AnimationPlayer.updateAnimations(DateTime.Now, g);
        }

        //mouse pointer location just or debugging
        private void formMouseMove(object sender, MouseEventArgs e) {
            mPos = e.Location;
        }

        //keyboard input handler, this game is keyboard only
        //depending on the game state, input is send to different parts of the program
        private void formKeyDown(object sender, KeyEventArgs e) { 
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
                case Game.STATE.IN_SHOP:
                    Shop.instance.handleInput(e);
                    break;
                case Game.STATE.GAME_OVER:
                case Game.STATE.YOU_WIN:
                    if (e.KeyCode == Keys.Enter) {
                        ImportantMessageText.clearMessage();
                        Game.restart();
                    }
                    break;
            }
        }
    }
}
