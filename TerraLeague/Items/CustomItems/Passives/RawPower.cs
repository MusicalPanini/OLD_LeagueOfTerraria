using Terraria;
using Terraria.ModLoader;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class RawPower : Passive
    {
        public override string Tooltip(Player player, ModItem modItem)
        {
            return "[c/0099cc:Passive: RAW POWER -] [c/99e6ff:Increase magic damage by 1.1x]";
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
