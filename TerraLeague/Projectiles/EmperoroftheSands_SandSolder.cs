using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TerraLeague.Buffs;
using TerraLeague.Projectiles.Minions;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class EmperoroftheSands_SandSolder : GroundMinion
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sand Solder");
            //Main.projPet[projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
            ProjectileID.Sets.Homing[projectile.type] = true;
            Main.projFrames[projectile.type] = 6;

            drawOriginOffsetY -= 12;
            drawOffsetX -= 12;
        }

        public override void SetDefaults()
        {
            projectile.width = 24;
            projectile.height = 24;
            projectile.netImportant = true;
            projectile.alpha = 0;
            projectile.timeLeft = 100;
            projectile.friendly = true;
            projectile.minion = true;
            projectile.minionSlots = 1f;
            projectile.penetrate = -1;

            // Animation
            attackFrame = 1;
            attackFrameCount = 4;

            runFrame = 1;
            runFrameCount = 4;
            runFrameSpeedMod = 0.1f;

            flyFrame = 5;
            flyFrameCount = 1;
            flyFrameSpeed = 1;
            flyRotationMod = 0.5f;

            fallFrame = 5;
            fallFrameCount = 1;

            idleFrame = 0;
            idleFrameCount = 1;

            AIPrioritiseNearPlayer = false;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 30;
        }

        public override void AI()
        {
            if (projectile.timeLeft == 100)
            {
                for (int i = 0; i < 20; i++)
                {
                    Dust dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y + 16), projectile.width, projectile.height, 32, 0f, 0, 0, default(Color), 3f);
                    dust.noGravity = true;
                    dust.velocity.Y -= 2;

                    dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y + 16), projectile.width, projectile.height, 32, 0f, 0, 0, default(Color), 2f);
                    dust.noGravity = true;
                    dust.velocity.Y -= 3;
                }
            }

            if (Main.player[projectile.owner].HasBuff(ModContent.BuffType<SandSolder>()))
                projectile.timeLeft = 10;

            if (Main.rand.Next(0, 4) == 0)
            {
                Dust dust = Dust.NewDustDirect(projectile.BottomLeft, projectile.width - 4, 2, 32);
                dust.position.Y -= 2;
                dust.velocity.Y *= 0.2f;
                dust.fadeIn = 0.5f;
                dust.noGravity = true;
            }

            if (Main.rand.Next(0, 30) == 0)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 32);
                dust.fadeIn = 0.5f;
            }

            base.AI();
        }

        public override bool? CanCutTiles()
        {
            return false;
        }

        public override bool MinionContactDamage()
        {
            return true;
        }
    }
}
