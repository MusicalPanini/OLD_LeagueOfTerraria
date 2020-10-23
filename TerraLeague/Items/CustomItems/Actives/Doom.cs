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

        public Doom(int PercentMaxLife, int DamageCap, int Cooldown)
        {
            percentMaxLife = PercentMaxLife;
            damageCap = DamageCap;
            activeCooldown = Cooldown;
        }

        public override string Tooltip(Player player, LeagueItem modItem)
        {
            return TooltipName("DISEASE HARVEST") + TerraLeague.CreateColorString(ActiveSecondaryColor, "Target an enemy and deal " + percentMaxLife + "% of their max life as magic damage to it and all near by enemies (Max: " + damageCap + ")" +
                "\nAll hit enemies will take 20% more magic damage for 4 seconds")
                + "\n" + TerraLeague.CreateColorString(ActiveSubColor, GetScaledCooldown(player) + " second cooldown");
        }

        public override void DoActive(Player player, LeagueItem modItem)
        {
            if (cooldownCount <= 0)
            {
                int npc = TerraLeague.NPCMouseIsHovering();
                if (npc != -1)
                {
                    int damage = (int)(Main.npc[npc].lifeMax * percentMaxLife * 0.01);
                    if (damage > damageCap)
                        damage = damageCap;
                    Projectile.NewProjectile(player.Center, Vector2.Zero, ProjectileType<Item_DoomBomb>(), damage, 0, player.whoAmI, npc);
                    SetCooldown(player);
                    //modPlayer.FindAndSetActiveStat(this, (int)(cooldown * modPlayer.Cdr * 60));
                }
            }
        }

        public override void PostPlayerUpdate(Player player, LeagueItem modItem)
        {
            base.PostPlayerUpdate(player, modItem);
        }
    }
}

