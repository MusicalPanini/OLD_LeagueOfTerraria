using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    class SMDR : ModProjectile
    {
        int baseDamage;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Super Mega Death Rocket");
        }

        public override void SetDefaults()
        {
            projectile.width = 26;
            projectile.height = 26;
            projectile.timeLeft = 1200;
            projectile.penetrate = 1000;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.ranged = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.scale = 1.5f;
            aiType = 0;
        }

        public override void AI()
        {
            if (projectile.timeLeft == 1200)
                baseDamage = projectile.damage;

            Lighting.AddLight(projectile.Center, 1f, 0.34f, 0.9f);
            projectile.damage = (int)(baseDamage * (projectile.velocity.Length() / 25));
            projectile.rotation = projectile.velocity.ToRotation();
            if (projectile.velocity.X < 0)
            {
                projectile.rotation = projectile.velocity.ToRotation();
                projectile.scale = -1.5f;
                projectile.spriteDirection = -1;
            }

            if (projectile.velocity.Length() < 25)
            {
                projectile.velocity.X *= 1.05f;
                projectile.velocity.Y *= 1.05f;

            }

            for (int i = 0; i < 3; i++)
            {
                Dust dust1 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height/2, 6);
                Dust dust2 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height/2, 6);
                dust1.scale = 2 * (projectile.velocity.Length() / 25);
                dust1.noGravity = true;
                dust2.scale = 2 * (projectile.velocity.Length() / 50);
                dust2.noGravity = true;
            }
            
            if (projectile.timeLeft < 30)
            {
                projectile.alpha += 9;
            }
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.DD2_ExplosiveTrapExplode.WithVolume(1f), projectile.position);

            for (int i = 0; i < 80; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 31, 0f, 0f, 100, default(Color), 2f);
                dust.velocity *= 3f;
                if (Main.rand.Next(2) == 0)
                {
                    dust.scale = 0.5f;
                    dust.fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                }
            }
            for (int i = 0; i < 120; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 4.5f);
                dust.noGravity = true;
                dust.velocity *= 5f;

                dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 3f);
                dust.velocity *= 3f;
            }
            for (int i = 0; i < 3; i++)
            {
                float velScale = (i+1) * 1f;

                Gore gore = Gore.NewGoreDirect(projectile.Center, default(Vector2), Main.rand.Next(61, 64), 2f);
                gore.velocity.X += 1.5f;
                gore.velocity.Y += 1.5f;

                gore = Gore.NewGoreDirect(projectile.Center, default(Vector2), Main.rand.Next(61, 64), 2f);
                gore.velocity.X -= 1.5f;
                gore.velocity.Y -= 1.5f;

                gore = Gore.NewGoreDirect(projectile.Center, default(Vector2), Main.rand.Next(61, 64), 2f);
                gore.velocity.X += 1.5f;
                gore.velocity.Y -= 1.5f;

                gore = Gore.NewGoreDirect(projectile.Center, default(Vector2), Main.rand.Next(61, 64), 2f);
                gore.velocity.X -= 1.5f;
                gore.velocity.Y += 1.5f;
            }

            //// Play explosion sound
            //// Smoke Dust spawn
            //for (int i = 0; i < 50; i++)
            //{
            //    int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 31, 0f, 0f, 100, default(Color), 2f);
            //    Main.dust[dustIndex].velocity *= 1.4f;
            //}
            //// Fire Dust spawn
            //for (int i = 0; i < 120; i++)
            //{
            //    int dustIndex = Dust.NewDust(projectile.Center, 1, 1, 6, 0f, 0f, 100, default(Color), 3f);
            //    Main.dust[dustIndex].noGravity = true;
            //    Main.dust[dustIndex].velocity *= 30f;
            //    Main.dust[dustIndex].velocity.X *= 1.5f;

            //    dustIndex = Dust.NewDust(projectile.position, projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 3f);
            //    Main.dust[dustIndex].velocity *= 3f;
            //}
            //// Large Smoke Gore spawn
            //for (int g = 0; g < 4; g++)
            //{
            //    int goreIndex = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 3f);
            //    Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X + 1.2f*g;
            //    Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 1.2f * g;
            //    goreIndex = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 3f);
            //    Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X - 1.2f * g;
            //    Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y - 1.2f * g;
            //    goreIndex = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 3f);
            //    Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X - 1.2f * g;
            //    Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 1.2f * g;
            //    goreIndex = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 3f);
            //    Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X + 1.2f * g;
            //    Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y - 1.2f * g;
            //}
            //// reset size to normal width and height.
            //projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
            //projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
            //projectile.width = 10;
            //projectile.height = 10;
            //projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
            //projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);

            //// TODO, tmodloader helper method
            //{
            //    int explosionRadius = 10;
            //    int minTileX = (int)(projectile.position.X / 16f - (float)explosionRadius);
            //    int maxTileX = (int)(projectile.position.X / 16f + (float)explosionRadius);
            //    int minTileY = (int)(projectile.position.Y / 16f - (float)explosionRadius);
            //    int maxTileY = (int)(projectile.position.Y / 16f + (float)explosionRadius);
            //    if (minTileX < 0)
            //    {
            //        minTileX = 0;
            //    }
            //    if (maxTileX > Main.maxTilesX)
            //    {
            //        maxTileX = Main.maxTilesX;
            //    }
            //    if (minTileY < 0)
            //    {
            //        minTileY = 0;
            //    }
            //    if (maxTileY > Main.maxTilesY)
            //    {
            //        maxTileY = Main.maxTilesY;
            //    }

            //}
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            hitDirection = projectile.Center.X > target.Center.X ? -1 : 1;

            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (target.boss || projectile.penetrate == 997)
                Prime();

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Prime();
            return false;
        }

        public void Prime()
        {
            projectile.tileCollide = false;
            projectile.velocity = Vector2.Zero;
            // Set to transparent. This projectile technically lives as  transparent for about 3 frames
            projectile.alpha = 255;
            // change the hitbox size, centered about the original projectile center. This makes the projectile damage enemies during the explosion.
            projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
            projectile.width = 350;
            projectile.height = 350;
            projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
            projectile.timeLeft = 3;
        }
    }
}
