using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class CrystalStaff_PrimordialBurst : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Primordial Burst");
        }

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.timeLeft = 300;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.magic = true;
            projectile.alpha = 255;
            projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }


        public override void AI()
        {
            if (projectile.soundDelay == 0)
            {
                TerraLeague.PlaySoundWithPitch(projectile.Center, 2, 8, 0f);
                TerraLeague.PlaySoundWithPitch(projectile.Center, 2, 113, -0.5f);
            }
            projectile.soundDelay = 100;

            if (projectile.ai[0] != -2)
            {
                projectile.friendly = true;

                NPC npc = Main.npc[(int)projectile.ai[0]];

                if (!npc.active && projectile.owner == Main.LocalPlayer.whoAmI)
                    projectile.Kill();

                float MaxSpeed = 18;

                float XDist = (float)npc.Center.X - projectile.Center.X;
                float YDist = (float)npc.Center.Y - projectile.Center.Y;

                float TrueDist = (float)System.Math.Sqrt((double)(XDist * XDist + YDist * YDist));
                if (TrueDist > MaxSpeed)
                {
                    TrueDist = MaxSpeed / TrueDist;
                    XDist *= TrueDist;
                    YDist *= TrueDist;
                    int num118 = (int)(XDist * 1000f);
                    int num119 = (int)(projectile.velocity.X * 1000f);
                    int num120 = (int)(YDist * 1000f);
                    int num121 = (int)(projectile.velocity.Y * 1000f);
                    if (num118 != num119 || num120 != num121)
                    {
                        projectile.netUpdate = true;
                    }

                    if (projectile.timeLeft > 270)
                    {
                        projectile.velocity.X = XDist * (1 - ((projectile.timeLeft - 270) / 30f));
                        projectile.velocity.Y = YDist * (1 - ((projectile.timeLeft - 270) / 30f));
                    }
                    else
                    {
                        projectile.velocity.X = XDist;
                        projectile.velocity.Y = YDist;
                    }

                }
            }

            for (int i = 0; i < 3; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position + Vector2.One * 4, projectile.width - 8, projectile.height - 8, i == 1 ? 173 : 172, 0, 0, 0, default, 3f);
                dust.noGravity = true;
                dust.velocity *= 0.3f;
                dust.velocity += projectile.velocity * 0.3f;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage = (int)(damage * (2 - (target.life / (float)target.lifeMax)));
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override bool? CanHitNPC(NPC target)
        {
            if ((int)projectile.ai[0] == target.whoAmI)
                return base.CanHitNPC(target);
            else
                return false;
        }


        public override void Kill(int timeLeft)
        {
            TerraLeague.PlaySoundWithPitch(projectile.Center, 2, 45, -0.5f);
            Main.PlaySound(SoundID.DD2_ExplosiveTrapExplode.WithVolume(1f), projectile.position);

            Dust dust;
            for (int i = 0; i < 25; i++)
            {
                dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 172, 0, 0, 0, default, 2f);
                dust.velocity *= 1.4f;
                dust.noGravity = true;
            }
            for (int i = 0; i < 40; i++)
            {
                dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 173, 0, 0, 0, default, 2f);
                dust.noGravity = true;
                dust.velocity *= 5f;
                dust.color = new Color(255, 0, 220);
                dust.noGravity = true;

                dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 172, 0, 0, 0, default, 2f);
                dust.velocity *= 3f;
                dust.color = new Color(255, 0, 220);
                dust.noGravity = true;
            }
            base.Kill(timeLeft);
        }
    }
}
