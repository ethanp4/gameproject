namespace gameproject.Enemies
{
    public abstract class BaseEnemy
    {
        public string name;
        public Image sprite;
        public int maxHealth;
        public int health;
        public int attack;
        public int defense;
        public int speed;
        public int level;
        public BaseEnemy(int targetLevel, Image sprite) {
            var statMultiplier = targetLevel * 0.6;
            maxHealth = (int)(20 * statMultiplier);
            health = maxHealth;
            attack = (int)(5 * statMultiplier);
            defense = (int)(5 * statMultiplier);
            speed = (int)(5 * statMultiplier);
            level = targetLevel;
            this.sprite = sprite;
        }
    }
}