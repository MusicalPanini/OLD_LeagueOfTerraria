using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.Achievements;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class MagicCards_BlueCard : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blue Card");
        }

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.timeLeft = 300;
            projectile.penetrate = 1;
            projectile.aiStyle = 2;
            projectile.friendly = true;
            projectile.magic = true;
            
        }

        public override void AI()
        {
            Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 261, 0, 0, 0, new Color(0, 0, 255));
            dust.noGravity = true;
            dust.scale = 1f;
            dust.velocity *= 0.1f;

            base.AI();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Main.player[projectile.owner].ManaEffect(25);
            Main.player[projectile.owner].statMana += 25;
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
            Microsoft.Xna.Framework.Audio.SoundEffectInstance sound = Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 4), projectile.Center);
            

            for (int i = 0; i < 12; i++)
            {
                Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 261, projectile.velocity.X * 0.25f, projectile.velocity.Y * 0.25f, 0, new Color(0, 0, 255));
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
            
            Main.PlaySound(SoundID.Item1, projectile.position);
            return true;
        }
    }
}
