using System.Diagnostics;

namespace gameproject.Enemies
{
    public abstract class BaseEnemy
    {
        private static Random random = new Random();
        public string name;
        public Image sprite;
        public int maxHealth;
        public int health;
        public int attack;
        //public int defense;
        //public int speed;
        public int level;

        public BaseEnemy(int level, Image sprite, float maxHpMultiplier = 1, float attackMultiplier = 1, float defenseMultiplier = 1, float speedMultiplier = 1) {
            var statMultiplier = level * 1.5;
            maxHealth = (int)(7 * statMultiplier * maxHpMultiplier);
            health = maxHealth;
            attack = (int)(1 * statMultiplier * attackMultiplier);
            //defense = (int)(1 * statMultiplier * defenseMultiplier);
            //speed = (int)(1 * statMultiplier * speedMultiplier);
            this.level = level;
            this.sprite = sprite;
        }
        public int calculateRewardXp() { 
            var rand = random.NextDouble();
            rand = Math.Clamp(rand, 0.8 , 1.2)+1;
            var randomness = rand * level;
            var rewardXp = (int)Math.Pow(level + randomness, 1.3);
            var req = Player.reqXpForNextLevel();
            Debug.WriteLine($"This enemy gave {(double)rewardXp / (double)req * 100.0}% of the required xp for the next level");
            //Debug.WriteLine($"LEVEL: Level was {level}, randomness was {randomness} reward xp was {rewardXp}");
            //Debug.WriteLine($"The required xp for this level is {Player.reqXpForNextLevel()}");
            return rewardXp;
        }

        public int calculateRewardCanadianDollars() {
            var rand = random.NextDouble() + 1;
            var randomness = rand * Math.Min(1 ,level * 0.4);
            var rewardCad = (int)Math.Pow(level + randomness, 1.5);
            Debug.WriteLine($"GOLD: Level was {level}, randomness was {randomness} reward cad was {rewardCad}");
            return rewardCad;
        }

        public override string ToString() {
            return $"Level {level} {name}";
        }
    }
}