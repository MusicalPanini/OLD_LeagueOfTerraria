using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TerraLeague.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class HexCoreStaff_GravityField : ModProjectile
    {
        int radius = 200;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gravity Field");
        }

        public override void SetDefaults()
        {
            projectile.width = 128;
            projectile.height = 32;
            projectile.friendly = false;
            projectile.penetrate = -1;
            projectile.alpha = 0;
            projectile.timeLeft = 6 * 60;
            projectile.magic = true;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 600;
            projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void AI()
        {
            if (projectile.timeLeft <= 360 - 60)
            {
                TerraLeague.DustBorderRing(radius, new Vector2(projectile.Center.X - 2, projectile.Top.Y), 21, new Color(255, 0, 255), 2);
                Dust dust = Dust.NewDustDirect(new Vector2(projectile.Center.X - 8, projectile.Top.Y - 8), 16, 16, 21, 0, 0, 0, new Color(255, 0, 255), 2);
                dust.velocity *= 0.1f;
                dust.noGravity = true;

                dust = Dust.NewDustDirect(new Vector2(projectile.Center.X - radius, projectile.Top.Y - radius), radius * 2, radius * 2, 21, 0, 0, 0, new Color(255, 0, 255), 2);
                dust.velocity = TerraLeague.CalcVelocityToPoint(dust.position, new Vector2(projectile.Center.X - 2, projectile.Top.Y), 10);
                dust.noGravity = true;

                projectile.friendly = true;
                if (projectile.timeLeft % 30 == 0)
                {
                    Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 15), projectile.position);
                }
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Stunned>(), projectile.timeLeft);
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = height = 16;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 192, projectile.velocity.X / 2, projectile.velocity.Y / 2, 100, new Color(255, 192, 0), 0.5f);
            }

            base.Kill(timeLeft);
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            hitbox = new Rectangle((int)projectile.Center.X - radius, (int)projectile.Top.Y - radius, radius*2, radius*2);

            base.ModifyDamageHitbox(ref hitbox);
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (projectile.friendly && !target.townNPC)
                return TerraLeague.IsHitboxWithinRange(projectile.Center, target.Hitbox, radius);
            else
                return false;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}
