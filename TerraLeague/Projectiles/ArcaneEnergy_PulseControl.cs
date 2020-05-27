using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.Achievements;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class ArcaneEnergy_PulseControl : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Arcane Energy Pulse Control");
        }

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.timeLeft = 300;
            projectile.friendly = false;
            projectile.magic = true;
            projectile.alpha = 255;
            projectile.tileCollide = false;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            player.slow = true;
            if (projectile.owner == Main.LocalPlayer.whoAmI)
            {
                int dir = player.Center.X > Main.MouseWorld.X ? -1 : 1;
                player.ChangeDir(dir);
                projectile.ai[1] = (float)TerraLeague.CalcAngle(player.Center, Main.MouseWorld) - player.fullRotation;
                projectile.netUpdate = true;
            }

            if (projectile.soundDelay == 0 && projectile.localAI[0] < 180)
            {
                projectile.soundDelay = 30;
                Microsoft.Xna.Framework.Audio.SoundEffectInstance sound = Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 15), projectile.position);
                if (sound != null)
                    sound.Pitch = -0.5f + projectile.localAI[0]/360f;
            }

            if (player.channel)
            {
                projectile.Center = player.MountedCenter;

                for (int k = 0; k < 2 + 1; k++)
                {
                    float scale = projectile.localAI[0] / 60f;
                    if (k % 2 == 1)
                        scale = projectile.localAI[0] / 75f;

                    Vector2 postion = (player.Top + new Vector2(0, -16)) + ((float)Main.rand.NextDouble() * 6.28318548f).ToRotationVector2() * (12f - (float)(2 * 2));
                    Dust dust = Dust.NewDustDirect(postion - Vector2.One * 8f, 16, 16, 113, 0, 0, 0, new Color(255, 0, 0), scale);
                    dust.velocity = Vector2.Normalize(projectile.Center - postion) * 1.5f * (10f - (float)2 * 2f) / 10f;
                    dust.noGravity = true;
                    dust.customData = player;
                }

                if (projectile.localAI[0] < 180)
                    projectile.localAI[0]++;
                player.itemTime = 24;
                player.itemAnimation = 24;
            }
            else
            {
                player.itemTime = 24;
                player.itemAnimation = 24;
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
            Player player = Main.player[projectile.owner];

            float rot = projectile.ai[1] + (player.direction == -1 ? MathHelper.Pi : 0) + player.fullRotation;

            Projectile.NewProjectileDirect(projectile.Center, new Vector2(10, 0).RotatedBy(rot), ModContent.ProjectileType<ArcaneEnergy_Pulse>(), (int)(projectile.damage * (1 + projectile.localAI[0]/180f)), projectile.knockBack, projectile.owner, projectile.localAI[0]);

            Microsoft.Xna.Framework.Audio.SoundEffectInstance sound = Main.PlaySound(new Terraria.Audio.LegacySoundStyle(3, 53), projectile.position);
            if (sound != null)
                sound.Pitch -= 0.5f;

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
