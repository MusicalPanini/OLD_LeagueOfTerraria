using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TerraLeague.Buffs;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Achievements;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class ChainedRocketHand_StaticField : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Static Field");
        }

        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 32;
            projectile.timeLeft = 300;
            projectile.penetrate = 100;
            projectile.friendly = true;
            projectile.alpha = 255;
            projectile.magic = true;
            projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void AI()
        {
            if (projectile.soundDelay == 0)
                Prime();
            projectile.soundDelay = 100;
            base.AI();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Confused, 300);

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override bool PreKill(int timeLeft)
        {
           
            return base.PreKill(timeLeft);
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(new LegacySoundStyle(3, 53), projectile.position);
            for (int i = 0; i < 100; i++)
            {
                Dust dust = Dust.NewDustDirect(new Vector2(projectile.Center.X - 175, projectile.Center.Y - 175), 350, 350, 261, 0, 0, 50, new Color(0, 255, 255), 1.5f);
                dust.velocity *= 15f;
                dust.noGravity = true;
                dust.noLight = true;
                dust.fadeIn = 2;
            }
            for (int i = 0; i < 100; i++)
            {
                Dust dust = Dust.NewDustDirect(new Vector2(projectile.Center.X - 175, projectile.Center.Y - 175), 350, 350, 261, 0, 0, 50, new Color(0, 255, 255), 1);
                dust.velocity *= 10f;
            }

            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < 18; i++)
                {
                    Vector2 pos = new Vector2(350, 0).RotatedBy(MathHelper.ToRadians((20 * i) + (j * 6))) + projectile.Center;

                    Dust dustR = Dust.NewDustPerfect(pos, 261, Vector2.Zero, 0, new Color(0, 255, 255), 1);
                    dustR.noGravity = true;
                    dustR.fadeIn = 1.5f;
                }
            }

            base.Kill(timeLeft);
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

        public override bool? CanHitNPC(NPC target)
        {
            if (target.townNPC)
                return false;
            return TerraLeague.IsHitboxWithinRange(projectile.Center, target.Hitbox, projectile.width / 2);
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}
