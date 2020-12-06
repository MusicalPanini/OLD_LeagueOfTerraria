using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class MagicCards_YellowCard : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Yellow Card");
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
            Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 261, 0, 0, 0, new Color(255, 255, 0));
            dust.noGravity = true;
            dust.scale = 1f;
            dust.velocity *= 0.1f;

            base.AI();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffType<Stunned>(), 120);
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
            TerraLeague.PlaySoundWithPitch(projectile.Center, 2, 37, 0.5f);

            for (int i = 0; i < 12; i++)
            {
                Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 261, projectile.velocity.X * 0.25f, projectile.velocity.Y * 0.25f, 0, new Color(255, 255, 0));
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
