using Microsoft.Xna.Framework;
using System;
using TerraLeague.Buffs;
using TerraLeague.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class DarkinBow_ArrowRain : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rain of Arrows");
        }

        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.alpha = 0;
            projectile.scale = 1f;
            projectile.timeLeft = 300;
            projectile.ranged = true;
            projectile.extraUpdates = 1;
            projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void AI()
        {
            Lighting.AddLight(projectile.Center, 0.5f, 0f, 0f);

            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;

            if (projectile.velocity.Y > 1)
                projectile.velocity.Y += 0.2f;
            else
                projectile.velocity.Y += 0.1f;

            if (projectile.velocity.Y > 16f)
                projectile.velocity.Y = 16f;

            Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.Blood, 0, 0, 0, default(Color), 1f);
            dust.noGravity = true;
            dust.velocity *= 0f;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            target.AddBuff(BuffType<Slowed>(), 120);

            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Main.PlaySound(SoundID.Item10, projectile.position);
            return true;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.Blood, projectile.velocity.X / 2, projectile.velocity.Y / 2, 100, new Color(33, 66, 133), 0.5f);
            }

            base.Kill(timeLeft);
        }
    }
}
