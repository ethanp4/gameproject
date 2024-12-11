using System.Runtime.CompilerServices;
using gameproject.Enemies;

namespace gameproject
{
    public class BattleHandler {
        public static BattleHandler instance { get; private set; }
        public double playerHealth = 100.0;
        public double enemyHealth = 100.0;
        public double playerCritRate = 0.05;
        public double enemyCritRate = 0.05;
        public double playerDamage = 10.0;
        public double enemyDamage = 5.0;

        public BaseEnemy enemy;

        public bool playerTurn = true;
        private bool playerDefending = false;
        public static Random random = new();
        enum END_STATE { WIN, LOSS, RAN_AWAY };
        public static void initBattle() //using this function as opposed to the constructor to make it the setting of other game states more explicit maybe
        {                               //only one battlehandler and one battleui will exist at a time
            instance = new();
            var enemy = new Snowman(1);
            instance.enemy = enemy;
            BattleUI.instance = new(); //these options will be randomly rolled from within here in the future
            ActionLog.appendAction($"Battle started with enemy {enemy}");
            Game.gameState = Game.STATE.BATTLE;
        }

        private static Image[] attackImages = {
            Bitmap.FromStream(new MemoryStream(Properties.Resources.Attack_1)),
            Bitmap.FromStream(new MemoryStream(Properties.Resources.Attack_2)),
            Bitmap.FromStream(new MemoryStream(Properties.Resources.Attack_3)),
            Bitmap.FromStream(new MemoryStream(Properties.Resources.Attack_4)),
        };
        private static AnimatedSprite attackAnim = new AnimatedSprite(new Point(500, 300), attackImages, 100, 300);
        public void playerChoice(string choiceString) //player chooses what to do, that is passed in as a parameter, then the enemy attacks
        {
            if (!playerTurn) return;
            ActionLog.appendAction($"Player chose {choiceString}");
            switch (choiceString) {
                case "Attack":
                    playerAttack();
                    if (enemyHealth <= 0) {
                        endBattle(END_STATE.WIN);
                    }
                    enemyTurn();
                    break;
                case "Defend":
                    playerDefending = true;

                    enemyTurn();
                    break;
                case "Run":
                    if (random.NextDouble() < 0.5) {
                        endBattle(END_STATE.RAN_AWAY);
                    } else {
                        ActionLog.appendAction("Failed to run away...");
                        enemyTurn();
                    }
                    break;
            }
        }
        private void playerAttack() {
            playerTurn = false;
            var dmg = playerDamage;
            AnimationPlayer.addAnimation(attackAnim);
            playerTurn = false;
            var crit = random.NextDouble() < playerCritRate;
            if (crit) { dmg *= 2; ActionLog.appendAction("Critical hit!"); }
            ActionLog.appendAction($"Player dealt {dmg} damage");
            enemyHealth -= playerDamage * 2;
            ActionLog.appendAction($"Enemy health: {enemyHealth}");
        }
        private void enemyTurn() //enemy attacks
        {
            var dmg = enemyDamage;
            var crit = random.NextDouble() < enemyCritRate;
            if (crit) { dmg *= 2; ActionLog.appendAction("Critical hit!"); }
            if (playerDefending) { dmg *= 0.5; ActionLog.appendAction("Player defended!"); }
            ActionLog.appendAction($"Enemy dealt {dmg} damage");
            playerHealth -= enemyDamage * 2;
            ActionLog.appendAction($"Player health: {playerHealth}");
            if (playerHealth <= 0) {
                endBattle(END_STATE.LOSS);
            }
            playerTurn = true;
        }

        private void endBattle(END_STATE result) {
            switch (result) {
                case END_STATE.WIN: //win state
                    ActionLog.appendAction("Enemy defeated!");
                    Game.gameState = Game.STATE.FREE_MOVEMENT;
                    break;
                case END_STATE.LOSS: //loss state
                    ActionLog.appendAction("Player defeated!");
                    Game.gameState = Game.STATE.GAME_OVER;
                    ImportantMessageText.setMessage("Game Over isnt programmed yet", 99999);
                    break;
                case END_STATE.RAN_AWAY: //ran away
                    ActionLog.appendAction("Player ran away!");
                    Game.gameState = Game.STATE.FREE_MOVEMENT;
                    break;
            }
        }
    }
}