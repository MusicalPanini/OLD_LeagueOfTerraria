using System;
using Terraria;
using Terraria.ModLoader;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class Haste : Passive
    {
        public override string Tooltip(Player player, ModItem modItem)
        {
            return TooltipName("HASTE") + TerraLeague.CreateColorString(PassiveSecondaryColor, "Gain an addtional 10% CDR");
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            modPlayer.Cdr -= 0.1;

            base.UpdateAccessory(player, modItem);
        }
    }
}
