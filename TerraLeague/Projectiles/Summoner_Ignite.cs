using Microsoft.Xna.Framework;
using System;
using TerraLeague.Buffs;
using TerraLeague.Items.SummonerSpells;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class Summoner_Ignite : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            
            DisplayName.SetDefault("Ignite");
        }

        public override void SetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = 1;
            projectile.alpha = 255;
            projectile.scale = 1.2f;
            projectile.timeLeft = 301;
            projectile.extraUpdates = 120;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
        }

        public override void AI()
        {
            if(projectile.soundDelay == 0)
                Main.PlaySound(new LegacySoundStyle(2, 73), projectile.Center);
            projectile.soundDelay = 100;

            if (!Main.npc[(int)projectile.ai[0]].active)
            {
                projectile.Kill();
            }
            else
            {
                projectile.timeLeft = 300;

                if (projectile.localAI[0] == 0f)
                {
                    AdjustMagnitude(ref projectile.velocity);
                    projectile.localAI[0] = 1f;
                }
                Vector2 move = Vector2.Zero;

                NPC npc = Main.npc[(int)projectile.ai[0]];

                Vector2 newMove = npc.Center - projectile.Center;
                float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                move = newMove;
                AdjustMagnitude(ref move);
                projectile.velocity = (10 * projectile.velocity + move) / 20f;
                AdjustMagnitude(ref projectile.velocity);

                Dust dust = Dust.NewDustPerfect(projectile.position, DustID.Fire, Vector2.Zero, 0, default(Color), 1f);
                dust.noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffType<Ignited>(), IgniteRune.debuffDuration * 60);
            target.AddBuff(BuffType<GrievousWounds>(), IgniteRune.debuffDuration * 60);

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.Fire,0,0,0,default(Color),2);
                dust.noGravity = true;
            }
            Main.PlaySound(SoundID.Dig, projectile.Center);

            base.Kill(timeLeft);
        }

        public override bool? CanHitNPC(NPC target)
        {
            if ((int)projectile.ai[0] == target.whoAmI)
                return true;
            else
                return false;
        }

        private void AdjustMagnitude(ref Vector2 vector)
        {
            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > 20f)
            {
                vector *= 8f / magnitude;
            }
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}
