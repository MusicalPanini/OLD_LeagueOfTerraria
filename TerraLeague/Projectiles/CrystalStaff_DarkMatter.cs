using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.Achievements;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class CrystalStaff_DarkMatter : ModProjectile
    {
        int radius = 100;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dark Matter");
        }

        public override void SetDefaults()
        {
            projectile.width = 34;
            projectile.height = 34;
            projectile.timeLeft = 100;
            projectile.penetrate = 100;
            projectile.friendly = false;
            projectile.magic = true;
            projectile.tileCollide = false;
            projectile.knockBack = 0;
            projectile.extraUpdates = 0;
            projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;

            projectile.alpha = 255;
        }

        public override void AI()
        {
                //if (projectile.ai[1] == 0f && !Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
                //{
                //    projectile.ai[1] = 1f;
                //    projectile.netUpdate = true;
                //}
                //if (projectile.ai[1] != 0f)
                //{
                //    projectile.tileCollide = true;
                //}

            if (projectile.ai[0] > 60)
            {
                projectile.rotation = MathHelper.Pi;

                for (int i = 0; i < 2; i++)
                {
                    Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 113, 0, 3, 0, default, 3f);
                    dust.noGravity = true;
                    dust.noLight = true;
                    dust.velocity *= 0.3f;
                }
            }
            else
            {
                TerraLeague.DustLine(projectile.Center, projectile.Center - (Vector2.UnitY * 1000), projectile.ai[0] % 2 == 0 ? 112 : 113, 0.2f, projectile.ai[0] / 45f);
                TerraLeague.DustBorderRing(radius, projectile.Center, projectile.ai[0] % 2 == 0 ? 112 : 113, default, projectile.ai[0] / 45f);

                projectile.ai[0]++;

                if (projectile.ai[0] > 60)
                {
                    projectile.velocity.Y = 25;
                    projectile.position.Y -= 1000;
                    projectile.tileCollide = false;
                    projectile.friendly = true;
                    projectile.extraUpdates = 4;
                    projectile.timeLeft = 1000 / 25;
                    //projectile.ai[1] = 0f;
                }
            }

            if (projectile.timeLeft == 1 && projectile.ai[1] == 0)
            {
                Prime();
                projectile.ai[1] = 1;
            }

            base.AI();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            hitDirection = projectile.Center.X > target.Center.X ? -1 : 1;

            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override bool PreKill(int timeLeft)
        {
           
            return base.PreKill(timeLeft);
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.DD2_ExplosiveTrapExplode.WithVolume(1f), projectile.position);

            Dust dust;
            for (int i = 0; i < 30; i++)
            {
                dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 31, 0f, 0f, 100, new Color(110,70,200), 2f);
                dust.velocity *= 1.4f;
            }
            for (int i = 0; i < 20; i++)
            {
                dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 112, 0f, 0f, 100, default, 2f);
                dust.noGravity = true;
                dust.velocity *= 1f;

                dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 113, 0f, 0f, 100, default(Color), 3f);
                dust.noGravity = true;
                dust.velocity *= 2f;
            }

            projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
            projectile.width = 10;
            projectile.height = 10;
            projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
            
            base.Kill(timeLeft);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (projectile.ai[0] > 60)
                Prime();
            return false;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = 10;
            height = 10;
            return true;
        }

        public void Prime()
        {
            projectile.velocity = Vector2.Zero;
            projectile.tileCollide = false;
            projectile.knockBack = 8;
            projectile.alpha = 255;
            projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
            projectile.width = radius * 2;
            projectile.height = radius * 2;
            projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
            projectile.timeLeft = 3;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            //Texture2D texture = Main.projectileTexture[projectile.type];
            //spriteBatch.Draw
            //(
            //    texture,
            //    new Vector2
            //    (
            //        projectile.position.X - Main.screenPosition.X + projectile.width * 0.5f,
            //        projectile.position.Y - Main.screenPosition.Y + projectile.height - texture.Height * 0.5f
            //    ),
            //    new Rectangle(0, 0, texture.Width, texture.Height),
            //    Color.White,
            //    projectile.rotation,
            //    texture.Size() * 0.5f,
            //    projectile.scale,
            //    SpriteEffects.None,
            //    0f
            //);
        }
    }
}
