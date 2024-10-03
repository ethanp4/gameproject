namespace gameproject {
    public partial class Form1 : Form {
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        const int framerate = 60;
        Game game;
        UI ui;
        public Form1() {
            InitializeComponent();
            DoubleBuffered = true;
            Width = 800;
            Height = 600;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            ui = new UI();
            game = new Game();

            timer.Interval = (int)Math.Floor(1f / (float)framerate * 1000f); // frametime for 60 fps
            timer.Tick += invalidateTimer;
            timer.Start();
        }

        private void invalidateTimer(object sender, EventArgs e) {
            Invalidate(); //repaint once every 16 ms for 60 fps
        }

        protected override void OnPaint(PaintEventArgs e) {
            var g = e.Graphics; // graphics object to draw with
            game.drawGame(g);
            ui.drawUI(g);
            //base.OnPaint(e); idk if this is needed
        }

        private void formMouseMove(object sender, MouseEventArgs e) {
            ui.mPos = e.Location;
        }
    }

    public class UI() {
        public Point mPos;
        private Font font = new Font("Times New Roman", 16);
        public void drawUI(Graphics g) {
            g.DrawString("Mouse position: " + mPos.ToString(), font, Brushes.Black, new Point(0,0));
        }
    }


    public class Game() {
        public void drawGame(Graphics g) {
            g.DrawEllipse(Pens.Aqua, new Rectangle(100, 100, 100, 100));

        }
    }
}
