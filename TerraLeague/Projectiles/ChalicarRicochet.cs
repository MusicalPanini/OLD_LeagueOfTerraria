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
    public class ChalicarRicochet : ModProjectile
    {
        public int[] HaveHit = new int[] { -1,-1,-1,-1,-1,-1,-1,-1,-1, -1, -1, -1, -1, -1, -1 };
        public int hitCounter = 15;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chalicar Ricochet");
        }

        public override void SetDefaults()
        {
            projectile.width = 36;
            projectile.height = 36;
            projectile.timeLeft = 60;
            projectile.penetrate = 15;
            projectile.friendly = true;
            projectile.ranged = true;
            projectile.hostile = false;
            projectile.tileCollide = true;
            projectile.ignoreWater = false;
            projectile.alpha = 0;
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
            if (projectile.penetrate != 15)
                projectile.tileCollide = false;

            if (projectile.soundDelay == 0 && projectile.type != 383)
            {
                projectile.soundDelay = 8;
                Main.PlaySound(SoundID.Item7, projectile.position);
            }

            if ((int)projectile.ai[1] == 1)
            {
                if ((int)projectile.ai[0] == -1 && projectile.owner == Main.LocalPlayer.whoAmI)
                {
                    projectile.ai[0] = FindNewTarget();
                }
                if ((int)projectile.ai[0] != -1)
                {
                    NPC npc = Main.npc[(int)projectile.ai[0]];

                    //Get the shoot trajectory from the projectile and target
                    float shootToX = npc.Center.X - projectile.Center.X;
                    float shootToY = npc.Center.Y - projectile.Center.Y;
                    float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

                    //Divide the factor, 3f, which is the desired velocity
                    if (distance != 0)
                        distance = 2f / distance;

                    //Multiply the distance by a multiplier if you wish the projectile to have go faster
                    shootToX *= distance * 3;
                    shootToY *= distance * 3;

                    //Set the velocities to the shoot values
                    projectile.velocity.X = shootToX;
                    projectile.velocity.Y = shootToY;
                }
                else if (projectile.owner == Main.LocalPlayer.whoAmI)
                {
                    projectile.Kill();
                }
            }

            projectile.rotation += 0.3f * (float)projectile.direction;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if ((bool)CanHitNPC(target))
            {
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
                projectile.ai[1] = 1;
                projectile.tileCollide = false;

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
                //If the npc is hostile
                if (!target.townNPC && !HaveHit.Contains(target.whoAmI) && !target.dontTakeDamage)
                {
                    //Get the shoot trajectory from the projectile and target
                    float shootToX = target.Center.X - projectile.Center.X;
                    float shootToY = target.Center.Y - projectile.Center.Y;
                    float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

                    //If the distance between the live targeted npc and the projectile is less than 480 pixels
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
            for (int i = 0; i < 12; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 11, projectile.oldVelocity.X * 0.25f, projectile.oldVelocity.Y * 0.25f, 157, new Color(234, 255, 0));
            }

            base.Kill(timeLeft);
        }
    }
}
