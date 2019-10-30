﻿using Terraria;
using Terraria.ModLoader;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class ManaCharge : Passive
    {
        public override string Tooltip(Player player, ModItem modItem)
        {
            return "[c/0099cc:Passive: MANA CHARGE -] [c/99e6ff:picking up Mana Stars will grant a stack up to 750]" +
                "\n[c/99e6ff:Gain 1 max mana per 10 stacks]";
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            player.statManaMax2 += modPlayer.manaChargeStacks/10;
            modPlayer.manaCharge = true;

            base.UpdateAccessory(player, modItem);
        }
    }
}
