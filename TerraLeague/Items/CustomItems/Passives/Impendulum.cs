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

        int[] stat = new int[]
        {
            4,5,6,7,8,9,10,11,12,11,10,9,8,7,6,5,4,3,2,1,0,1,2,3
        };
        int stat2 = 0;

        public override string Tooltip(Player player, ModItem modItem)
        {
            return "[c/0099cc:Passive: IMPENDULUM -] [c/99e6ff:Gain life, mana, magic and minion damage based on the time of day]" +
                "\n[c/99e6ff:+" + stat[(int)((1800 + Main.time) / 3600)] * lifePerTime + " life and mana, +" + (int)(stat[(int)((1800 + Main.time) / 3600)] * magicMinionDamage) + "% magic and minion damage]";
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            stat2 = (int)((1800 + Main.time) / 3600);
            modPlayer.accessoryStat[TerraLeague.FindAccessorySlotOnPlayer(player, modItem)] = stat[stat2];
            player.statLifeMax2 += lifePerTime * stat[stat2];
            player.statManaMax2 += lifePerTime * stat[stat2];
            player.magicDamage += (float)(magicMinionDamage * 0.01f * stat[stat2]);
            modPlayer.TrueMinionDamage += magicMinionDamage * 0.01f * stat[stat2];

            base.UpdateAccessory(player, modItem);
        }
    }
}
