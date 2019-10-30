using System;
using TerraLeague.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class SteelTempest : ModProjectile
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

        public float movementFactor // Change this value to alter how fast the spear moves
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
                if (!player.GetModPlayer<PLAYERGLOBAL>().gathering2 && !player.GetModPlayer<PLAYERGLOBAL>().gathering3 /*&& !player.HasBuff(BuffType("Cooldown"))*/)
                {
                    player.AddBuff(BuffType<LastBreath2>(), 360);
                }
                else /*if (!player.HasBuff(BuffType("Cooldown")))*/
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

            //Vector2 ownerMountedCenter = projOwner.RotatedRelativePoint(projOwner.MountedCenter, true);
            player.itemTime = projectile.timeLeft;
            projectile.position.X = player.MountedCenter.X - (float)(projectile.width / 2);
            projectile.position.Y = player.MountedCenter.Y - (float)(projectile.height / 2);
            player.direction = projectile.direction;

            if (projectile.velocity.X < 0)
                projectile.spriteDirection = -1;
            // As long as the player isn't frozen, the spear can move
            if (!player.frozen)
            {
                if (movementFactor == 0f) // When initially thrown out, the ai0 will be 0f
                {
                    movementFactor = 5f; // Make sure the spear moves forward when initially thrown out
                    projectile.netUpdate = true; // Make sure to netUpdate this spear
                }
                if (player.itemTime < 20 / 2) // Somewhere along the item animation, make sure the spear moves back
                {
                    movementFactor -= 3f;
                }
                else // Otherwise, increase the movement factor
                {
                    movementFactor += 3f;
                }
            }
            // Change the spear position based off of the velocity and the movementFactor
            projectile.position += projectile.velocity * movementFactor;

            // Apply proper rotation, with an offset of 135 degrees due to the sprite's rotation, notice the usage of MathHelper, use this class!
            // MathHelper.ToRadians(xx degrees here)

            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
            
        }
    }
}
