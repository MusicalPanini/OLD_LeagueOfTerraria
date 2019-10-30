using Microsoft.Xna.Framework;
using TerraLeague.Projectiles;
using Terraria;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CustomItems.Actives
{
    public class Doom : Active
    {
        int percentMaxLife;
        int damageCap;
        int cooldown;

        public Doom(int PercentMaxLife, int DamageCap, int Cooldown)
        {
            percentMaxLife = PercentMaxLife;
            damageCap = DamageCap;
            cooldown = Cooldown;
        }

        public override string Tooltip(Player player, LeagueItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            return "[c/ff4d4d:Active: DOOM -] [c/ff8080:Target an enemy and deal " + percentMaxLife + "% of their max life as magic damage to it and all near by enemies (Max: " + damageCap + ")]" +
                "\n[c/ff8080:All hit enemies will take 20% more magic damage for 4 seconds]" +
                "\n[c/cc0000:" + (int)(cooldown * modPlayer.cdrLastStep) + " second cooldown]";
        }

        public override void DoActive(Player player, LeagueItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (modItem.GetStatOnPlayer(player) <= 0)
            {
                int npc = TerraLeague.NPCMouseIsHovering();
                if (npc != -1)
                {
                    int damage = (int)(Main.npc[npc].lifeMax * percentMaxLife * 0.01);
                    if (damage > damageCap)
                        damage = damageCap;
                    Projectile.NewProjectile(player.Center, Vector2.Zero, ProjectileType<DeathfireProj>(), damage, 0, player.whoAmI, npc);
                    modPlayer.FindAndSetActiveStat(this, (int)(cooldown * modPlayer.Cdr * 60));
                }
            }
        }

        public override void PostPlayerUpdate(Player player, LeagueItem modItem)
        {
            AddStat(player, modItem, cooldown * 60, -1, true);
            base.PostPlayerUpdate(player, modItem);
        }
    }
}

