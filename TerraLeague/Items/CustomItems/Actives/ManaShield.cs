using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace TerraLeague.Items.CustomItems.Actives
{
    public class ManaShield : Active
    {
        int duration;
        int percentMana;
        int manaScaling;
        int baseShield;

        public ManaShield(int Duration, int PercentMana, int ManaScaling, int BaseShield, int Cooldown)
        {
            duration = Duration;
            percentMana = PercentMana;
            manaScaling = ManaScaling;
            baseShield = BaseShield;
            activeCooldown = Cooldown;
        }

        public override string Tooltip(Player player, LeagueItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            return TooltipName("MANA Barrier") + TerraLeague.CreateColorString(ActiveSecondaryColor, "Consume ") + TerraLeague.CreateScalingTooltip(UI.HealthbarUI.ManaColor.Hex3(), "CUR MANA", modPlayer.manaLastStep, percentMana) 
                + TerraLeague.CreateColorString(ActiveSecondaryColor, " mana\nGain a ") + TerraLeague.CreateScalingTooltip(DamageType.NONE, baseShield, 100, true) + " + " + TerraLeague.CreateScalingTooltip(UI.HealthbarUI.ManaColor.Hex3(), "CUR MANA", modPlayer.manaLastStep, (int)(percentMana * manaScaling * 0.01), true)
                + TerraLeague.CreateColorString(ActiveSecondaryColor, " shield for " + duration + " seconds")
                +"\n" + TerraLeague.CreateColorString(ActiveSubColor, GetScaledCooldown(player) + " second cooldown"); ;
        }

        public override void DoActive(Player player, LeagueItem modItem)
        {
            if (cooldownCount <= 0)
            {
                PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

                int manaUsed = (int)(player.statMana * percentMana * 0.01);
                player.CheckMana(manaUsed, true);

                modPlayer.AddShield(manaUsed + baseShield,duration * 60, Color.SkyBlue, ShieldType.Basic);
                SetCooldown(player);

                Efx(player);
                if (Main.netMode == NetmodeID.MultiplayerClient)
                    PacketHandler.SendActiveEfx(-1, player.whoAmI, player.whoAmI, modItem.item.type);
            }
        }


        public override void PostPlayerUpdate(Player player, LeagueItem modItem)
        {
            base.PostPlayerUpdate(player, modItem);
        }

        override public void Efx(Player user)
        {
            Main.PlaySound(new LegacySoundStyle(2, 29).WithPitchVariance(-0.5f), user.Center);
        }
    }
}

