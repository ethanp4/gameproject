namespace gameproject.Enemies {
    internal class Snowman : BaseEnemy {
        static Image sprite = Image.FromFile(@"..\..\..\Resources\snowman.jpeg");
        public Snowman(int targetLevel) : base(targetLevel, sprite) {
            name = $"Level {targetLevel} Snowman";
        }
    }
}
