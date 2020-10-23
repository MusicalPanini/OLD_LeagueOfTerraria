using Terraria;
using Terraria.ModLoader;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class TouchOfDeath : Passive
    {
        int magicArmorPen;

        public TouchOfDeath(int MagicArmorPen)
        {
            magicArmorPen = MagicArmorPen;
        }

        public override string Tooltip(Player player, ModItem modItem)
        {
            return TooltipName("TOUCH OF DEATH") + TerraLeague.CreateColorString(PassiveSecondaryColor, "Increases magic armor penetration by " + magicArmorPen);
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            modPlayer.magicArmorPen += magicArmorPen * (int)passiveStat;

            base.UpdateAccessory(player, modItem);
        }
    }
}
