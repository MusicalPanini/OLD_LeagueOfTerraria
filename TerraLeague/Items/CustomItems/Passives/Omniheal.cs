using Terraria;
using Terraria.ModLoader;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class Omniheal : Passive
    {
        int percentLifesteal;

        public Omniheal(int PercentLifesteal)
        {
            percentLifesteal = PercentLifesteal;
        }


        public override string Tooltip(Player player, ModItem modItem)
        {
            return TooltipName("OMNIHEAL") + TerraLeague.CreateColorString(PassiveSecondaryColor, "Gain +" + percentLifesteal + " life steal"/* +
                "\n" + percentLifesteal * 3 + "% decreased maximum life" +
                "\n" + percentLifesteal * 3 + "% increased damage taken"*/);
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            modPlayer.lifeStealMelee += percentLifesteal;// * 0.01;
            modPlayer.lifeStealRange += percentLifesteal;// * 0.01;
            modPlayer.lifeStealMagic += percentLifesteal;// * 0.01;
            modPlayer.lifeStealMinion += percentLifesteal;// * 0.01;

            //modPlayer.healthModifier -= percentLifesteal * 0.03;
            //modPlayer.damageTakenModifier += percentLifesteal * 0.03;

            base.UpdateAccessory(player, modItem);
        }
    }
}
