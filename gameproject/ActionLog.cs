namespace gameproject
{
    public static class ActionLog
    {
        //public static List<string> log = new(){ "Example 1", "Example 2", "Example 3" };
        public static List<KeyValuePair<string, DateTime>> log = new() {
            new("Example 1", DateTime.Now-TimeSpan.FromSeconds(2)),
            new("Example 2", DateTime.Now-TimeSpan.FromSeconds(1)),
            new("Example 3", DateTime.Now) 
        };
        public static void appendAction(string action)
        {
            log.Insert(0, new(action, DateTime.Now));
            if (log.Count > 21)
            {
                log.RemoveAt(21);
            }
        }
        public static void drawLog(Graphics g)
        {
            var logWindow = new Rectangle(GameForm.windowWidth-417, 0, 400, 400);
            g.DrawRectangle(Pens.White, logWindow);
            for (int i = 0; i < log.Count; i++)
            {
                g.DrawString($"{log[i].Value:HH:mm:ss} - {log[i].Key}", GameForm.font, Brushes.White, new Point(logWindow.X, 370 - i * 20));
            }
        }

    }
}