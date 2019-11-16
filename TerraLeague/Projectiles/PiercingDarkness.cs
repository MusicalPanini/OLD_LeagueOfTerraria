using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.Achievements;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class PiercingDarkness : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Piercing Darkness");
        }

        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 32;
            projectile.timeLeft = 300;
            projectile.friendly = false;
            projectile.tileCollide = false;
            projectile.ranged = true;
            projectile.alpha = 255;
            projectile.penetrate = -1;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            if ((int)projectile.ai[1] == 1)
            {
                Dust dust;
                for (int i = 0; i < 3; i++)
                {
                    Vector2 dustBoxPosition = new Vector2(projectile.position.X + 6, projectile.position.Y + 6);
                    int dustBoxWidth = projectile.width - 12;
                    int dustBoxHeight = projectile.height - 12;
                    dust = Dust.NewDustDirect(dustBoxPosition, dustBoxWidth, dustBoxHeight, DustID.Smoke, 0f, 0f, 100, default(Color), 2);
                    dust.noGravity = true;
                    dust.velocity *= 0.1f;
                    dust.velocity += projectile.velocity * 0.1f;
                    dust.position.X -= projectile.velocity.X / 3f * (float)i;
                    dust.position.Y -= projectile.velocity.Y / 3f * (float)i;
                }

                for (int i = 0; i < 3; i++)
                {
                    Vector2 pos = new Vector2(projectile.Center.X + (-4 + (i * 4)), projectile.Center.Y);
                    dust = Dust.NewDustPerfect(pos, 188);
                    dust.velocity /= 10;
                    dust.scale = 0.75f;
                    dust.alpha = 100;
                }

                for (int i = 0; i < 200; i++)
                {
                    Player healTarget = Main.player[i];
                    if (projectile.Hitbox.Intersects(healTarget.Hitbox) && i != projectile.owner)
                    {
                        HitPlayer(healTarget);
                    }
                }

            }
            else
            {
                int dir = player.Center.X > Main.MouseWorld.X ? -1 : 1;
                player.ChangeDir(dir);

                projectile.Center = player.Center + new Vector2(-16, -14) + new Vector2(80, 0).RotatedBy(projectile.rotation) + Main.OffsetsPlayerOnhand[player.bodyFrame.Y / 56];

                for (int k = 0; k < 2 + 1; k++)
                {
                    int num18 = DustID.Smoke;
                    float num19 = 0.8f;
                    if (k == 1)
                    {
                        num18 = 66;
                        num19 = 1f;
                    }
                    
                    Vector2 vector11 = projectile.Center + ((float)Main.rand.NextDouble() * 6.28318548f).ToRotationVector2() * (12f - (float)(2 * 2));
                    int num20 = Dust.NewDust(vector11 - Vector2.One * 8f, 16, 16, num18, 0, 0, 0, default(Color), 1f);
                    Main.dust[num20].velocity = Vector2.Normalize(projectile.Center - vector11) * 1.5f * (10f - (float)2 * 2f) / 10f;
                    Main.dust[num20].noGravity = true;
                    Main.dust[num20].scale = num19;
                    Main.dust[num20].customData = player;
                }

                projectile.localAI[0]++;
                if (projectile.localAI[0] > 15)
                {
                    projectile.ai[1] = 1;
                    projectile.friendly = true;
                    projectile.timeLeft = 80;
                    projectile.extraUpdates = 16;
                    projectile.velocity = new Vector2(10, 0).RotatedBy(projectile.rotation);

                    Microsoft.Xna.Framework.Audio.SoundEffectInstance sound = Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 72, Terraria.Audio.SoundType.Sound), projectile.Center);
                    if (sound != null)
                        sound.Pitch = -1f;

                    player.GetModPlayer<PLAYERGLOBAL>().lifeToHeal += (int)projectile.ai[0];
                }
            }

            base.AI();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public void HitPlayer(Player player)
        {
            Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 29), player.Center);

            projectile.netUpdate = true;
            if (projectile.owner == Main.LocalPlayer.whoAmI)
            {
                if (player.whoAmI != projectile.owner)
                    player.GetModPlayer<PLAYERGLOBAL>().SendHealPacket((int)projectile.ai[0], player.whoAmI, -1, projectile.owner);
            }
        }

        public override void Kill(int timeLeft)
        {
            if ((int)projectile.ai[1] == 1 && timeLeft > 0)
            {
                for (int i = 0; i < 12; i++)
                {
                    Dust dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.Smoke, projectile.velocity.X * 0.25f, projectile.velocity.Y * 0.25f, 0);
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
            
            Main.PlaySound(SoundID.Item1, projectile.position);
            return true;
        }
    }
}
