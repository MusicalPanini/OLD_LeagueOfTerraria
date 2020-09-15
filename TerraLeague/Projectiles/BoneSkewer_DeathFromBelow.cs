using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TerraLeague.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class BoneSkewer_DeathFromBelow : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Death From Below");
        }

        public override void SetDefaults()
        {
            projectile.width = 40;
            projectile.height = 40;
            projectile.alpha = 255;
            projectile.timeLeft = 100;
            projectile.penetrate = -1;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.melee = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = false;
            projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void AI()
        {
            if ((int)projectile.ai[1] != 1)
            {
                projectile.rotation = -MathHelper.PiOver4 * (int)projectile.ai[0];
                if ((int)projectile.ai[0] == 1)
                {
                    projectile.spriteDirection = -1;
                }

                if (projectile.alpha < 0)
                {
                    projectile.alpha = 0;
                    projectile.friendly = true;
                    projectile.timeLeft = 54;
                    projectile.velocity = new Vector2(-10 * (int)projectile.ai[0], -10);
                    projectile.extraUpdates = 7;
                    projectile.ai[1] = 1;
                    TerraLeague.PlaySoundWithPitch(projectile.Center, 2, 71, -0.5f);
                }

                projectile.alpha -= 15;
            }

            if (projectile.timeLeft <= 30 && (int)projectile.ai[1] == 1)
            {
                projectile.velocity *= 0;
                projectile.extraUpdates = 0;
                projectile.friendly = false;
                projectile.alpha += 255 / 30;
            }

            if (Main.rand.Next(0, 3) == 0)
            {
                Dust dustIndex = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 67, 0f, 0f, 100, default(Color), 1f);
                dustIndex.noGravity = true;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 27), projectile.position);

            return true;
        }

        public override void Kill(int timeLeft)
        {
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (target.life <= 0)
            {
                Main.player[projectile.owner].AddBuff(BuffType<DeathFromBelowRefresh>(), 600);
            }

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}
