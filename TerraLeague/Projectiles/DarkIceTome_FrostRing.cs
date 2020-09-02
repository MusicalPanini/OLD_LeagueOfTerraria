﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using TerraLeague.Buffs;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class DarkIceTome_FrostRing : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ring of Frost");
        }

        public override void SetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.timeLeft = 300;
            projectile.penetrate = 100;
            projectile.friendly = true;
            projectile.alpha = 255;
            projectile.magic = true;
            projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void AI()
        {
            if (projectile.timeLeft == 300)
                Prime();
            base.AI();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffType<Slowed>(), 300);

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override bool PreKill(int timeLeft)
        {
           
            return base.PreKill(timeLeft);
        }

        public override void Kill(int timeLeft)
        {
            TerraLeague.PlaySoundWithPitch(projectile.Center, 2, 82, -0.7f);

            for (int i = 0; i < 50; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 67, 0, 0, 50, default(Color), 1.5f);
                dust.velocity *= 2f;
                dust.noGravity = true;
                dust.noLight = true;
                dust.fadeIn = 2;
            }
            for (int i = 0; i < 50; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 67, 0, 0, 50, default(Color), 1);
                dust.velocity *= 2f;
                dust.noGravity = true;
            }

            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < 18; i++)
                {
                    Vector2 pos = new Vector2(350, 0).RotatedBy(MathHelper.ToRadians((20 * i) + (j * 6))) + projectile.Center;

                    Dust dustR = Dust.NewDustPerfect(pos, 113, Vector2.Zero, 0, default(Color), 1);
                    dustR.noGravity = true;
                    dustR.fadeIn = 1.5f;
                }
            }

            projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
            projectile.width = 10;
            projectile.height = 10;
            projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);

            base.Kill(timeLeft);
        }

        public override bool? CanHitNPC(NPC target)
        {
            return TerraLeague.IsHitboxWithinRange(projectile.Center, target.Hitbox, projectile.width/2);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public void Prime()
        {
            int size = 700;

            projectile.tileCollide = false;
            projectile.velocity = Vector2.Zero;
            projectile.alpha = 255;
            projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
            projectile.width = size;
            projectile.height = size;
            projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
            projectile.timeLeft = 2;
        }
    }
}
