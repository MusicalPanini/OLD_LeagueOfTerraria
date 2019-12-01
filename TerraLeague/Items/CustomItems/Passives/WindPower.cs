using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
namespace TerraLeague.Items.CustomItems.Passives
{
    public class WindPower : Passive
    {
        public override string Tooltip(Player player, ModItem modItem)
        {
            return "[c/0099cc:Passive: WIND POWER -] [c/99e6ff:15% increased attack speed]";
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            player.GetModPlayer<PLAYERGLOBAL>().windPower = true;

            base.UpdateAccessory(player, modItem);
        }
    }
}
