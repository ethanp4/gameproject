namespace gameproject
{
    public class BattleHandler
    {
        public static BattleHandler instance { get; private set; }
        public double playerHealth = 100.0;
        public double enemyHealth = 100.0;
        public double playerDamage = 10.0;
        public double enemyDamage = 10.0;
        public bool playerTurn = true;
        
        public static void initBattle()
        {
            instance = new();
            Game.gameState = Game.STATE.BATTLE;
        }

        public void playerChoice() //player chooses what to do, that is passed in as a parameter, then the enemy attacks
        {

        }
    }
}