using Terraria;
using Terraria.ModLoader;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class IcyZone : Passive
    {
        public override string Tooltip(Player player, ModItem modItem)
        {
            return "[c/0099cc:Passive: ICY ZONE -] [c/99e6ff:Triggering SPELLBLADE will cause nearby enemies to take damage equal to you defence + armor and be slowed]";
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            modPlayer.icyZone = true;
            base.UpdateAccessory(player, modItem);
        }
    }
}
