using Terraria;
using Terraria.ModLoader;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class SpiritualRestoration : Passive
    {
        public override string Tooltip(Player player, ModItem modItem)
        {
            return "[c/0099cc:Passive: SPIRITUAL RESTORATION -] [c/99e6ff:Increases all incoming healing by 30%]";
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            modPlayer.spiritualRestur = true;

            base.UpdateAccessory(player, modItem);
        }
    }
}
