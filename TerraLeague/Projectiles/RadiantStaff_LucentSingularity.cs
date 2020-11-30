using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    class RadiantStaff_LucentSingularity : ModProjectile
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
            projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
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
                    TerraLeague.PlaySoundWithPitch(projectile.Center, 2, 15, 0.5f - (projectile.timeLeft / 300f));
                }

                Dust dust = Dust.NewDustDirect(projectile.Center - (Vector2.One*8), 16, 16, 228, 0, 0, 0, default(Color), 1.5f);
                dust.velocity *= 0;
                dust.noGravity = true;

                TerraLeague.DustBorderRing(projectile.width / 2, projectile.Center, 246, default(Color), 1);

                if (projectile.timeLeft % 15 == 0)
                {
                    TerraLeague.GiveNPCsInRangeABuff(projectile.Center, projectile.width / 2f, BuffType<Buffs.Slowed>(), 15, true, true);
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

            TerraLeague.PlaySoundWithPitch(projectile.Center, 2, 15, 0.5f);

            TerraLeague.DustRing(246, projectile, default(Color));

            base.Kill(timeLeft);
        }

        public void Prime()
        {
            projectile.velocity = Vector2.Zero;
            projectile.timeLeft = 299;
            projectile.alpha = 255;
            projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
            projectile.width = 256;
            projectile.height = 256;
            projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (projectile.friendly && !target.townNPC)
                return TerraLeague.IsHitboxWithinRange(projectile.Center, target.Hitbox, projectile.width / 2);
            else
                return false;
        }

        public override bool? CanCutTiles()
        {
            return projectile.friendly;
        }
    }
}
