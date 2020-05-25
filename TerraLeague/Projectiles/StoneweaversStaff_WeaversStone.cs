using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Achievements;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class StoneweaversStaff_WeaversStone : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Weaver's Stone");
        }

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.timeLeft = 180;
            projectile.penetrate = 1;
            projectile.friendly = true;
            projectile.magic = true;
        }

        public override void AI()
        {
            if ((int)projectile.ai[0] == 0)
            {
                if (projectile.timeLeft < 150)
                {
                    projectile.velocity.Y += 0.4f;
                    projectile.velocity.X *= 0.97f;
                }
            }
            
            if ((int)projectile.ai[0] == 1)
            {
                if (projectile.timeLeft == 119)
                    projectile.velocity = new Vector2(projectile.velocity.X * -0.2f, -5);

                projectile.velocity.Y += 0.4f;
                projectile.velocity.X *= 0.97f;

                if (projectile.timeLeft == 1)
                    Prime();
            }

            if (projectile.velocity.Y > 16)
                projectile.velocity.Y = 16;

            projectile.rotation += (Math.Abs(projectile.velocity.X) + Math.Abs(projectile.velocity.Y)) * 0.03f * (float)projectile.direction;

            if (projectile.velocity.Length() > 2)
                Dust.NewDustDirect(projectile.position, 16, 16, 4, 0f, 0f, 100, new Color(255, 125, 0), 0.7f);
            
            base.AI();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if ((int)projectile.ai[0] == 0)
            {
                projectile.penetrate = 1000;
                projectile.netUpdate = true;
                projectile.ai[0] = 1;
                projectile.friendly = false;
                projectile.timeLeft = 120;
            }
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
            if ((int)projectile.ai[0] == 1)
            {
                Main.PlaySound(new LegacySoundStyle(2, 14), projectile.position);
                for (int g = 0; g < 4; g++)
                {
                    Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                }

                Dust dust;
                for (int i = 0; i < 20; i++)
                {
                    dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 4, 0f, 0f, 100, new Color(255, 125, 0), 1f);
                    dust.velocity *= 2f;

                    dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 4, 0f, 0f, 100, new Color(255, 125, 0), 1.5f);
                    dust.velocity *= 1.5f;
                }
            }
            else if ((int)projectile.ai[0] == 0)
            {
                Main.PlaySound(SoundID.Dig, projectile.Center);

                for (int i = 0; i < 12; i++)
                {
                    Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 4, projectile.oldVelocity.X * 0.25f, projectile.oldVelocity.Y * 0.25f, 0, new Color(255, 125, 0));
                }
            }
            base.Kill(timeLeft);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            if ((int)projectile.ai[0] == 0)
                fallThrough = true;
            else
                fallThrough = false;

            width = height = 10;
            return true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if ((int)projectile.ai[0] == 0)
                return true;
            return false;
        }

        public void Prime()
        {
            projectile.damage = (int)(projectile.damage * 1.5);
            projectile.knockBack = 6;
            projectile.friendly = true;
            projectile.velocity = Vector2.Zero;
            projectile.tileCollide = false;
            projectile.alpha = 255;
            projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
            projectile.width = 150;
            projectile.height = 150;
            projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
        }
    }
}
