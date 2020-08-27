using System;
using TerraLeague.Buffs;
using TerraLeague.Projectiles.Minions;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class HextechWrench_EvolutionTurret : SentryMinion
    {
        int frameCount = 0;

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 6;
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.width = 48;
            projectile.height = 47;
            projectile.friendly = false;
            projectile.minion = true;
            projectile.sentry = true;
            projectile.penetrate = 1;
            projectile.timeLeft = Projectile.SentryLifeTime;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            shoot = ProjectileType<HextechWrench_EvoTurretShot>();
            shootSpeed = 20f;
            shootCool = 75;
            shootCountdown = 40;
            shootSound = new Terraria.Audio.LegacySoundStyle(2, 12, Terraria.Audio.SoundType.Sound).WithPitchVariance(1);
        }

        public override void CheckActive()
        {
            

            
        }

        public override void Behavior()
        {
            Player player = Main.player[projectile.owner];
            //if (player.HasBuff(BuffType<EvolutionTurrets>()))
            //{
            //    projectile.timeLeft = 2;
            //}
            base.Behavior();
        }

        public override void CreateDust()
        {

        }

        public void CheckIfNearOwner()
        {
            Player checkTarget = Main.player[projectile.owner];
            float distance = Vector2.Distance(checkTarget.Center, projectile.Center);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            fallThrough = false;
            return true;
        }

        public override void SelectFrame()
        {
            int currentAnim;

            if (frameCount > 20)
                currentAnim = 1;
            else
                currentAnim = 0;

            if (frameCount >= 40)
                frameCount = 0;
            else
                frameCount++;

            if (target != null)
            {
                double xdif = projectile.Center.X - (target.position.X + (target.width / 2.0));
                double ydif = projectile.Center.Y - (target.position.Y + (target.height / 2.0));

                if (xdif > 0)
                {
                    projectile.spriteDirection = -1;
                }
                else
                {
                    projectile.spriteDirection = 1;
                }

                if (ydif > 0)
                {
                    if (Math.Abs(ydif) * Math.Sqrt(3) < Math.Abs(xdif) * (1 / Math.Sqrt(3)))
                    {
                        projectile.frame = 0;
                    }
                    else
                    {
                        projectile.frame = 4;
                    }
                }
                else
                {
                    if (Math.Abs(ydif) * Math.Sqrt(3) < Math.Abs(xdif)* (1/Math.Sqrt(3)))
                    {
                        projectile.frame = 0;
                    }
                    else
                    {
                        projectile.frame = 2;
                    }
                }
            }
            else
            {
                projectile.frame = 0;
            }

            projectile.frame += currentAnim;
        }
    }
}
