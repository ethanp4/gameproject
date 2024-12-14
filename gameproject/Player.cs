using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace gameproject
{
    public static class Player
    {
        const int baseHealth = 25;
        const int baseAttack = 2;
        const double baseCritRate = 0.01;
        const double critRateLevelMultiplier = 1.1;
        const double healthLevelMultiplier = 1.2;
        const double attackLevelMultiplier = 1.2;
        const double mpLevelMultiplier = 1.2;
        public static int attack = baseAttack;
        public static int xp = 2;
        public static int canadianDollars = 0;
        public static int baseMaxMP = 20;

        public static int health = getMaxHealth(); // Start with max HP
        public static int maxMP { get { return getMaxMP(); } } //return the function
        public static int MP = maxMP;

        public static void reset() {
            xp = 2;
            canadianDollars = 0;
            health = getMaxHealth();
            MP = getMaxMP();
        }

        public static int getMaxHealth()
        {
            return (int)(baseHealth * calculateLevel() * healthLevelMultiplier);
        }

        public static int getAttack()
        {
            return (int)(baseAttack * calculateLevel() * attackLevelMultiplier);
        }

        public static double getCritRate()
        {
            return (baseCritRate * calculateLevel() * critRateLevelMultiplier);
        }

        public static int getMaxMP() {
            return (int)(baseMaxMP * calculateLevel() * mpLevelMultiplier);
        }

        public static void addXp(int xp) {
            var prevLevel = calculateLevel();
            Player.xp += xp;
            if (calculateLevel() != prevLevel)
            {
                ActionLog.appendAction($"Player is now level {calculateLevel()}!", ActionLog.COLORS.SPECIAL);
                var healthDiff = getMaxHealth() - health;
                health += (int)(healthDiff * 0.50); //gain 50% of missing health
                var mpDiff = getMaxMP() - MP;
                MP += (int)(mpDiff * 0.50);
            }
        }

        public static int calculateLevel()
        {
            return (int)Math.Sqrt(xp / 2); // lv 1 - 2 xp, lv 2 - 8 xp, lv 3 - 18 xp, lv 4 - 32 xp
        }

        public static int reqXpForNextLevel()
        {
            return (int)Math.Pow(calculateLevel() + 1, 2) * 2;
        }


    }
}
