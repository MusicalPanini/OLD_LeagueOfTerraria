using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.Achievements;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class MagicCards_GreenCard : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magic Card");
        }

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.timeLeft = 180;
            projectile.penetrate = -1;
            projectile.aiStyle = 2;
            projectile.friendly = true;
            projectile.magic = true;
        }

        public override void AI()
        {
            if (projectile.ai[0] == 2)
            {
                projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
                if (projectile.timeLeft == 180)
                {
                    projectile.timeLeft = 60;
                    projectile.aiStyle = 0;
                    projectile.tileCollide = false;
                }

                if (projectile.velocity.X > 0)
                    projectile.rotation += 0.5f;
                else
                    projectile.rotation -= 0.5f;

                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 261, 0, 0, 0, new Color(0, 255, 0));
                dust.noGravity = true;
                dust.scale = 1f;
                dust.velocity *= 0.1f;
            }
            else
            {
                if (projectile.timeLeft == 180)
                {
                    projectile.penetrate = 1;
                }
            }

            base.AI();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 12; i++)
            {
                Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 261, projectile.velocity.X * 0.25f, projectile.velocity.Y * 0.25f, 0, new Color(0, 255, 0));
            }

            base.Kill(timeLeft);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = height = 10;
            return true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Main.PlaySound(SoundID.Dig, projectile.Center);
            return true;
        }
    }
}
