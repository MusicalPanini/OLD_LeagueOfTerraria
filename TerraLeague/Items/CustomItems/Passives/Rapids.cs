using Terraria;
using Terraria.ModLoader;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class Rapids : Passive
    {
        public override string Tooltip(Player player, ModItem modItem)
        {
            return TooltipName("Rapids") + TerraLeague.CreateColorString(PassiveSecondaryColor, "Healing an ally will give you both 15% movement speed and " +
                "\n10% increased magic and minion damage");
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            player.GetModPlayer<PLAYERGLOBAL>().rapids = true;

            base.UpdateAccessory(player, modItem);
        }
    }
}
