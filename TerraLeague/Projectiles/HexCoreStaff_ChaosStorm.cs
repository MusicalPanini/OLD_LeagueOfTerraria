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
    public class HexCoreStaff_ChaosStorm : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chaos Storm");
        }

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 24;
            projectile.alpha = 0;
            projectile.penetrate = -1;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.magic = true;

            projectile.tileCollide = true;
            projectile.ignoreWater = false;

            projectile.scale = 1;
            projectile.timeLeft = 3600;
        }

        public override void AI()
        {
            if (projectile.soundDelay == 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 226, 0, 0, 0, default(Color), 2);
                    dust.noGravity = true;
                }
            }
            projectile.soundDelay = 100;

            if (Main.rand.Next(3) == 0)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, 8, 226, 0, 0, 0, default(Color), 0.5f);
            }

            if (!Main.player[projectile.owner].channel && projectile.timeLeft < 3600 && projectile.owner == Main.LocalPlayer.whoAmI || projectile.alpha != 0 || Main.player[projectile.owner].dead || !Main.player[projectile.owner].active)
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
            if (projectile.Distance(player.MountedCenter) > 1000)
                projectile.Kill();

            Lighting.AddLight(projectile.Center, 0, 1, 1);

            if (Main.LocalPlayer.whoAmI == player.whoAmI)
            {
                projectile.localAI[0]++;
                if ((int)projectile.localAI[0] == 60)
                {
                    //Projectile.NewProjectileDirect(projectile.Center, Vector2.Zero, ProjectileType<HexCoreStaff_Storm>(), projectile.damage, projectile.knockBack, projectile.owner);\
                    TerraLeague.DustBorderRing(224, projectile.Center, 226, default, 0.75f);
                    List<int> npcs = TerraLeague.GetAllNPCsInRange(projectile.Center, 224, true);
                    for (int i = 0; i < npcs.Count; i++)
                    {
                        NPC npc = Main.npc[npcs[i]];
                        if (!npc.dontTakeDamage)
                        {
                            Projectile.NewProjectileDirect(npc.Center, Vector2.Zero, ProjectileType<HexCoreStaff_ChaosStormZap>(), projectile.damage, projectile.knockBack, projectile.owner, npc.whoAmI);
                            TerraLeague.DustLine(projectile.Center, npc.Center, 160, 1, 1);
                        }
                    }

                    if (npcs.Count > 0)
                        TerraLeague.PlaySoundWithPitch(projectile.Center, 3, 53, 0);

                    projectile.localAI[0] = 0;
                }
            }

            

            if (projectile.ai[0] == 0 && projectile.owner == Main.LocalPlayer.whoAmI)
            {
                float num114 = 6;

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

                    // movement
                    projectile.velocity.X = num115 * Vector2.Distance(projectile.position, Main.MouseWorld) / 1000;
                    projectile.velocity.Y = num116 * Vector2.Distance(projectile.position, Main.MouseWorld) / 1000;

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

                    projectile.velocity.X = num115 * Vector2.Distance(projectile.position, Main.MouseWorld) / 1000;
                    projectile.velocity.Y = num116 * Vector2.Distance(projectile.position, Main.MouseWorld) / 1000;
                }
            }
            projectile.timeLeft = 3000;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
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

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
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

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}
