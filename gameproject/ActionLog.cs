namespace gameproject
{
    public static class ActionLog
    {
        //public static List<string> log = new(){ "Example 1", "Example 2", "Example 3" };
        public static List<KeyValuePair<string, COLORS>> log = new()
        {
        };

        // Add a new action to the log with the specified color
        public static void appendAction(string action, COLORS color)
        {
            log.Insert(0, new(action, color));
            if (log.Count > 21)
            {
                log.RemoveAt(21);
            }
        }

        // Enum for log message colors
        public enum COLORS { ENEMY, PLAYER, SPECIAL, SYSTEM, WARNING } // Added WARNING color

        private static List<Color> enumColors = new() { Color.Red, Color.Blue, Color.Gold, Color.White, Color.Orange }; // Added Orange for WARNING

        // Draw the log on the screen
        public static void drawLog(Graphics g)
        {
            var logWindow = new Rectangle(GameForm.windowWidth-560, 0, 600, 400);
            g.DrawRectangle(Pens.White, logWindow);
            for (int i = 0; i < log.Count; i++)
            {
                g.DrawString($"{log[i].Key}", GameForm.font, new SolidBrush(enumColors[(int)log[i].Value]), new Point(logWindow.X, 370 - i * 20));
            }
        }
    }
}
