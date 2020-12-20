using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace TerraLeague.Items
{
    public class TerraLeagueHealthPotionGLOBAL : GlobalItem
    {
        public override void UpdateInventory(Item item, Player player)
        {
            if (item.healLife > 0)
            {
                int stack = item.stack;
                bool fav = item.favorited;

                item.SetDefaults(item.type);

                item.stack = stack;
                item.favorited = fav;

                if (player.GetModPlayer<PLAYERGLOBAL>().hasSpiritualRestorationLastStep)
                    item.healLife = (int)(item.healLife * 1.3);

                item.healLife = player.GetModPlayer<PLAYERGLOBAL>().ScaleValueWithHealPower(item.healLife, true);
            }

            base.UpdateInventory(item, player);
        }
    }
}
