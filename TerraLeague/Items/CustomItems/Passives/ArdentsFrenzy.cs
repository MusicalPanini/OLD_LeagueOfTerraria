using Terraria;
using Terraria.ModLoader;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class ArdentsFrenzy : Passive
    {
        public override string Tooltip(Player player, ModItem modItem)
        {
            return "[c/0099cc:Passive: ARDENT'S FRENZY -] [c/99e6ff:Healing an ally will give you both 'Frenzy']";
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            player.GetModPlayer<PLAYERGLOBAL>().ardentsFrenzy = true;

            base.UpdateAccessory(player, modItem);
        }
    }
}
