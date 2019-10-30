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
            return "[c/0099cc:Passive: TOUCH OF DEATH -] [c/99e6ff:Increases magic armor penetration by " + magicArmorPen + "]";
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            modPlayer.magicArmorPen += magicArmorPen;

            base.UpdateAccessory(player, modItem);
        }
    }
}
