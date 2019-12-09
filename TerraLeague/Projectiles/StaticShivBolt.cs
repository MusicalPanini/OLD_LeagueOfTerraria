using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class StaticShivBolt : ModProjectile
    {
        public int[] HaveHit = new int[] { -1,-1,-1,-1,-1,-1 };
        public int hitCounter = 6;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Energized Bolt");
            Main.projFrames[projectile.type] = 5;
        }

        public override void SetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.timeLeft = 302;
            projectile.penetrate = 6;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.alpha = 255;
            projectile.extraUpdates = 8;
            projectile.netImportant = true;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            if (projectile.owner == Main.LocalPlayer.whoAmI)
            for (int i = 0; i < HaveHit.Length; i++)
            {
                writer.Write(HaveHit[i]);
            }

            base.SendExtraAI(writer);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            for (int i = 0; i < HaveHit.Length; i++)
            {
                HaveHit[i] = reader.ReadInt32();
            }

            base.ReceiveExtraAI(reader);
        }

        public override void AI()
        {
            if (projectile.timeLeft == 302)
            {
                if ((int)projectile.ai[1] == 1)
                {
                    HaveHit = new int[] { -1, -1, -1, -1, -1, -1 , -1, -1, -1};
                    hitCounter = 9;
                    projectile.penetrate = 9;
                }
            }

            if (projectile.timeLeft == 300)
            {
                Main.PlaySound(new LegacySoundStyle(3, 53), projectile.position);
            }

            if ((int)projectile.ai[0] == -1 && projectile.owner == Main.LocalPlayer.whoAmI)
            {
                projectile.ai[0] = FindNewTarget();
            }
            if ((int)projectile.ai[0] != -1)
            {
                NPC npc = Main.npc[(int)projectile.ai[0]];

                if (!npc.active)
                {
                    projectile.ai[0] = FindNewTarget();
                }
                else
                {
                    float shootToX = npc.Center.X - projectile.Center.X;
                    float shootToY = npc.Center.Y - projectile.Center.Y;
                    float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

                    if (distance != 0)
                        distance = 2f / distance;

                    shootToX *= distance * 5;
                    shootToY *= distance * 5;

                    projectile.velocity.X = shootToX;
                    projectile.velocity.Y = shootToY;
                }
            }
            else if (projectile.owner == Main.LocalPlayer.whoAmI)
            {
                projectile.Kill();
            }
            for (int i = 0; i < 4; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 261, 0, 0, 0, new Color(255, 255, 0, 150), 1f);
                dust.velocity *= i == 3? 3 : 0.3f;
                dust.scale = i == 4 ? 1.25f : 1;
                dust.noGravity = true;
            }

            Lighting.AddLight(projectile.position, 1f, 1f, 0f);
            
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if ((bool)CanHitNPC(target))
            {
                target.immune[projectile.owner] = 2;
                projectile.netUpdate = true;
                projectile.timeLeft = 301;
                for (int i = 0; i < HaveHit.Length; i++)
                {
                    if (HaveHit[i] == -1)
                    {
                        HaveHit[i] = target.whoAmI;
                        break;
                    }
                }

                projectile.ai[0] = FindNewTarget();
                hitCounter--;

                if (hitCounter <= 0 || projectile.ai[0] == -1)
                {
                    projectile.Kill();
                }
                base.OnHitNPC(target, damage, knockback, crit);
            }
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (!HaveHit.Contains((int)projectile.ai[0]) && !target.townNPC)
                return true;
            else
                return false;
        }

        public int FindNewTarget()
        {
            projectile.netUpdate = true;

            NPC closest = null;
            float cDistance = 99999;
            for (int i = 0; i < 200; i++)
            {
                NPC target = Main.npc[i];
                if (!target.townNPC && !HaveHit.Contains(target.whoAmI) && !target.dontTakeDamage)
                {
                    float shootToX = target.Center.X - projectile.Center.X;
                    float shootToY = target.Center.Y - projectile.Center.Y;
                    float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

                    if (distance < 480f && !target.friendly && target.active && distance < cDistance)
                    {
                        closest = target;
                        cDistance = distance;
                    }
                }
            }
            if (closest != null)
            {
                closest.immune[projectile.owner] = 0;
                return closest.whoAmI;
            }
            else
            {
                return -1;
            }
        }

        public override void Kill(int timeLeft)
        {
            base.Kill(timeLeft);
        }
    }
}
