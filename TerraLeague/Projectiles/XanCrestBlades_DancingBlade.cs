using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class XanCrestBlades_DancingBlade : ModProjectile
    {
        List<int> iFrames = new List<int>();
        List<Vector2> positions = new List<Vector2>();
        List<float> angles = new List<float>();
        List<int> damage = new List<int>();
        int baseDamage = 0;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 3;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
            DisplayName.SetDefault("Dancing Blade");
        }

        public override void SetDefaults()
        {
            projectile.width = 26;
            projectile.height = 62;
            projectile.alpha = 0;
            projectile.penetrate = -1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.minion = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.scale = 1;
            projectile.tileCollide = false;
            projectile.timeLeft = 3600;
        }

        public override void AI()
        {
            if (/*Main.mouseLeftRelease */ !Main.player[projectile.owner].channel && projectile.timeLeft < 3600 && projectile.owner == Main.LocalPlayer.whoAmI || projectile.alpha != 0 || Main.player[projectile.owner].dead || !Main.player[projectile.owner].active)
            {
                projectile.alpha += 20;
                if (projectile.alpha > 250)
                {
                    DeadMode();
                }
            }

            Player player = Main.player[projectile.owner];
            projectile.netUpdate = true;
            //player.itemTime = 5;

            if (projectile.ai[0] == 0 && projectile.owner == Main.LocalPlayer.whoAmI)
            {
                if (projectile.timeLeft == 3600)
                    baseDamage = projectile.damage;

                float num114 = 14;

                Vector2 vector10 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
                float num115 = (float)Main.mouseX + Main.screenPosition.X - vector10.X;
                float num116 = (float)Main.mouseY + Main.screenPosition.Y - vector10.Y;
                if (Main.player[projectile.owner].gravDir == -1f)
                {
                    num116 = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY - vector10.Y;
                }
                float num117 = (float)Math.Sqrt((double)(num115 * num115 + num116 * num116));
                if (num117 > num114)
                {
                    num117 = num114 / num117;
                    num115 *= num117;
                    num116 *= num117;
                    int num118 = (int)(num115 * 1000f);
                    int num119 = (int)(projectile.velocity.X * 1000f);
                    int num120 = (int)(num116 * 1000f);
                    int num121 = (int)(projectile.velocity.Y * 1000f);
                    if (num118 != num119 || num120 != num121)
                    {
                        projectile.netUpdate = true;
                    }


                    if (Vector2.Distance(projectile.position, Main.MouseWorld) < 600)
                    {
                        projectile.velocity.X = num115 * Vector2.Distance(projectile.position, Main.MouseWorld) / 600;
                        projectile.velocity.Y = num116 * Vector2.Distance(projectile.position, Main.MouseWorld) / 600;
                    }
                    else
                    {
                        projectile.velocity.X = num115;
                        projectile.velocity.Y = num116;
                    }
                }
                else
                {
                    int num122 = (int)(num115 * 1000f);
                    int num123 = (int)(projectile.velocity.X * 1000f);
                    int num124 = (int)(num116 * 1000f);
                    int num125 = (int)(projectile.velocity.Y * 1000f);
                    if (num122 != num123 || num124 != num125)
                    {
                        projectile.netUpdate = true;
                    }

                    if (Vector2.Distance(projectile.position, Main.MouseWorld) < 600)
                    {
                        projectile.velocity.X = num115 * Vector2.Distance(projectile.position, Main.MouseWorld) / 600;
                        projectile.velocity.Y = num116 * Vector2.Distance(projectile.position, Main.MouseWorld) / 600;
                    }
                    else
                    {
                        projectile.velocity.X = num115;
                        projectile.velocity.Y = num116;
                    }
                }

                
            }

            if ((int)projectile.ai[0] == 0)
            {
                projectile.damage = (int)((projectile.velocity.Length() / 14) * baseDamage);
                projectile.rotation = (projectile.velocity.X / 8);
                projectile.ai[1] = 12 - (int)projectile.velocity.Length() / 2;
                positions.Add(projectile.Center);
                angles.Add(projectile.rotation);
                damage.Add(projectile.damage);
                iFrames.Add((int)projectile.ai[1]);

                Projectile proj;
                int projOwned = player.ownedProjectileCounts[ProjectileType<XanCrestBlades_DancingBlade>()];

                for (int i = 1; i <= projOwned; i++)
                {
                    if (positions.Count >= (6 * i))
                    {
                        proj = Main.projectile.Where(x => (int)x.ai[0] == i && x.owner == projectile.owner && projectile.type == x.type).First();
                        proj.Center = positions[positions.Count - (6 * i)];
                        proj.rotation = angles[angles.Count - (6 * i)];
                        proj.damage = damage[damage.Count - (6 * i)];
                        proj.ai[1] = iFrames[iFrames.Count - (6 * i)];
                    }


                    if (positions.Count >= 6 * projOwned)
                    {
                        positions.RemoveAt(0);
                        angles.RemoveAt(0);
                        damage.RemoveAt(0);
                        iFrames.RemoveAt(0);
                    }
                }
            }

            if (projectile.rotation < 0)
                projectile.spriteDirection = 1;
            else if (projectile.rotation > 0)
                projectile.spriteDirection = -1;

            projectile.timeLeft = 3000;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {

            target.immune[projectile.owner] = (int)projectile.ai[1]; 
            
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            //if (projectile.ai[0] == 1f)
            //{
            //    crit = true;
            //}

            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override void Kill(int timeLeft)
        {
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = height = 10;
            return true;
        }

        public void DeadMode()
        {
            projectile.Kill();
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
            for (int k = 0; k < projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
                spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, 1, projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            }
            return true;
        }
    }
}
