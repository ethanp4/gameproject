using System.Diagnostics;

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

        public static void initBattle() //using this function as opposed to the constructor to make it the setting of other game states more explicit maybe
        {                               //only one battlehandler and one battleui will exist at a time
            instance = new();
            BattleUI.instance = new(0); //these options will be randomly rolled from within here in the future
            ActionLog.appendAction($"Battle started with enemy {0}");
            Game.gameState = Game.STATE.BATTLE;
        }

        private static Image[] attackImages = {
            Bitmap.FromStream(new MemoryStream(Properties.Resources.Attack_1)),
            Bitmap.FromStream(new MemoryStream(Properties.Resources.Attack_2)),
            Bitmap.FromStream(new MemoryStream(Properties.Resources.Attack_3)),
            Bitmap.FromStream(new MemoryStream(Properties.Resources.Attack_4)),
        };
        public void playerChoice(string choiceString) //player chooses what to do, that is passed in as a parameter, then the enemy attacks
        {
            var attack = new AnimatedSprite(new Point(500, 300), attackImages, 100, 300);
            AnimationPlayer.addAnimation(attack);
            ActionLog.appendAction($"Player chose {choiceString}");
            Debug.WriteLine($"Player chose {choiceString}");
        }
    }
}