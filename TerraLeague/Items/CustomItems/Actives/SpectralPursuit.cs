using Microsoft.Xna.Framework;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CustomItems.Actives
{
    public class SpectralPursuit : Active
    {
        int baseDamage;
        int minionScaling;
        int cooldown;

        public SpectralPursuit(int BaseDamage, int MinionScaling, int Cooldown)
        {
            baseDamage = BaseDamage;
            minionScaling = MinionScaling;
            cooldown = Cooldown;
        }

        public override string Tooltip(Player player, LeagueItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            return "[c/ff4d4d:Active: SPECTURAL PURSUIT -] [c/ff8080:Send out " + modPlayer.maxMinionsLastStep + " spooky ghosts that track down a nearby enemy, dealing] " + baseDamage + " + [c/" + TerraLeague.SUMColor + ":" + (int)(modPlayer.SUM * minionScaling / 100d) + "] [c/ff8080:damage and slowing]" +
                "\n[c/cc0000:" + (int)(cooldown * modPlayer.cdrLastStep) + " second cooldown]";
        }

        public override void DoActive(Player player, LeagueItem modItem)
        {
            if (modItem.GetStatOnPlayer(player) <= 0)
            {
                PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

                Efx(player);
                modPlayer.FindAndSetActiveStat(this, (int)(cooldown * modPlayer.Cdr * 60));

                if (player.whoAmI == Main.myPlayer)
                {
                    for (int i = 0; i < modPlayer.maxMinionsLastStep; i++)
                    {
                        float hSpeed = 5;
                        if (modPlayer.maxMinionsLastStep > 1)
                            hSpeed = 10 * ((i) / (float)(modPlayer.maxMinionsLastStep - 1));


                        Projectile.NewProjectile(player.position.X, player.position.Y, hSpeed-5, -4, ProjectileType<SpookyGhost>(), baseDamage + (int)(modPlayer.SUM * minionScaling / 100d), 0, player.whoAmI);
                    }
                }

                // For Server
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
            TerraLeague.DustRing(261, user, new Color(0, 255, 255, 0));
            Main.PlaySound(new LegacySoundStyle(3, 54).WithPitchVariance(-0.2f), user.position);
        }
    }
}

