using System;
using TerraLeague.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class LastBreath_SteelTempest : ModProjectile
    {
        bool enemyHit = false;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("SteelTempest");
        }

        public override void SetDefaults()
        {
            projectile.width = 14;
            projectile.height = 14;
            projectile.penetrate = -1;
            projectile.alpha = 0;
            projectile.melee = true;
            projectile.tileCollide = false;
            projectile.friendly = true;
            projectile.timeLeft = 20;
        }

        public float movementFactor
        {
            get
            {
                return projectile.ai[0];
            }
            set
            {
                projectile.ai[0] = value;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Player player = Main.player[projectile.owner];

            if (!enemyHit)
            {
                if (!player.GetModPlayer<PLAYERGLOBAL>().gathering2 && !player.GetModPlayer<PLAYERGLOBAL>().gathering3)
                {
                    player.AddBuff(BuffType<LastBreath2>(), 360);
                }
                else
                {
                    player.AddBuff(BuffType<LastBreath3>(), 360);
                    player.ClearBuff(BuffType<LastBreath2>());
                }
            }

            enemyHit = true;

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void AI()
        {
            

            if (projectile.timeLeft == 20)
            {
                if (projectile.ai[1] != 0)
                {
                    projectile.friendly = false;
                    Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 117).WithPitchVariance(0.8f), projectile.Center);
                }
                else
                {
                    Main.PlaySound(SoundID.Item1, projectile.Center);
                }
            }

            Player player = Main.player[projectile.owner];

            player.itemTime = projectile.timeLeft;
            projectile.position.X = player.MountedCenter.X - (float)(projectile.width / 2);
            projectile.position.Y = player.MountedCenter.Y - (float)(projectile.height / 2);
            player.direction = projectile.direction;

            if (projectile.velocity.X < 0)
                projectile.spriteDirection = -1;
            if (!player.frozen)
            {
                if (movementFactor == 0f) 
                {
                    movementFactor = 5f; 
                    projectile.netUpdate = true;
                }
                if (player.itemTime < 20 / 2) 
                {
                    movementFactor -= 3f;
                }
                else 
                {
                    movementFactor += 3f;
                }
            }
            projectile.position += projectile.velocity * movementFactor;

            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
        }
    }
}
