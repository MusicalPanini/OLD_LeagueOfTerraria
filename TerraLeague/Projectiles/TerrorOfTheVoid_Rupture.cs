using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class TerrorOfTheVoid_Rupture : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rupture");
        }

        public override void SetDefaults()
        {
            projectile.width = 256;
            projectile.height = 128;
            projectile.friendly = false;
            projectile.penetrate = -1;
            projectile.alpha = 255;
            projectile.scale = 1f;
            projectile.timeLeft = 180;
            projectile.magic = true;
            projectile.tileCollide = true;
            projectile.scale = 4;
            drawOriginOffsetX = -128;
            drawOriginOffsetY = 192;
            projectile.hide = false;
            projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void AI()
        {
            if (projectile.timeLeft > 90)
            {
                for (int i = 0; i < 3; i++)
                {
                    Dust dustIndex = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y + 96 * 5), projectile.width, 32, 97, 0f, 0f, 0, default(Color), 1.5f);
                }
            }

            if (projectile.timeLeft == 90)
            {
                TerraLeague.PlaySoundWithPitch(projectile.Center, 2, 89, -1f);

                projectile.position.Y += projectile.height;
                projectile.velocity.Y = -projectile.height/8;
                projectile.alpha = 0;
                projectile.extraUpdates = 0;
                projectile.friendly = true;
                projectile.tileCollide = false;
            }
            if (projectile.timeLeft == 84)
            {
                projectile.velocity *= 0;
                projectile.extraUpdates = 0;
                projectile.friendly = false;
                for (int i = 0; i < 40; i++)
                {
                    Dust dustIndex = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 97, 0f, -8f, projectile.alpha, default(Color), 3);
                }
            }
            else if (projectile.timeLeft < 52)
            {
                projectile.alpha += 15;

                if (projectile.alpha <= 0)
                    projectile.Kill();
            }
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            target.velocity = new Vector2(0, -16);

            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override void Kill(int timeLeft)
        {
            base.Kill(timeLeft);
        }

        public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
        {
            //drawCacheProjsBehindNPCsAndTiles.Add(index);
        }
    }
}
