

namespace gameproject {
    internal class Shop {
        public static Shop instance;
        internal void drawShop(Graphics g) {
            var textPoint = new Point(200, 160);
            g.FillRectangle(Brushes.Black, new Rectangle(textPoint.X, textPoint.Y, 280, 38));
            g.DrawString("Welcome to the shop", GameForm.font, Brushes.Gold, textPoint);
            var shopMenuBorder = new Rectangle(100, 200, 500, 650);
            g.DrawRectangle(Pens.Black, shopMenuBorder);
        }

        internal void handleInput(KeyEventArgs e) {
            //throw new NotImplementedException();
        }
    }
}