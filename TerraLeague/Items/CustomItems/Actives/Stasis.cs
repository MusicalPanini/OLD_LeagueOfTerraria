using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CustomItems.Actives
{
    public class Stasis : Active
    {
        int duration;
        bool stopWatch;

        public Stasis(int Duration, int Cooldown, bool StopWatch = false)
        {
            duration = Duration;
            activeCooldown = Cooldown;
            stopWatch = StopWatch;
        }

        public override string Tooltip(Player player, LeagueItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            if (stopWatch)
            {
                return TooltipName("STASIS") + TerraLeague.CreateColorString(ActiveSecondaryColor, "Render yourself frozen and invulnerable for " + duration + " seconds") +
                 "\n" + TerraLeague.CreateColorString(ActiveSubColor, "Can only be used once a day");
            }
            else
            {
                return TooltipName("STASIS") + TerraLeague.CreateColorString(ActiveSecondaryColor, "Render yourself frozen and invulnerable for " + duration + " seconds") +
                 "\n" + TerraLeague.CreateColorString(ActiveSubColor, GetScaledCooldown(player) + " second cooldown");
            }
        }

        public override void DoActive(Player player, LeagueItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (cooldownCount <= 0 && !stopWatch || stopWatch && modPlayer.stopWatchActive)
            {
                if (stopWatch)
                {
                    modPlayer.stopWatchActive = false;
                }
                else
                {
                    SetCooldown(player);
                }

                player.AddBuff(BuffType<Buffs.Stasis>(), 120);

                Efx(player);
                if (Main.netMode == NetmodeID.MultiplayerClient)
                    PacketHandler.SendActiveEfx(-1, player.whoAmI, player.whoAmI, modItem.item.type);
            }
        }

        public override void PostPlayerUpdate(Player player, LeagueItem modItem)
        {
            base.PostPlayerUpdate(player, modItem);
        }

        static public void Efx(int user)
        {
            Player player = Main.player[user];

            TerraLeague.DustRing(261, player, new Color(255, 255, 0, 0));
            Main.PlaySound(new LegacySoundStyle(2, 29), player.Center);
        }
    }
}

