using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class Disruption : Passive
    {
        public override string Tooltip(Player player, ModItem modItem)
        {
            return "[c/0099cc:Passive: DISRUPTION -] [c/99e6ff:Reduces NPC spawnrate]";
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            modPlayer.Disruption = true;

            base.UpdateAccessory(player, modItem);
        }
    }
}
