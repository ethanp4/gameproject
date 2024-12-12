using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;
using gameproject.Enemies;

namespace gameproject
{
    public class BattleHandler {
        public static BattleHandler instance { get; private set; }

        public BaseEnemy enemy;

        public bool playerTurn = true;
        private bool playerDefending = false;
        public static Random random = new();
        enum END_STATE { WIN, LOSS, RAN_AWAY };
        public static void initBattle() //using this function as opposed to the constructor to make it the setting of other game states more explicit maybe
        {                               //only one battlehandler and one battleui will exist at a time
            instance = new();
            var enemy = new Snowman(Player.calculateLevel()); //these options will be randomly rolled from within here in the future
            instance.enemy = enemy;
            BattleUI.instance = new();
            ActionLog.appendAction($"Battle started with {enemy}", ActionLog.COLORS.SYSTEM);
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
            ActionLog.appendAction($"Player chose {choiceString}", ActionLog.COLORS.PLAYER);
            switch (choiceString) {
                case "Attack":
                    playerAttack();
                    if (enemy.health <= 0) {
                        endBattle(END_STATE.WIN);
                        return;
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
                        ActionLog.appendAction("Failed to run away...", ActionLog.COLORS.SYSTEM);
                        enemyTurn();
                    }
                    break;
            }
        }
        private void playerAttack() {
            playerTurn = false;
            var dmg = Player.getAttack();
            AnimationPlayer.addAnimation(attackAnim);
            playerTurn = false;
            var crit = random.NextDouble() < Player.getCritRate();
            if (crit) { dmg *= 2; ActionLog.appendAction("Critical hit!", ActionLog.COLORS.SPECIAL); }
            ActionLog.appendAction($"Player dealt {dmg} damage", ActionLog.COLORS.PLAYER);
            enemy.health -= dmg;
            ActionLog.appendAction($"Enemy health: {enemy.health}", ActionLog.COLORS.ENEMY);
        }

        private void enemyTurn() //enemy attacks
        {
            double dmg = enemy.attack;
            var crit = random.NextDouble() < 0.1;
            if (crit) { dmg *= 2; ActionLog.appendAction("Critical hit!", ActionLog.COLORS.SPECIAL); }
            if (playerDefending) { dmg *= 0.5; ActionLog.appendAction("Player defended!", ActionLog.COLORS.PLAYER); }
            ActionLog.appendAction($"Enemy dealt {dmg} damage", ActionLog.COLORS.ENEMY);
            Player.health -= enemy.attack * 2;
            ActionLog.appendAction($"Player health: {Player.health}", ActionLog.COLORS.SYSTEM);
            if (Player.health <= 0) {
                endBattle(END_STATE.LOSS);
            }
            playerTurn = true;
        }
        
        private void endBattle(END_STATE result) {
            switch (result) {
                case END_STATE.WIN: //win state
                    var xp = enemy.calculateRewardXp();
                    Player.addXp(xp);
                    var profit = enemy.calculateRewardCanadianDollars();
                    Player.canadianDollars += profit;
                    ActionLog.appendAction("Enemy defeated!", ActionLog.COLORS.SYSTEM);
                    ActionLog.appendAction($"Player received {xp} xp and {profit} canadian dollars!", ActionLog.COLORS.SPECIAL);
                    Game.gameState = Game.STATE.FREE_MOVEMENT;
                    break;
                case END_STATE.LOSS: //loss state
                    ActionLog.appendAction("Player defeated!", ActionLog.COLORS.SYSTEM);
                    Game.gameState = Game.STATE.GAME_OVER;
                    ImportantMessageText.setMessage("Game Over isnt programmed yet", 99999);
                    break;
                case END_STATE.RAN_AWAY: //ran away
                    ActionLog.appendAction("Player ran away!", ActionLog.COLORS.SYSTEM);
                    Game.gameState = Game.STATE.FREE_MOVEMENT;
                    break;
            }
        }
    }
}