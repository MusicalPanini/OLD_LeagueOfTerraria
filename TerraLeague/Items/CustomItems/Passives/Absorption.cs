using Terraria;
using Terraria.ModLoader;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class Absorption : Passive
    {
        public override string Tooltip(Player player, ModItem modItem)
        {
            return TooltipName("Absorbtion") + 
                TerraLeague.CreateColorString(PassiveSecondaryColor, "Every second in combat will grant a stack up to 6\nGain 1 resist per stack");
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            modPlayer.resist += (int)passiveStat;

            base.UpdateAccessory(player, modItem);
        }

        public override void PostPlayerUpdate(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (modPlayer.CombatTimer >= 240)
            {
                cooldownCount = 0;
                passiveStat = 0;
            }
            if (modPlayer.CombatTimer < 120 && passiveStat < 6)
            {
                cooldownCount++;
                if (cooldownCount >= 60)
                {
                    cooldownCount = 0;
                    AddStat(player, 6, 1);
                }
            }

            base.PostPlayerUpdate(player, modItem);
        }
    }
}
