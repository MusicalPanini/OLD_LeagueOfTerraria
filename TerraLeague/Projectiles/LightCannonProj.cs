using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.Achievements;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class LightCannonProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Light Cannon");
        }

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.timeLeft = 300;
            projectile.friendly = false;
            projectile.ranged = true;
            projectile.alpha = 255;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            if ((int)projectile.ai[1] == 1)
            {
                Lighting.AddLight(projectile.Center, Color.White.ToVector3());
                for (int i = 0; i < 3; i++)
                {
                    Vector2 dustBoxPosition = new Vector2(projectile.position.X + 6, projectile.position.Y + 6);
                    int dustBoxWidth = projectile.width - 12;
                    int dustBoxHeight = projectile.height - 12;
                    Dust dust = Dust.NewDustDirect(dustBoxPosition, dustBoxWidth, dustBoxHeight, 66, 0f, 0f, 100, default(Color), 1 * (projectile.timeLeft/80f) + 1);
                    dust.noGravity = true;
                    dust.velocity *= 0.1f;
                    dust.velocity += projectile.velocity * 0.1f;
                    dust.position.X -= projectile.velocity.X / 3f * (float)i;
                    dust.position.Y -= projectile.velocity.Y / 3f * (float)i;
                }
            }
            else if (player.channel)
            {
                int dir = player.Center.X > Main.MouseWorld.X ? -1 : 1;
                player.ChangeDir(dir);
                player.itemRotation = (float)TerraLeague.CalcAngle(player.Center, Main.MouseWorld) - player.fullRotation;

                projectile.Center = player.Center + new Vector2(-16, -14) + new Vector2(80 * dir, 0).RotatedBy(player.itemRotation + player.fullRotation) + Main.OffsetsPlayerOnhand[player.bodyFrame.Y / 56];

                for (int k = 0; k < 2 + 1; k++)
                {
                    int num18 = 66;
                    float num19 = 0.8f;
                    if (k % 2 == 1)
                    {
                        num19 = 0.6f;
                    }
                    
                    Vector2 vector11 = projectile.Center + ((float)Main.rand.NextDouble() * 6.28318548f).ToRotationVector2() * (12f - (float)(2 * 2));
                    int num20 = Dust.NewDust(vector11 - Vector2.One * 8f, 16, 16, num18, 0, 0, 0, default(Color), 1f);
                    Main.dust[num20].velocity = Vector2.Normalize(projectile.Center - vector11) * 1.5f * (10f - (float)2 * 2f) / 10f;
                    Main.dust[num20].noGravity = true;
                    Main.dust[num20].scale = num19;
                    Main.dust[num20].customData = player;
                }

                projectile.ai[0]++;
                if (projectile.ai[0] > 30)
                {
                    projectile.ai[1] = 1;
                    projectile.friendly = true;
                    projectile.timeLeft = 80;
                    projectile.extraUpdates = 16;
                    projectile.velocity = TerraLeague.CalcVelocityToMouse(player.Center, 10);

                    Microsoft.Xna.Framework.Audio.SoundEffectInstance sound = Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 72, Terraria.Audio.SoundType.Sound), projectile.Center);
                    if (sound != null)
                        sound.Pitch = -1f;
                }
            }
            else
            {
                player.itemTime = 0;
                player.itemAnimation = 0;
                projectile.Kill();
            }

            base.AI();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
            if ((int)projectile.ai[1] == 1 && timeLeft > 0)
            {
                for (int i = 0; i < 12; i++)
                {
                    Dust dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 66, projectile.velocity.X * 0.25f, projectile.velocity.Y * 0.25f, 0);
                    dust.noGravity = true;
                    dust.scale = 1 * (timeLeft / 80f) + 1;
                }
            }
            base.Kill(timeLeft);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = height = 10;
            return true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }
    }
}
