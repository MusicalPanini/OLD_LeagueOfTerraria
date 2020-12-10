using Terraria;
using Terraria.ModLoader;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class IcyZone : Passive
    {
        public override string Tooltip(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            return TooltipName("Snow Bind") + TerraLeague.CreateColorString(PassiveSecondaryColor, "Triggering SPELLBLADE will cause nearby enemies to take ") + 
                TerraLeague.CreateScalingTooltip(TerraLeague.ARMORColor, "ARM", modPlayer.armorLastStep, 100) +
                TerraLeague.CreateColorString(PassiveSecondaryColor, " damage and apply 'Slowed'");
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            modPlayer.icyZone = true;
            base.UpdateAccessory(player, modItem);
        }
    }
}
