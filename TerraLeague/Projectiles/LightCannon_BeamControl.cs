using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.Achievements;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class LightCannon_BeamControl : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Light Cannon Beam Control");
        }

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.timeLeft = 300;
            projectile.friendly = false;
            projectile.ranged = true;
            projectile.alpha = 255;
            projectile.tileCollide = false;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            player.itemTime = 18;
            player.itemAnimation = 18;
            if (player.channel)
            {
                projectile.Center = player.Center + new Vector2(-16, -14) + new Vector2(80 * player.direction, 0).RotatedBy(player.itemRotation + player.fullRotation) + Main.OffsetsPlayerOnhand[player.bodyFrame.Y / 56];

                if (projectile.owner == Main.LocalPlayer.whoAmI)
                {
                    int dir = player.Center.X > Main.MouseWorld.X ? -1 : 1;
                    player.ChangeDir(dir);
                    projectile.ai[1] = (float)TerraLeague.CalcAngle(player.Center, Main.MouseWorld) - player.fullRotation;
                    projectile.netUpdate = true;
                }
                player.itemRotation = projectile.ai[1];

                for (int k = 0; k < 2 + 1; k++)
                {
                    float scale = 0.8f;
                    if (k % 2 == 1)
                    {
                        scale = 0.6f;
                    }

                    Vector2 postion = projectile.Center + ((float)Main.rand.NextDouble() * 6.28318548f).ToRotationVector2() * (12f - (float)(2 * 2));
                    Dust dust = Dust.NewDustDirect(postion - Vector2.One * 8f, 16, 16, 66, 0, 0, 0, default(Color), scale);
                    dust.velocity = Vector2.Normalize(projectile.Center - postion) * 1.5f * (10f - (float)2 * 2f) / 10f;
                    dust.noGravity = true;
                    dust.customData = player;
                }
                if (projectile.localAI[0] < 30)
                {
                    projectile.localAI[0]++;
                }
                else
                {
                    projectile.ai[0] = 1;
                    projectile.Kill();
                }
            }
            else
            {
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
            if (projectile.ai[0] != 0)
            {
                if (projectile.owner == Main.LocalPlayer.whoAmI)
                {
                    Player player = Main.player[projectile.owner];

                    float rot = projectile.ai[1] + (player.direction == -1 ? MathHelper.Pi : 0) + player.fullRotation;

                    Projectile.NewProjectileDirect(projectile.Center, new Vector2(10, 0).RotatedBy(rot), ProjectileType<LightCannon_Beam>(), projectile.damage, projectile.knockBack, projectile.owner);
                }

                TerraLeague.PlaySoundWithPitch(projectile.Center, 2, 72, -1f);
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
