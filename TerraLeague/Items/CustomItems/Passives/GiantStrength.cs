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
            return "[c/0099cc:Passive: GIANT STRENGTH -] [c/99e6ff:Melee attacks deal] [c/" + TerraLeague.MELColor + ":" + (int)(modPlayer.MEL * meleeScaling / 100d) + "] [c/99e6ff:extra damage]";
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            modPlayer.meleeFlatDamage += (int)(modPlayer.MEL * meleeScaling / 100d);

            base.UpdateAccessory(player, modItem);
        }
    }
}
