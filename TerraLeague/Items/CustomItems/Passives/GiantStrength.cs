using Terraria;
using Terraria.ModLoader;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class GiantStrength : Passive
    {
        int meleeScaling;

        public GiantStrength(int MeleeScaling)
        {
            meleeScaling = MeleeScaling;
        }

        public override string Tooltip(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            return TooltipName("GIANT STRENGTH") + TerraLeague.CreateColorString(PassiveSecondaryColor, "Melee attacks deal ") + TerraLeague.CreateScalingTooltip(DamageType.MEL, modPlayer.MEL, meleeScaling) + TerraLeague.CreateColorString(PassiveSecondaryColor, " extra damage");
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            modPlayer.meleeFlatDamage += (int)(modPlayer.MEL * meleeScaling / 100d);

            base.UpdateAccessory(player, modItem);
        }
    }
}
