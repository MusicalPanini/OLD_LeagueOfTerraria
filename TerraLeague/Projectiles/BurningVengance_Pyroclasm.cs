using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TerraLeague.Buffs;
using TerraLeague.NPCs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class BurningVengance_Pyroclasm : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pyroclasm");
        }

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.timeLeft = 300;
            projectile.penetrate = 10;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.magic = true;
            projectile.alpha = 255;
            projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }


        public override void AI()
        {
            if (projectile.timeLeft > 300)
            {
                if (projectile.timeLeft == 314)
                {
                    TerraLeague.PlaySoundWithPitch(projectile.Center, 2, 45, -0.5f);
                }

                projectile.friendly = false;

                if ((int)projectile.ai[0] == -1)
                {
                    projectile.velocity = Vector2.Zero;
                }
                else
                {
                    NPC npc = Main.npc[(int)projectile.ai[0]];

                    projectile.Center = npc.Center;
                }

                if (projectile.timeLeft == 301)
                {
                    projectile.ai[1] = FindNewTarget();

                    if ((int)projectile.ai[1] == -1)
                        projectile.Kill();
                    else
                        projectile.ai[0] = (int)projectile.ai[1];
                }
            }
            else
            {
                projectile.friendly = true;

                if ((int)projectile.ai[0] >= 0)
                {

                    NPC npc = Main.npc[(int)projectile.ai[0]];

                    if (!npc.active && projectile.owner == Main.LocalPlayer.whoAmI)
                    {
                        projectile.ai[0] = FindNewTarget();

                        if (projectile.ai[0] == -1)
                        {
                            projectile.Kill();
                            return;
                        }
                    }

                    float MaxSpeed = 18;

                    float XDist = (float)npc.Center.X - projectile.Center.X;
                    float YDist = (float)npc.Center.Y - projectile.Center.Y;

                    float TrueDist = (float)System.Math.Sqrt((double)(XDist * XDist + YDist * YDist));
                    if (TrueDist > MaxSpeed)
                    {
                        TrueDist = MaxSpeed / TrueDist;
                        XDist *= TrueDist;
                        YDist *= TrueDist;
                        int num118 = (int)(XDist * 1000f);
                        int num119 = (int)(projectile.velocity.X * 1000f);
                        int num120 = (int)(YDist * 1000f);
                        int num121 = (int)(projectile.velocity.Y * 1000f);
                        if (num118 != num119 || num120 != num121)
                        {
                            projectile.netUpdate = true;
                        }

                        if (projectile.timeLeft > 270)
                        {
                            projectile.velocity.X = XDist * (1 - ((projectile.timeLeft - 270) / 30f));
                            projectile.velocity.Y = YDist * (1 - ((projectile.timeLeft - 270) / 30f));
                        }
                        else
                        {
                            projectile.velocity.X = XDist;
                            projectile.velocity.Y = YDist;
                        }
                    }
                }
                else
                {
                    projectile.ai[0] = FindNewTarget();

                    if (projectile.ai[0] == -1)
                    {
                        projectile.Kill();
                        return;
                    }
                }

            }

            for (int i = 0; i < 2; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.Fire, 0, 0, 0, default(Color), 4f);
                dust.noGravity = true;
                dust.noLight = true;

                dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.Fire, 0, 3, 0, default(Color), 1f);
                dust.noLight = true;

            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.netUpdate = true;

            if (target.HasBuff(BuffID.OnFire))
            {
                target.AddBuff(BuffType<Ablaze>(), 600);
                target.DelBuff(target.FindBuffIndex(BuffID.OnFire));
            }
            else if (target.HasBuff(BuffType<Ablaze>()))
            {
                target.AddBuff(BuffType<Ablaze>(), 600);
            }
            else
            {
                target.AddBuff(BuffID.OnFire, 1200);
            }

            projectile.friendly = false;
            
            projectile.timeLeft = 315;

            if (target.life <= 0)
                projectile.ai[0] = -1;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (target.GetGlobalNPC<TerraLeagueNPCsGLOBAL>().ablaze)
            {
                damage *= 2;
                Projectile.NewProjectileDirect(target.Center, Vector2.Zero, ProjectileType<BurningVengance_PyroclasmExplosion>(), projectile.damage / 2, 5, projectile.owner);
            }
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override bool? CanHitNPC(NPC target)
        {
            if ((int)projectile.ai[0] == target.whoAmI && projectile.timeLeft < 300)
                return true;
            else
                return false;
        }

        public int FindNewTarget()
        {
            projectile.netUpdate = true;

            int npc = TerraLeague.GetClosestNPC(projectile.Center, 600, (int)projectile.ai[0]);

            if (npc != -1)
            {
                Main.npc[npc].immune[projectile.owner] = 0;
                return npc;
            }
            else
            {
                return -1;
            }
        }

        public override void Kill(int timeLeft)
        {
            if (projectile.penetrate == 0)
            {
                TerraLeague.PlaySoundWithPitch(projectile.Center, 2, 45, -0.5f);
            }

            base.Kill(timeLeft);
        }
    }
}
