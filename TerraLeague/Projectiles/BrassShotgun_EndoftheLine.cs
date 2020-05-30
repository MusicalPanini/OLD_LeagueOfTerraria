using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    class BrassShotgun_EndoftheLine : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("End of the Line");
        }

        public override void SetDefaults()
        {
            projectile.width = 20;
            projectile.height = 20;
            projectile.alpha = 0;
            projectile.timeLeft = 600;
            projectile.penetrate = -1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.tileCollide = true;
            projectile.ignoreWater = false;
            projectile.ranged = true;
            projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void AI()
        {
            if ((int)projectile.ai[1] == 0)
            {
                projectile.rotation += projectile.velocity.X * 0.05f;
                Lighting.AddLight(projectile.position, 0.5f, 0.45f, 0.30f);
                projectile.velocity.Y += 0.4f;

                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.Smoke);
                dust.alpha = 100;
                dust.velocity /= 3;
                dust.noGravity = true;
            }
            else
            {
                Player player = Main.player[projectile.owner];

                if (projectile.timeLeft == 1000)
                {
                    //Reset();
                }
                else if (projectile.timeLeft == 1000 - 7)
                {
                    Prime(20);
                }

                if (projectile.localAI[0] == 0f)
                {
                    AdjustMagnitude(ref projectile.velocity);
                    projectile.localAI[0] = 1f;
                }

                
                Vector2 move = player.MountedCenter - projectile.Center;
                AdjustMagnitude(ref move);
                projectile.velocity = (10 * projectile.velocity + move) / 11.5f;
                AdjustMagnitude(ref projectile.velocity);


                    if (projectile.Hitbox.Intersects(player.Hitbox) || (int)projectile.ai[0] > 10)
                    {
                        projectile.Kill();
                    }

                for (int i = 0; i < 2; i++)
                {
                    Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.Fire, 0, 0, 0, default(Color), 3f);
                    dust.noGravity = true;
                    dust.noLight = true;

                    dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.Fire, 0, 3, 0, default(Color), 2f);
                    dust.noLight = true;

                }
            }

            base.AI();
        }
        
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if ((int)projectile.ai[1] == 0)
            {
                projectile.ai[1] = 1;
                projectile.tileCollide = false;
                projectile.timeLeft = 1000;
                //projectile.damage *= 2;
                projectile.alpha = 255;
                projectile.width = 10;
                projectile.height = 10;
                projectile.velocity = Vector2.Zero;
                projectile.extraUpdates = 1; ;
                Prime(200);
            }

            return false;
        }

       
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override void Kill(int timeLeft)
        {

        }

        public void Reset()
        {
            projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
            projectile.width = 10;
            projectile.height = 10;
            projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
        }

        public void Prime(int size)
        {
            projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
            projectile.width = size;
            projectile.height = size;
            projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
            projectile.timeLeft = 1001;

            Main.PlaySound(new LegacySoundStyle(2, 14), projectile.position);

            Dust dust;
            for (int i = 0; i < (size == 200 ? 40f : 20f); i++)
            {
                dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 31, 0f, 0f, 100, default(Color), size == 200 ? 2f : 1f);
                dust.velocity *= 0.5f;

            }
            for (int i = 0; i < 50; i++)
            {
                dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), size == 200 ? 4f : 2f);
                dust.noGravity = true;
                dust.velocity *= 3f;
                dust.color = new Color(255, 0, 220);

                dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), size == 200 ? 3f : 1.5f);
                dust.color = new Color(255, 0, 220);
                dust.noGravity = true;
            }

            projectile.ai[0]++;
        }

        private void AdjustMagnitude(ref Vector2 vector)
        {
            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > 15f)
            {
                vector *= 15f / magnitude;
            }
        }
    }
}
