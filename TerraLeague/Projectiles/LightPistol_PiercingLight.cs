using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.Achievements;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class LightPistol_PiercingLight : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Piercing Light");
        }

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.timeLeft = 300;
            projectile.friendly = false;
            projectile.ranged = true;
            projectile.tileCollide = false;
            projectile.alpha = 255;
            projectile.penetrate = -1;
            projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
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
                    Dust dust = Dust.NewDustDirect(dustBoxPosition, dustBoxWidth, dustBoxHeight, 66, 0f, 0f, 100, default(Color), 1 * (projectile.timeLeft/50f) + 1);
                    dust.noGravity = true;
                    dust.velocity *= 0.1f;
                    dust.velocity += projectile.velocity * 0.1f;
                    dust.position.X -= projectile.velocity.X / 3f * (float)i;
                    dust.position.Y -= projectile.velocity.Y / 3f * (float)i;
                }
            }
            else
            {
                //int dir = player.Center.X > projectile.Center.X ? -1 : 1;
                //player.ChangeDir(dir);

                projectile.Center = player.Center + new Vector2(0, -6) + new Vector2(38,0).RotatedBy(projectile.velocity.ToRotation());

                for (int k = 0; k < 2 + 1; k++)
                {
                    float scale = 0.8f;
                    if (k % 2 == 1)
                    {
                        scale = 0.6f;
                    }
                    
                    Vector2 position = projectile.Center + ((float)Main.rand.NextDouble() * 6.28318548f).ToRotationVector2() * (12f - (float)(2 * 2));
                    Dust dust = Dust.NewDustDirect(position - Vector2.One * 8f, 16, 16, 66, 0, 0, 0, default(Color), scale);
                    dust.velocity = Vector2.Normalize(projectile.Center - position) * 1.5f * (10f - (float)2 * 2f) / 10f;
                    dust.noGravity = true;
                    dust.customData = player;
                }

                projectile.localAI[0]++;
                if (projectile.localAI[0] > 20)
                {
                    projectile.ai[1] = 1;
                    projectile.friendly = true;
                    projectile.timeLeft = 50;
                    projectile.extraUpdates = 16;
                    //projectile.velocity = new Vector2(10, 0).RotatedBy(projectile.rotation);

                    TerraLeague.PlaySoundWithPitch(projectile.Center, 2, 12, -1);
                }
            }

            base.AI();
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
    }
}
