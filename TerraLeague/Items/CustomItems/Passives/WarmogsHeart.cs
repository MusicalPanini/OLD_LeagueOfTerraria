using TerraLeague.Items.CompleteItems;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class WarmogsHeart : Passive
    {
        public override string Tooltip(Player player, ModItem modItem)
        {
            return "[c/0099cc:Passive: WARMOG'S HEART -] [c/99e6ff:If you have over 600 maximum life and are standing still, gain a huge life regen boost]";
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<Warmogs>()) != -1)
            {
                if (modItem.item.type == ItemType<Warmogs>())
                    modPlayer.warmogsHeart = true;
            }
            else
            {
                modPlayer.warmogsHeart = true;
            }

            base.UpdateAccessory(player, modItem);
        }
    }
}
