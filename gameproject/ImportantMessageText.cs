namespace gameproject
{
    public static class ImportantMessageText //as opposed to action log, this is a single message in the center of the screen to indicate something important to the player
    {
        public static string currentMessage = "";
        public static DateTime startTime;
        public static float messageLength; //in seconds
        public static DateTime endTime { get { return startTime.AddSeconds(messageLength); } }
        public static Font biggerFont = new Font(GameForm.font.FontFamily, GameForm.font.Size * 2);
        public static void setMessage(string message, float length) //message will be overwritten if called again
        {
            currentMessage = message;
            messageLength = length;
            startTime = DateTime.Now;
        }

        public static void updateImportantMessageText(Graphics g)
        {
            if (DateTime.Now > endTime) { return; }
            if (currentMessage == "") { return; }
            g.DrawString(currentMessage, biggerFont, Brushes.Beige, new Point(GameForm.windowWidth / 2, GameForm.windowHeight / 2));
        }
    }
}
