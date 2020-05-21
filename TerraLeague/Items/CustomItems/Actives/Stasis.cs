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
        int cooldown;
        bool stopWatch;

        public Stasis(int Duration, int Cooldown, bool StopWatch = false)
        {
            duration = Duration;
            cooldown = Cooldown;
            stopWatch = StopWatch;
        }

        public override string Tooltip(Player player, LeagueItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            if (stopWatch)
            {
                return "[c/ff4d4d:Active: STASIS -] [c/ff8080:Render yourself frozen and invulnerable for " + duration + " seconds]" +
                    "\n[c/cc0000:Can only be used once a day]";
            }
            else
            {
                return "[c/ff4d4d:Active: STASIS -] [c/ff8080:Render yourself frozen and invulnerable for " + duration + " seconds]" +
                    "\n[c/cc0000:" + (int)(cooldown * modPlayer.cdrLastStep) + " second cooldown]";
            }
        }

        public override void DoActive(Player player, LeagueItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (modItem.GetStatOnPlayer(player) <= 0 && !stopWatch || stopWatch && modPlayer.stopWatchActive)
            {
                if (stopWatch)
                {
                    modPlayer.stopWatchActive = false;
                }
                else
                {
                    modPlayer.FindAndSetActiveStat(this, (int)(cooldown * modPlayer.Cdr * 60));
                }

                player.AddBuff(BuffType<Buffs.Stasis>(), 120);

                Efx(player);
                if (Main.netMode == NetmodeID.MultiplayerClient)
                    PacketHandler.SendActiveEfx(-1, player.whoAmI, player.whoAmI, modItem.item.type);
            }
        }

        public override void PostPlayerUpdate(Player player, LeagueItem modItem)
        {
            if (!stopWatch)
                AddStat(player, modItem, cooldown * 60, -1, true);

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

