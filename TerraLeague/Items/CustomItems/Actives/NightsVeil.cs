using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;

namespace TerraLeague.Items.CustomItems.Actives
{
    public class NightsVeil : Active
    {
        int duration;
        int shieldAmount;
        int cooldown;

        public NightsVeil(int Duration, int ShieldAmount, int Cooldown)
        {
            duration = Duration;
            shieldAmount = ShieldAmount;
            cooldown = Cooldown;
        }

        public override string Tooltip(Player player, LeagueItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            return "[c/ff4d4d:Active: NIGHT'S VEIL -] [c/ff8080:Gain a " + (int)(shieldAmount * Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().healPowerLastStep) + " Magic Shield for " + duration + " seconds]" +
                "\n[c/cc0000:" + (int)(cooldown * modPlayer.cdrLastStep) + " second cooldown]";
        }

        public override void DoActive(Player player, LeagueItem modItem)
        {
            if (modItem.GetStatOnPlayer(player) <= 0)
            {
                PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

                modPlayer.AddShieldAttachedToBuff((int)(shieldAmount * modPlayer.healPower), duration * 60, new Color(43, 36, 110), ShieldType.Magic);
                modPlayer.FindAndSetActiveStat(this, (int)(cooldown * modPlayer.Cdr * 60));

                Efx(player);
                if (Main.netMode == 1)
                {
                    PacketHandler.SendActiveEfx(-1, player.whoAmI, player.whoAmI, modItem.item.type);
                }
            }
        }

        public override void PostPlayerUpdate(Player player, LeagueItem modItem)
        {
            AddStat(player, modItem, cooldown * 60, -1, true);
            base.PostPlayerUpdate(player, modItem);
        }

        override public void Efx(Player user)
        {
            //TerraLeague.DustRing(261, user, new Color(255, 255, 0, 0));
            Main.PlaySound(new LegacySoundStyle(2, 29), user.Center);
        }
    }
}

