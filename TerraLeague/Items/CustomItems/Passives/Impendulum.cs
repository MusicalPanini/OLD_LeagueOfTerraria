using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class Impendulum : Passive
    {
        int lifePerTime;
        double magicMinionDamage;
        
        public Impendulum(int LifePerTime, double magicMinionDamagePercentPer)
        {
            lifePerTime = LifePerTime;
            magicMinionDamage = magicMinionDamagePercentPer;
        }

        public static int[] stat = new int[]
        {
            2,1,0,1,2,3,4,5,6,5,4,3,2,1,0,1,2,3,4,5,6,5,4,3,2
        };
        public static int GetHour { get { return (int)((1800 + (Main.time + (Main.dayTime ? 0 : 54000))) / 3600); } }
        public static int GetStat { get { return stat[GetHour]; } }

        public override string Tooltip(Player player, ModItem modItem)
        {
            return TooltipName("IMPENDULUM") + TerraLeague.CreateColorString(PassiveSecondaryColor, "Gain life, mana, magic and minion damage based on the time of day\n" + GetStat * lifePerTime + " life and mana, +" + (int)(GetStat * magicMinionDamage) + "% magic and minion damage");
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            passiveStat = GetStat;
            player.statLifeMax2 += lifePerTime * GetStat;
            player.statManaMax2 += lifePerTime * GetStat;
            player.magicDamage += (float)(magicMinionDamage * 0.01f * GetStat);
            modPlayer.TrueMinionDamage += magicMinionDamage * 0.01f * GetStat;

            base.UpdateAccessory(player, modItem);
        }
    }
}
