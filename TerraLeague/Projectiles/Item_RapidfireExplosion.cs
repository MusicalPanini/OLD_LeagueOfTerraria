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
    public class Item_RapidfireExplosion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rapidfire Cannon");
        }

        public override void SetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.timeLeft = 300;
            projectile.penetrate = 100;
            projectile.friendly = true;
            projectile.alpha = 255;
            projectile.ranged = true;
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
            target.immune[projectile.owner] = 1; 

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            hitDirection = projectile.Center.X > target.Center.X ? -1 : 1;

            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(new LegacySoundStyle(2, 14), projectile.position);

            Dust dust;
            for (int i = 0; i < 15; i++)
            {
                dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 31, 0f, 0f, 100, default(Color), 1f);
                dust.velocity *= 1.4f;
            }
            for (int i = 0; i < 10; i++)
            {
                dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 1.5f);
                dust.noGravity = true;
                dust.velocity *= 5f;

                dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 1f);
                dust.velocity *= 3f;
            }
            for (int g = 0; g < 3; g++)
            {
                Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
            }

            projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
            projectile.width = 10;
            projectile.height = 10;
            projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
            base.Kill(timeLeft);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public void Prime()
        {
            int size = (int)projectile.ai[1] == 0 ? 90 : 135;

            projectile.tileCollide = false;
            projectile.velocity = Vector2.Zero;
            projectile.alpha = 255;
            projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
            projectile.width = size;
            projectile.height = size;
            projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
            projectile.timeLeft = 1;
        }
    }
}
