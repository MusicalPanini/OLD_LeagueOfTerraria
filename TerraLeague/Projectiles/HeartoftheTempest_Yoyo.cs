using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class HeartoftheTempest_Yoyo : ModProjectile
    {
        int hits = 0;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.YoyosLifeTimeMultiplier[projectile.type] = 10f;
            ProjectileID.Sets.YoyosMaximumRange[projectile.type] = 450f;
            ProjectileID.Sets.YoyosTopSpeed[projectile.type] = 13f;
            DisplayName.SetDefault("Heart of the Tempest");
        }

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.aiStyle = 99;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.melee = true;
        }

        public override void AI()
        {
            ProjectileID.Sets.YoyosLifeTimeMultiplier[projectile.type] = 10f;

            if (Main.rand.NextBool())
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 226, 0, 0, 0, new Color(0, 255, 255), 0.5f);
                dust.noGravity = true;
            }


            Lighting.AddLight(projectile.Center, 0f, 0.3f, 0.75f);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (projectile.owner == Main.LocalPlayer.whoAmI)
            {
                if (hits < 3)
                {
                    hits++;

                    if (hits >= 3)
                    {
                        Projectile.NewProjectileDirect(projectile.Center, Vector2.Zero, ModContent.ProjectileType<HeartoftheTempest_SlicingMaelstrom>(), damage, 0, projectile.owner, projectile.whoAmI);
                    }
                }
                else if (Main.player[projectile.owner].ownedProjectileCounts[ModContent.ProjectileType<HeartoftheTempest_SlicingMaelstrom>()] == 0 && hits >= 3)
                {
                    hits = 1;
                }
            }
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }

        public override void Kill(int timeLeft)
        {
            base.Kill(timeLeft);
        }
    }
}
