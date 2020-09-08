using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class AtlasGauntlets_Right : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Atlas Gauntlets");
        }

        public override void SetDefaults()
        {
            projectile.width = 30;
            projectile.height = 30;
            projectile.alpha = 0;
            projectile.timeLeft = 14;
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.melee = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
        }

        public override void AI()
        {
            if (projectile.soundDelay == 0)
            {
                TerraLeague.PlaySoundWithPitch(projectile.Center, 2, 1, -1);
            }
            projectile.soundDelay = 100;

            if (projectile.timeLeft > 3)
            {
                if (projectile.ai[0] < 3)
                {
                    projectile.ai[0]++;
                }
            }
            else
                projectile.ai[0]--;

            if (projectile.velocity.X < 0)
                projectile.spriteDirection = -1;

            Player player = Main.player[projectile.owner];
            player.heldProj = projectile.whoAmI;
            projectile.Center = player.MountedCenter + (projectile.velocity * (projectile.ai[0])) + new Vector2(-6, 0);
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;

            if (projectile.timeLeft == 10)
                projectile.friendly = false;

            player.ChangeDir(projectile.direction);
        }

        public override void Kill(int timeLeft)
        {
            
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = height = 10; 
            return true;
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (!Main.player[projectile.owner].CanHit(target))
                return false;
            return base.CanHitNPC(target);
        }
    }
}