using System;
using Terraria;
using Terraria.ModLoader;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class Haste : Passive
    {
        public override string Tooltip(Player player, ModItem modItem)
        {
            return TooltipName("HASTE") + TerraLeague.CreateColorString(PassiveSecondaryColor, "Gain an additional 10 Ability, Item, and Summoner Spell Haste");
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            modPlayer.abilityHaste += 10;
            modPlayer.summonerHaste += 10;
            modPlayer.itemHaste += 10;

            base.UpdateAccessory(player, modItem);
        }
    }
}
