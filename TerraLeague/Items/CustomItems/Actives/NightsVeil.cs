using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace TerraLeague.Items.CustomItems.Actives
{
    public class NightsVeil : Active
    {
        int duration;
        int shieldAmount;

        public NightsVeil(int Duration, int ShieldAmount, int Cooldown)
        {
            duration = Duration;
            shieldAmount = ShieldAmount;
            activeCooldown = Cooldown;
        }

        public override string Tooltip(Player player, LeagueItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            return TooltipName("NIGHT'S VEIL") + TerraLeague.CreateColorString(ActiveSecondaryColor, "Gain a ") + TerraLeague.CreateScalingTooltip(DamageType.NONE, shieldAmount, 100, true) + TerraLeague.CreateColorString(ActiveSecondaryColor, " Magic Shield for " + duration + " seconds") +
                 "\n" + TerraLeague.CreateColorString(ActiveSubColor, GetScaledCooldown(player) + " second cooldown");
        }

        public override void DoActive(Player player, LeagueItem modItem)
        {
            if (cooldownCount <= 0)
            {
                PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

                modPlayer.AddShield((int)(shieldAmount * modPlayer.healPower), duration * 60, new Color(43, 36, 110), ShieldType.Magic);
                SetCooldown(player);

                Efx(player);
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    PacketHandler.SendActiveEfx(-1, player.whoAmI, player.whoAmI, modItem.item.type);
                }
            }
        }

        public override void PostPlayerUpdate(Player player, LeagueItem modItem)
        {
            base.PostPlayerUpdate(player, modItem);
        }

        override public void Efx(Player user)
        {
            Main.PlaySound(new LegacySoundStyle(2, 29), user.Center);
        }
    }
}

