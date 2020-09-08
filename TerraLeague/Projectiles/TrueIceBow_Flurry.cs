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
    public class TrueIceBow_Flurry : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("True Ice Arrow");
        }

        public override void SetDefaults()
        {
            projectile.arrow = true;
            projectile.width = 10;
            projectile.height = 10;
            projectile.alpha = 0;
            projectile.timeLeft = 1200;
            projectile.penetrate = 1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.ranged = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = false;
            projectile.aiStyle = 1;
            aiType = ProjectileID.WoodenArrowFriendly;
        }

        public override void AI()
        {
            if (projectile.soundDelay == 0)
                Main.PlaySound(SoundID.Item5, projectile.Center);
            projectile.soundDelay = 100;

            Dust dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 67, 0f, 0f, 100, default(Color));
            dust.noGravity = true;
            dust.velocity /= 3;
            Lighting.AddLight(projectile.position, 0f, 0f, 0.5f);
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffType<Slowed>(), 120);
            base.OnHitPlayer(target, damage, crit);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffType<Slowed>(), 120);
            base.OnHitNPC(target, damage, knockback, false);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Main.PlaySound(SoundID.Dig, projectile.Center);
            return base.OnTileCollide(oldVelocity);
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 80, 0f, 0f, 100, default(Color), 0.7f);
            }
        }
    }
}
