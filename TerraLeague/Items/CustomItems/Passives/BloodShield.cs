using Terraria;
using Terraria.ModLoader;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class BloodShield : Passive
    {
        public override string Tooltip(Player player, ModItem modItem)
        {
            return TooltipName("BLOOD Barrier") + TerraLeague.CreateColorString(PassiveSecondaryColor, "Life stealing will overheal, converting the heal into a shield (Max 200 shield)");
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            player.GetModPlayer<PLAYERGLOBAL>().bloodShield = true;

            base.UpdateAccessory(player, modItem);
        }
    }
}
