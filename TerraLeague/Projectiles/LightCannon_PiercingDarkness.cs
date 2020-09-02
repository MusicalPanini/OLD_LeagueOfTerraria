using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.Achievements;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class LightCannon_PiercingDarkness : ModProjectile
    {
        bool[] hasHitPlayer = new bool[200];

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
            projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
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
                //int dir = player.MountedCenter.X > projectile.Center.X ? -1 : 1;
                //player.ChangeDir(dir);

                projectile.Center = player.MountedCenter + new Vector2(-16, -14) + new Vector2(80, 0).RotatedBy(projectile.velocity.ToRotation() + player.fullRotation) + Main.OffsetsPlayerOnhand[player.bodyFrame.Y / 56];

                for (int k = 0; k < 3; k++)
                {
                    int type = DustID.Smoke;
                    float scale = 0.8f;
                    if (k == 1)
                    {
                        type = 66;
                        scale = 1f;
                    }
                    
                    Vector2 position = projectile.Center + ((float)Main.rand.NextDouble() * 6.28318548f).ToRotationVector2() * (12f - (float)(2 * 2));
                    Dust dust = Dust.NewDustDirect(position - Vector2.One * 8f, 16, 16, type, 0, 0, 0, default(Color), scale);
                    dust.velocity = Vector2.Normalize(projectile.Center - position) * 1.5f * (10f - (float)2 * 2f) / 10f;
                    dust.noGravity = true;
                    dust.customData = player;
                }

                projectile.localAI[0]++;
                if (projectile.localAI[0] > 15)
                {
                    projectile.ai[1] = 1;
                    projectile.friendly = true;
                    projectile.timeLeft = 80;
                    projectile.extraUpdates = 16;
                    //projectile.velocity = new Vector2(10, 0).RotatedBy(projectile.rotation);

                    TerraLeague.PlaySoundWithPitch(projectile.Center, 2, 72, -1f);

                    if (projectile.owner == Main.LocalPlayer.whoAmI)
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
                {
                    if (!hasHitPlayer[player.whoAmI])
                    {
                        Main.player[projectile.owner].GetModPlayer<PLAYERGLOBAL>().SendHealPacket((int)projectile.ai[0], player.whoAmI, -1, projectile.owner);
                        hasHitPlayer[player.whoAmI] = true;
                    }
                }
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
            return true;
        }
    }
}
