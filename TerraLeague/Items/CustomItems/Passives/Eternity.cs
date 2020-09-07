using Terraria;
using Terraria.ModLoader;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class Eternity : Passive
    {
        public override string Tooltip(Player player, ModItem modItem)
        {
            return TooltipName("ETERNITY") + TerraLeague.CreateColorString(PassiveSecondaryColor, "Taking damage restores mana\nUsing mana heals life");
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            player.magicCuffs = true;

            base.UpdateAccessory(player, modItem);
        }

        public override void PostPlayerUpdate(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (modPlayer.manaLastStep > player.statMana && player.whoAmI == Main.myPlayer)
            {
                if ((int)((modPlayer.manaLastStep - player.statMana) * 0.1) > 0 && modPlayer.manaLastStep <= player.statManaMax2)
                {
                    modPlayer.lifeToHeal += modPlayer.ScaleValueWithHealPower((modPlayer.manaLastStep - player.statMana) * 0.2f);
                }
            }

            modPlayer.manaLastStep = player.statMana;

            base.PostPlayerUpdate(player, modItem);
        }
    }
}
