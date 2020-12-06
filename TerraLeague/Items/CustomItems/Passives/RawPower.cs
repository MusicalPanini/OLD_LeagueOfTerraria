using Terraria;
using Terraria.ModLoader;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class RawPower : Passive
    {
        public override string Tooltip(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            return TooltipName("Magical opus") + TerraLeague.CreateColorString(PassiveSecondaryColor, "Gain 1.1x increased magic damage");
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            modPlayer.magicStatScaling += 0.1f;
            modPlayer.rawPower = true;

            base.UpdateAccessory(player, modItem);
        }
    }
}
