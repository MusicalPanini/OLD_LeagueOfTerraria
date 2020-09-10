using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TerraLeague.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class Infernum_Flame : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Infernum");
        }

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.alpha = 255;
            projectile.scale = 1f;
            projectile.timeLeft = 75;
            projectile.ranged = true;
        }

        public override void AI()
        {
            if (projectile.soundDelay == 0 && (int)projectile.ai[1] == 1)
                projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
            projectile.soundDelay = 100;

            if (Main.rand.Next(0, 1) == 0)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 88, 0, 0, 0, default(Color), 2.5f);
                dust.noGravity = true;
                dust.velocity *= 0.1f;
                dust.velocity += projectile.velocity;
                Dust dust2 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 88, 0, 0, 0, default(Color), 0.75f);
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPC(target, damage, knockback, crit);


            target.AddBuff(ModContent.BuffType<InfernumMark>(), 60 * 5);

            int critAI = crit ? 1 : 0;

            if ((int)projectile.ai[1] == 1)
            {
                for (int i = 0; i < 16; i++)
                {
                    Projectile.NewProjectileDirect(projectile.Center, projectile.velocity.RotatedBy(MathHelper.TwoPi / 16 * i) * 0.75f, ModContent.ProjectileType<Infernum_FlameSpread>(), (int)(projectile.damage * 0.75), projectile.knockBack, projectile.owner, target.whoAmI, critAI);
                }
            }
            else
            {
                if (crit)
                {
                    float startRad = MathHelper.ToRadians(50);
                    float rotation = startRad * 2 / 5f;

                    for (int i = 0; i < 6; i++)
                    {
                        Projectile.NewProjectileDirect(projectile.Center, projectile.velocity.RotatedBy(startRad - (rotation * i)) * 0.75f, ModContent.ProjectileType<Infernum_FlameSpread>(), (int)(projectile.damage * 0.5), projectile.knockBack, projectile.owner, target.whoAmI, critAI);
                    }
                }
                else
                {
                    float startRad = MathHelper.ToRadians(20);
                    float rotation = startRad * 2 / 3f;

                    for (int i = 0; i < 4; i++)
                    {
                        Projectile.NewProjectileDirect(projectile.Center, projectile.velocity.RotatedBy(startRad - (rotation * i)) * 0.75f, ModContent.ProjectileType<Infernum_FlameSpread>(), (int)(projectile.damage * 0.5), projectile.knockBack, projectile.owner, target.whoAmI);
                    }
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }

        public override void Kill(int timeLeft)
        {
            base.Kill(timeLeft);
        }

        public override bool? CanHitNPC(NPC target)
        {
            return base.CanHitNPC(target);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = height = 10;

            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }
    }
}
