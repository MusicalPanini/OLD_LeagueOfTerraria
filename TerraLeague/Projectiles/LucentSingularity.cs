using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    class LucentSingularity : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lucent Singularity");
        }

        public override void SetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.timeLeft = 360;
            projectile.penetrate = 1000;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.magic = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.alpha = 255;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffType<Illuminated>(), 300);

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void AI()
        {
            if (projectile.width == 8)
            {
                if (projectile.velocity.X < 0 && projectile.Center.X < projectile.ai[0])
                {
                    if (projectile.velocity.Y < 0 && projectile.Center.Y < projectile.ai[1])
                    {
                        projectile.Center = new Vector2(projectile.ai[0], projectile.ai[1]);
                        Prime();
                    }
                    else if (projectile.velocity.Y >= 0 && projectile.Center.Y > projectile.ai[1])
                    {
                        projectile.Center = new Vector2(projectile.ai[0], projectile.ai[1]);
                        Prime();
                    }
                }
                else if (projectile.velocity.X >= 0 && projectile.Center.X > projectile.ai[0])
                {
                    if (projectile.velocity.Y < 0 && projectile.Center.Y < projectile.ai[1])
                    {
                        projectile.Center = new Vector2(projectile.ai[0], projectile.ai[1]);
                        Prime();
                    }
                    else if (projectile.velocity.Y >= 0 && projectile.Center.Y > projectile.ai[1])
                    {
                        projectile.Center = new Vector2(projectile.ai[0], projectile.ai[1]);
                        Prime();
                    }
                }

                if (projectile.timeLeft < 300)
                {
                    Prime();
                }

                for (int i = 0; i < 3; i++)
                {
                    Vector2 dustBoxPosition = new Vector2(projectile.position.X + 6, projectile.position.Y + 6);
                    int dustBoxWidth = projectile.width - 12;
                    int dustBoxHeight = projectile.height - 12;
                    Dust dust = Dust.NewDustDirect(dustBoxPosition, dustBoxWidth, dustBoxHeight, 228, 0f, 0f, 100, default(Color), 1.5f);
                    dust.noGravity = true;
                    dust.velocity *= 0.1f;
                    dust.velocity += projectile.velocity * 0.1f;
                    dust.position.X -= projectile.velocity.X / 3f * (float)i;
                    dust.position.Y -= projectile.velocity.Y / 3f * (float)i;
                }
            }
            else
            {
                if (projectile.soundDelay == 0)
                {
                    projectile.soundDelay = 25;
                    Microsoft.Xna.Framework.Audio.SoundEffectInstance efx = Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 15), projectile.Center);
                    if (efx != null)
                        efx.Pitch = 0.5f - (projectile.timeLeft/300f);
                }

                Dust dust = Dust.NewDustDirect(projectile.Center - (Vector2.One*8), 16, 16, 228, 0, 0, 0, default(Color), 1.5f);
                dust.velocity *= 1.25f;
                int displacement = Main.rand.Next(24);

                for (int i = 0; i < 18; i++)
                {
                    Vector2 pos = new Vector2(128, 0).RotatedBy(MathHelper.ToRadians((20 * i) + displacement)) + projectile.Center;

                    Dust dustR = Dust.NewDustPerfect(pos, 246, Vector2.Zero, 0, default(Color), 1);
                    dust.velocity *= 0;
                    dust.noGravity = true;
                }

                for (int i = 0; i < 200; i++)
                {
                    if (Main.npc[i].Hitbox.Intersects(projectile.Hitbox))
                    {
                        Main.npc[i].AddBuff(BuffType<Buffs.Slowed>(), 2);
                    }
                }
            }

            if (projectile.timeLeft <= 2)
                projectile.friendly = true;
            
            if (projectile.timeLeft < 30)
            {
                projectile.alpha += 9;
            }
        }

        public override void Kill(int timeLeft)
        {
            projectile.friendly = true;
            Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 20), projectile.position);

            Microsoft.Xna.Framework.Audio.SoundEffectInstance efx = Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 15), projectile.position);
            if (efx != null)
                efx.Pitch = 0.5f;

            TerraLeague.DustRing(246, projectile, default(Color));

            base.Kill(timeLeft);
        }

        public void Prime()
        {
            projectile.velocity = Vector2.Zero;
            projectile.timeLeft = 299;
            // Set to transparent. This projectile technically lives as  transparent for about 3 frames
            projectile.alpha = 255;
            // change the hitbox size, centered about the original projectile center. This makes the projectile damage enemies during the explosion.
            projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
            projectile.width = 256;
            projectile.height = 256;
            projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
        }
    }
}
