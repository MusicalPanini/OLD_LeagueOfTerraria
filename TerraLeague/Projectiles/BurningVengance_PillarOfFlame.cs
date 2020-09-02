using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using TerraLeague.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class BurningVengance_PillarOfFlame : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.Homing[projectile.type] = true;
            DisplayName.SetDefault("Pillar Of Flame");
        }

        public override void SetDefaults()
        {
            projectile.width = 160;
            projectile.height = 8;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = -1;
            projectile.alpha = 255;
            projectile.scale = 1f;
            projectile.timeLeft = 1080;
            projectile.magic = true;
            projectile.extraUpdates = 24;
            projectile.tileCollide = false;
            projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void AI()
        {
            if (projectile.soundDelay == 0)
            {
                TerraLeague.PlaySoundWithPitch(projectile.Center, 2, 34, -0.5f);
                projectile.soundDelay = 48;
            }

            if (Main.rand.Next(0, 2) == 0)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.Fire, 0, 0, 0, default(Color), 4f);
                dust.noGravity = true;
                dust.velocity = new Vector2(0, -5f);
                dust.noLight = true;
                dust.fadeIn = 2;

                dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.Fire, 0, 0, 0, default(Color), 6f);
                dust.noGravity = true;
                dust.noLight = true;

                dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.Fire, 0, 3, 0, default(Color), 1f);
                dust.noLight = true;

            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
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

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return base.OnTileCollide(oldVelocity);
        }

        public override void Kill(int timeLeft)
        {
            base.Kill(timeLeft);
        }

        public override bool? CanHitNPC(NPC target)
        {
            return base.CanHitNPC(target);
        }
    }
}
